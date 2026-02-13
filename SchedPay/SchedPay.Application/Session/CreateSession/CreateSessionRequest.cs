using System;
using System.Collections.Generic;
using System.Text;

namespace SchedPay.Application.Sessions.CreateSession;
public class CreateSessionRequest
{
    public Guid ProfessionalId { get; init; }
    public Guid ClientId { get; init; }
    public Guid ServiceId { get; init; }
    public DateTimeOffset StartUtc { get; init; }
    public DateTimeOffset EndUtc { get; init; }
}
