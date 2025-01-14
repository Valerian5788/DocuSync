using DocuSync.Domain.Entities.Enums;
using DocuSync.Domain.Entities;
using DocuSync.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DocuSync.Infrastructure.Data.Repositories
{
    public class DocumentTypeRepository : IDocumentTypeRepository
    {
        private readonly DocuSyncDbContext _context;

        public DocumentTypeRepository(DocuSyncDbContext context)
        {
            _context = context;
        }

        public async Task<DocumentType> GetByIdAsync(Guid id)
        {
            return await _context.DocumentTypes.FindAsync(id);
        }

        public async Task<IEnumerable<DocumentType>> GetAllAsync()
        {
            return await _context.DocumentTypes.ToListAsync();
        }

        public async Task<DocumentType> AddAsync(DocumentType entity)
        {
            await _context.DocumentTypes.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(DocumentType entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(DocumentType entity)
        {
            _context.DocumentTypes.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<DocumentType>> GetByFrequencyAsync(DocumentFrequency frequency)
        {
            return await _context.DocumentTypes
                .Where(dt => dt.Frequency == frequency)
                .ToListAsync();
        }

        public async Task<bool> IsInUseAsync(Guid id)
        {
            return await _context.Requirements
                .AnyAsync(r => r.DocumentTypeId == id);
        }

        Task<DocumentType> IDocumentTypeRepository.UpdateAsync(DocumentType documentType)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}