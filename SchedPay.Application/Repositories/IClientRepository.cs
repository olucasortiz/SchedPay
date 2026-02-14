using SchedPay.Domain.Entities;

namespace SchedPay.Application.Repositories
{
    public interface IClientRepository
    {
        Task AddAsync(Client client, CancellationToken ct);
        Task<Client?> GetByIdAsync(Guid id, CancellationToken ct);
    }
}
