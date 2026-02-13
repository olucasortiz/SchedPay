
using SchedPay.Application.Repositories;
using SchedPay.Domain.Entities;
using SchedPay.Domain.ValueObjects;
using SchedPay.Domain.Enums;
using System.Linq;

namespace SchedPay.Application.Sessions.CreateSession;
public class CreateSessionUseCase
{
    private readonly ISessionRepository _sessionRepository;

    public CreateSessionUseCase(ISessionRepository sessionRepository)
    {
        _sessionRepository = sessionRepository??
            throw new ArgumentNullException(nameof(sessionRepository));
    }
    public CreateSessionResponse Execute(CreateSessionRequest request)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));
        var timeRange = new TimeRange(request.StartUtc, request.EndUtc);
        var existingSession = _sessionRepository.ListByProfessionalId(request.ProfessionalId);

        var hasConflict = existingSession.Any(s => s.Status == SessionStatus.Scheduled && s.TimeRange.Overlaps(timeRange));
        if(hasConflict)
        {
            throw new InvalidOperationException("The professional has a conflicting session during the specified time range.");
        }
        var session = new Session(
            request.ProfessionalId,
            request.ClientId,
            request.ServiceId,
            timeRange);
        _sessionRepository.Add(session);

        return new CreateSessionResponse{SessionId = session.Id};

    }
}
