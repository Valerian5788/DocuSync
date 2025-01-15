using DocuSync.Domain.Entities;
using DocuSync.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocuSync.Infrastructure.Data.Repositories
{
    public class PortalCredentialsRepository : IPortalCredentialsRepository
    {
        private readonly DocuSyncDbContext _context;

        public PortalCredentialsRepository(DocuSyncDbContext context)
        {
            _context = context;
        }

        public async Task<PortalCredentials> GetByIdAsync(Guid id)
        {
            return await _context.PortalCredentials
                .Include(p => p.Client)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<PortalCredentials>> GetAllAsync()
        {
            return await _context.PortalCredentials
                .Include(p => p.Client)
                .ToListAsync();
        }

        public async Task<PortalCredentials> AddAsync(PortalCredentials entity)
        {
            await _context.PortalCredentials.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(PortalCredentials entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(PortalCredentials entity)
        {
            _context.PortalCredentials.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<PortalCredentials> GetByClientAsync(Guid clientId, string portalType)
        {
            return await _context.PortalCredentials
                .Include(p => p.Client)
                .FirstOrDefaultAsync(p =>
                    p.ClientId == clientId &&
                    p.PortalType == portalType);
        }

        public async Task<bool> ExistsForClientAsync(Guid clientId, string portalType)
        {
            return await _context.PortalCredentials
                .AnyAsync(p =>
                    p.ClientId == clientId &&
                    p.PortalType == portalType);
        }
    }
}
