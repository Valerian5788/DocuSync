using DocuSync.Application.Common.Results;
using DocuSync.Application.Services.Interfaces;
using DocuSync.Domain.Entities.Enums;
using DocuSync.Domain.Entities;
using DocuSync.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocuSync.Application.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _repository;

        public ClientService(IClientRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<IEnumerable<Client>>> GetAllAsync()
        {
            var clients = await _repository.GetAllAsync();
            return Result<IEnumerable<Client>>.Success(clients);
        }

        public async Task<Result<Client>> GetByIdAsync(Guid id)
        {
            var client = await _repository.GetByIdAsync(id);
            if (client == null)
                return Result<Client>.Failure("Client not found");

            return Result<Client>.Success(client);
        }

        public async Task<Result<Client>> CreateAsync(string name, string horusEmail)
        {
            if (await _repository.ExistsAsync(horusEmail))
                return Result<Client>.Failure("A client with this email already exists");

            var client = new Client(name, horusEmail);
            await _repository.AddAsync(client);

            return Result<Client>.Success(client);
        }

        public async Task<Result<Client>> UpdateAsync(Guid id, string name, string horusEmail, ClientStatus status)
        {
            var client = await _repository.GetByIdAsync(id);
            if (client == null)
                return Result<Client>.Failure("Client not found");

            client.UpdateName(name);

            if (status == ClientStatus.Active)
                client.Activate();
            else
                client.Deactivate();

            await _repository.UpdateAsync(client);
            return Result<Client>.Success(client);
        }

        public async Task<Result<bool>> DeleteAsync(Guid id)
        {
            var client = await _repository.GetByIdAsync(id);
            if (client == null)
                return Result<bool>.Failure("Client not found");

            await _repository.DeleteAsync(client);
            return Result<bool>.Success(true);
        }
    }
}
