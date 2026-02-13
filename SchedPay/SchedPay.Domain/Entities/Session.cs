using SchedPay.Domain.Enums;
using SchedPay.Domain.ValueObjects;
using System;

namespace SchedPay.Domain.Entities
{
    public class Session
    {
        public Guid Id { get; private set; }
        public Guid ProfessionalId { get; private set; }
        public Guid ClientId { get; private set; }
        public Guid ServiceId { get; private set; }
        public TimeRange TimeRange { get; private set; }
        public SessionStatus Status { get; private set; }
        public DateTimeOffset CreatedAtUtc { get; private set; }
        public DateTimeOffset? CancelledAtUtc { get; private set; }
        public DateTimeOffset? CompletedAtUtc { get; private set; }
        public DateTimeOffset? NoShowMarkedAtUtc { get; private set; }

        public Session(Guid professionalId, Guid clientId, Guid serviceId, TimeRange timeRange)
        {
            Id = Guid.NewGuid();
            ProfessionalId = professionalId;
            ClientId = clientId;
            ServiceId = serviceId;
            TimeRange = timeRange;
            Status = SessionStatus.Scheduled;
            CreatedAtUtc = DateTimeOffset.UtcNow;
        }
        public void MarkNoShow(DateTimeOffset markedAtUtc)
        {
            if(Status == SessionStatus.Scheduled)
            {
                if(markedAtUtc >= TimeRange.EndUtc)
                {
                   NoShowMarkedAtUtc = markedAtUtc;
                   Status = SessionStatus.NoShow;
                }
            }
        }
        public void Complete(DateTimeOffset completedAtUtc)
        {
            if (Status == SessionStatus.Scheduled)
            {
                if (completedAtUtc >= TimeRange.EndUtc)
                {
                    CompletedAtUtc = completedAtUtc;
                    Status = SessionStatus.Completed;
                }
            }
            else throw new ArgumentException("Only scheduled sessions can be completed.");
        }

        public void Cancel(DateTimeOffset cancelledAtUtc)
        {
            if(Status == SessionStatus.Scheduled)
            {
                CancelledAtUtc = cancelledAtUtc;
                Status = SessionStatus.Canceled;
            }
            else 
                throw new ArgumentException("Only scheduled sessions can be canceled.");
        }

    }
}
