using DocuSync.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocuSync.Application.Services.Interfaces
{
    public interface IRequirementService
    {
        Task<IEnumerable<Requirement>> GetActiveRequirementsAsync();
    }
}
