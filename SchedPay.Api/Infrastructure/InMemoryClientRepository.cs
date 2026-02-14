using System.Collections.Concurrent;
using SchedPay.Application.Repositories;
using SchedPay.Domain.Entities;

namespace SchedPay.Api.Infrastructure;

public sealed class InMemoryClientRepository : IClientRepository
{
    private readonly ConcurrentDictionary<Guid, Client> _clients = new();

    public Task AddAsync(Client client, CancellationToken ct)
    {
        _clients[client.Id] = client;
        return Task.CompletedTask;
    }

    public Task<Client?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        _clients.TryGetValue(id, out var client);
        return Task.FromResult(client);
    }
}
