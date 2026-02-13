using SchedPay.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchedPay.Application.Repositories
{
    public interface IClientRepository
    {
        Task AddAsync(Client client, CancellationToken ct);
        Task<Client?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<bool> ExistsActiveByIdAsync(Guid id, CancellationToken ct);
    }
}
