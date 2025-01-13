using DocuSync.Application.Common.Results;
using DocuSync.Application.Services.Interfaces;
using DocuSync.Domain.Entities;
using DocuSync.Domain.Entities.Enums;
using DocuSync.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocuSync.Application.Services
{
    public class RequirementService : IRequirementService
    {
        private readonly IRequirementRepository _repository;
        private readonly ILogger<RequirementService> _logger;

        public RequirementService(IRequirementRepository repository, ILogger<RequirementService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<Requirement>>> GetActiveForClientAsync(Guid clientId)
        {
            try
            {
                var requirements = await _repository.GetByClientAsync(clientId);
                var active = requirements.Where(r =>
                    r.Status != RequirementStatus.Completed &&
                    r.Status != RequirementStatus.Cancelled);

                return Result<IEnumerable<Requirement>>.Success(active);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting active requirements for client {ClientId}", clientId);
                return Result<IEnumerable<Requirement>>.Failure("Failed to get active requirements");
            }
        }

        public async Task<Result<Requirement>> GetByIdAsync(Guid id)
        {
            try
            {
                var requirement = await _repository.GetByIdAsync(id);
                if (requirement == null)
                    return Result<Requirement>.Failure("Requirement not found");

                return Result<Requirement>.Success(requirement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting requirement {Id}", id);
                return Result<Requirement>.Failure("Failed to get requirement");
            }
        }

        public async Task<Result<Requirement>> CreateAsync(Guid clientId, Guid documentTypeId, DateTime dueDate)
        {
            try
            {
                var requirement = new Requirement(clientId, documentTypeId, dueDate);
                await _repository.AddAsync(requirement);
                return Result<Requirement>.Success(requirement);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid requirement data");
                return Result<Requirement>.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating requirement");
                return Result<Requirement>.Failure("Failed to create requirement");
            }
        }

        public async Task<Result<bool>> UpdateStatusAsync(Guid id, RequirementStatus status)
        {
            try
            {
                var requirement = await _repository.GetByIdAsync(id);
                if (requirement == null)
                    return Result<bool>.Failure("Requirement not found");

                switch (status)
                {
                    case RequirementStatus.Received:
                        requirement.MarkAsReceived();
                        break;
                    case RequirementStatus.Validated:
                        requirement.MarkAsValidated();
                        break;
                    case RequirementStatus.Completed:
                        requirement.MarkAsCompleted();
                        break;
                    default:
                        return Result<bool>.Failure("Invalid status transition");
                }

                await _repository.UpdateAsync(requirement);
                return Result<bool>.Success(true);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid status transition for requirement {Id}", id);
                return Result<bool>.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating requirement status {Id}", id);
                return Result<bool>.Failure("Failed to update requirement status");
            }
        }

        public async Task<Result<IEnumerable<Requirement>>> GetActiveRequirementsAsync()
        {
            try
            {
                var requirements = await _repository.GetAllAsync();
                var active = requirements.Where(r =>
                    r.Status != RequirementStatus.Completed &&
                    r.Status != RequirementStatus.Cancelled);

                return Result<IEnumerable<Requirement>>.Success(active);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting active requirements");
                return Result<IEnumerable<Requirement>>.Failure("Failed to get active requirements");
            }
        }


    }
}
