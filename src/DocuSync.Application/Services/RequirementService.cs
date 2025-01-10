using DocuSync.Application.Services.Interfaces;
using DocuSync.Domain.Entities;
using DocuSync.Domain.Repositories;
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

        public RequirementService(IRequirementRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Requirement>> GetActiveRequirementsAsync()
        {
            // Implementation to get active requirements
            return await _repository.GetActiveAsync();
        }
    }
}
