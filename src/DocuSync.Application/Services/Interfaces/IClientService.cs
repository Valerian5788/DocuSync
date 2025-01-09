using DocuSync.Application.Common.Results;
using DocuSync.Domain.Entities.Enums;
using DocuSync.Domain.Entities;

namespace DocuSync.Application.Services.Interfaces
{
    public interface IClientService
    {
        Task<Result<IEnumerable<Client>>> GetAllAsync();
        Task<Result<Client>> GetByIdAsync(Guid id);
        Task<Result<Client>> CreateAsync(string name, string horusEmail);
        Task<Result<Client>> UpdateAsync(Guid id, string name, string horusEmail, ClientStatus status);
        Task<Result<bool>> DeleteAsync(Guid id);
    }
}
