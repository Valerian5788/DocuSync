using DocuSync.Domain.Common;
using DocuSync.Domain.Entities;
using DocuSync.Domain.Entities.Enums;

namespace DocuSync.Domain.Repositories
{
    public interface IRequirementRepository : IRepository<Requirement>
    {
        Task<IEnumerable<Requirement>> GetByClientAsync(Guid clientId);
        Task<IEnumerable<Requirement>> GetByStatusAsync(RequirementStatus status);
        Task<IEnumerable<Requirement>> GetOverdueAsync();
        Task<IEnumerable<Requirement>> GetUpcomingAsync(int daysAhead);
        Task<Requirement> GetByIdAsync(Guid id, CancellationToken cancellation = default);
        Task UpdateAsync(Requirement requirement, CancellationToken cancellation = default);
    }
}