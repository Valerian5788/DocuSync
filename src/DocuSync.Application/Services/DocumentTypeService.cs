using DocuSync.Application.Common.Results;
using DocuSync.Domain.Entities.Enums;
using DocuSync.Domain.Entities;
using DocuSync.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocuSync.Application.Services.Interfaces;

namespace DocuSync.Application.Services
{
    public class DocumentTypeService : IDocumentTypeService
    {
        private readonly IDocumentTypeRepository _repository;

        public DocumentTypeService(IDocumentTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<IEnumerable<DocumentType>>> GetAllAsync()
        {
            var types = await _repository.GetAllAsync();
            return Result<IEnumerable<DocumentType>>.Success(types);
        }

        public async Task<Result<DocumentType>> CreateAsync(string name, DocumentFrequency frequency)
        {
            var documentType = new DocumentType(name, frequency);
            await _repository.AddAsync(documentType);
            return Result<DocumentType>.Success(documentType);
        }

        public async Task<Result<bool>> DeleteAsync(Guid id)
        {
            if (await _repository.IsInUseAsync(id))
                return Result<bool>.Failure("Document type is in use and cannot be deleted");

            var documentType = await _repository.GetByIdAsync(id);
            if (documentType == null)
                return Result<bool>.Failure("Document type not found");

            await _repository.DeleteAsync(documentType);
            return Result<bool>.Success(true);
        }
    }
}
