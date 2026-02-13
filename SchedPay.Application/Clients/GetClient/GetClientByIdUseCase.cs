using SchedPay.Application.Repositories;
using SchedPay.Domain.Entities;
namespace SchedPay.Application.Clients.GetClient
{
    public class GetClientByIdUseCase
    {
        private readonly IClientRepository _clientRepository;

        public GetClientByIdUseCase(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }
        public async Task<GetClientByIdResponse> ExecuteAsync(Guid id, CancellationToken ct)
        {   
            if(id == Guid.Empty){
                throw new ArgumentException("Client id is required.", nameof(id));
            }
            var client = await _clientRepository.GetByIdAsync(id, ct);
            if(client == null || client.IsDeleted)
                throw new InvalidOperationException("Client not found.");
            return new GetClientByIdResponse(
                client.Id,
                client.Name,
                client.ContactInfo
            );
        }
    }
}
