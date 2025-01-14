using DocuSync.Domain.Common;
using DocuSync.Domain.Entities;
using DocuSync.Domain.Entities.Enums;
using System.Xml.Linq;

namespace DocuSync.Domain.Repositories
{
    public interface IDocumentTypeRepository : IRepository<DocumentType>
    {
        Task<IEnumerable<DocumentType>> GetByFrequencyAsync(DocumentFrequency frequency);
        Task<bool> IsInUseAsync(Guid id);
        Task<IEnumerable<DocumentType>> GetAllAsync();
        Task<DocumentType?> GetByIdAsync(Guid id);
        Task<DocumentType> AddAsync(DocumentType documentType);
        Task<DocumentType> UpdateAsync(DocumentType documentType);
        Task DeleteAsync(Guid id);
    }
}