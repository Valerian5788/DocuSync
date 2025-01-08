using DocuSync.Domain.Entities.Enums;
using DocuSync.Domain.Entities;
using DocuSync.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DocuSync.Infrastructure.Data.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly DocuSyncDbContext _context;

        public ClientRepository(DocuSyncDbContext context)
        {
            _context = context;
        }

        public async Task<Client> GetByIdAsync(Guid id)
        {
            return await _context.Clients.FindAsync(id);
        }

        public async Task<IEnumerable<Client>> GetAllAsync()
        {
            return await _context.Clients.ToListAsync();
        }

        public async Task<Client> AddAsync(Client entity)
        {
            await _context.Clients.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Client entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Client entity)
        {
            _context.Clients.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<Client> GetByHorusEmailAsync(string horusEmail)
        {
            return await _context.Clients
                .FirstOrDefaultAsync(c => c.HorusEmail == horusEmail);
        }

        public async Task<IEnumerable<Client>> GetActiveClientsAsync()
        {
            return await _context.Clients
                .Where(c => c.Status == ClientStatus.Active)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(string horusEmail)
        {
            return await _context.Clients
                .AnyAsync(c => c.HorusEmail == horusEmail);
        }
    }
}
