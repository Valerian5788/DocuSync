using DocuSync.Application.Common.Results;
using DocuSync.Domain.Entities.Enums;
using DocuSync.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocuSync.Application.Services.Interfaces
{
    public interface IDocumentTypeService
    {
        Task<Result<IEnumerable<DocumentType>>> GetAllAsync();
        Task<Result<DocumentType>> CreateAsync(string name, DocumentFrequency frequency);
        Task<Result<bool>> DeleteAsync(Guid id);
    }
}
