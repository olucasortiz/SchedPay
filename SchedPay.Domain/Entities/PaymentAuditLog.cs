using SchedPay.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchedPay.Domain.Entities
{
    public class PaymentAuditLog
    {
        public Guid Id { get; private set; }
        public Guid PaymentId { get; private set; }

        public PaymentStatus OldStatus { get; private set; }
        public PaymentStatus NewStatus { get; private set; }

        public DateTimeOffset ChangedAtUtc { get; private set; }
        public string ChangedBy { get; private set; }
        public string? Reason { get; private set; }

        public DateTimeOffset CreatedAtUtc { get; private set; }

        public PaymentAuditLog(Guid id, Guid paymentId, PaymentStatus oldStatus, PaymentStatus newStatus,
                               DateTimeOffset changedAtUtc, string changedBy, string? reason,DateTimeOffset createdAtUtc)
        {
            if(paymentId == Guid.Empty)
                throw new ArgumentException("PaymentId cannot be empty.", nameof(paymentId));
            if(string.IsNullOrWhiteSpace(changedBy))
                throw new ArgumentException("ChangedBy cannot be null or whitespace.", nameof(changedBy));
            if(oldStatus == newStatus)
                throw new ArgumentException("OldStatus and NewStatus cannot be the same.", nameof(oldStatus));
            if(reason is {  Length: > 500 })
                throw new ArgumentException("Reason cannot exceed 500 characters.", nameof(reason));
            Id = id;
            PaymentId = paymentId;
            OldStatus = oldStatus;
            NewStatus = newStatus;
            ChangedAtUtc = DateTimeOffset.UtcNow;
            ChangedBy = changedBy;
            Reason = string.IsNullOrWhiteSpace(reason) ? null:reason ;
            CreatedAtUtc = DateTimeOffset.UtcNow;
        }
    }
}
