using DocuSync.Domain.Common;

namespace DocuSync.Domain.Repositories
{
    public interface IClientRepository : IRepository<Client>
    {
        Task<Client> GetByHorusEmailAsync(string horusEmail);
        Task<IEnumerable<Client>> GetActiveClientsAsync();
        Task<bool> ExistsAsync(string horusEmail);
    }
}