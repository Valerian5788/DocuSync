using DocuSync.Application.Common.Results;
using DocuSync.Domain.Entities;
using DocuSync.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocuSync.Application.Services.Interfaces
{
    public interface IRequirementService
    {
        Task<Result<IEnumerable<Requirement>>> GetActiveRequirementsAsync();
        Task<Result<IEnumerable<Requirement>>> GetActiveForClientAsync(Guid clientId);
        Task<Result<Requirement>> GetByIdAsync(Guid id);
        Task<Result<Requirement>> CreateAsync(Guid clientId, Guid documentTypeId, DateTime dueDate);
        Task<Result<bool>> UpdateStatusAsync(Guid id, RequirementStatus status);
    }
}
