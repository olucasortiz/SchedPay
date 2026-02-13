using SchedPay.Domain.Entities;

namespace SchedPay.Application.Repositories
{
    public interface ISessionRepository
    {
        IReadOnlyList<Session> ListByProfessionalId(Guid professionalId);
        void Add(Session session);
    }
}
