using DocuSync.Domain.Entities.Enums;
using DocuSync.Domain.Entities;
using DocuSync.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DocuSync.Infrastructure.Data.Repositories
{
    public class RequirementRepository : IRequirementRepository
    {
        private readonly DocuSyncDbContext _context;

        public RequirementRepository(DocuSyncDbContext context)
        {
            _context = context;
        }

        public async Task<Requirement> GetByIdAsync(Guid id)
        {
            return await _context.Requirements.FindAsync(id);
        }

        public async Task<IEnumerable<Requirement>> GetAllAsync()
        {
            return await _context.Requirements.ToListAsync();
        }

        public async Task<Requirement> AddAsync(Requirement entity)
        {
            await _context.Requirements.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Requirement entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Requirement entity)
        {
            _context.Requirements.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Requirement>> GetByClientAsync(Guid clientId)
        {
            return await _context.Requirements
                .Where(r => r.ClientId == clientId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Requirement>> GetByStatusAsync(RequirementStatus status)
        {
            return await _context.Requirements
                .Where(r => r.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Requirement>> GetOverdueAsync()
        {
            var today = DateTime.UtcNow.Date;
            return await _context.Requirements
                .Where(r => r.DueDate < today
                    && r.Status != RequirementStatus.Completed
                    && r.Status != RequirementStatus.Cancelled)
                .ToListAsync();
        }

        public async Task<IEnumerable<Requirement>> GetUpcomingAsync(int daysAhead)
        {
            var today = DateTime.UtcNow.Date;
            var futureDate = today.AddDays(daysAhead);

            return await _context.Requirements
                .Where(r => r.DueDate >= today
                    && r.DueDate <= futureDate
                    && r.Status != RequirementStatus.Completed
                    && r.Status != RequirementStatus.Cancelled)
                .OrderBy(r => r.DueDate)
                .ToListAsync();
        }
    }
}