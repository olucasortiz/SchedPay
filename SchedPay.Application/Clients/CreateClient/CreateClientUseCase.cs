using SchedPay.Application.Repositories;
using SchedPay.Application.Common;
using SchedPay.Domain.Entities;
namespace SchedPay.Application.Clients.CreateClient
{
    public sealed class CreateClientUseCase
    {
        private readonly IClientRepository _clientRepository;
        private readonly IClock _clock; 

        public CreateClientUseCase(IClientRepository clientRepository, IClock clock)
        {
            _clientRepository = clientRepository;
            _clock = clock;
        }

        public async Task<CreateClientResponse> ExecuteAsync(CreateClientRequest request, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentException("Client name cannot be empty.");
            if(string.IsNullOrEmpty(request.ContactInfo))
                throw new ArgumentException("Contact information cannot be empty.");

            var client = new Client(id: Guid.NewGuid(),
                name: request.Name, contactInfo: request.ContactInfo, createdAtUtc: _clock.UtcNow);
            await _clientRepository.AddAsync(client, ct);
            return new CreateClientResponse(client.Id);
        }
    }
}
