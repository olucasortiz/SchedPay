using SchedPay.Domain.Enums;

namespace SchedPay.Domain.Entities
{
    public class Payment
    {
        public Guid Id { get; private set; }
        public Guid ClientId { get; private set; }
        public Guid ScheduleId { get; private    set; }
        public PaymentStatus Status { get; private set; }
        public string Reference { get; private set; }
        public decimal Amount { get; private set; }
        public DateTimeOffset DueDateUtc { get; private set; }
        public DateTimeOffset CreatedAtUtc { get; private set; }
        public DateTimeOffset? PaidAtUtc { get; private set; }
        public DateTimeOffset? CancelledAtUtc { get; private set; }

        public Payment(Guid id, Guid clientId, Guid scheduleId, string reference, decimal amount, DateTimeOffset dueDateUtc, DateTimeOffset createdAtUtc)
        {
            if(amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.");
            if(string.IsNullOrWhiteSpace(reference))
                throw new ArgumentException("Reference is required.");
            Id = id;
            ClientId = clientId;
            ScheduleId = scheduleId;
            Reference = reference;
            Amount = amount;
            DueDateUtc = dueDateUtc;
            CreatedAtUtc = createdAtUtc;
            Status = PaymentStatus.Pending;
        }

        public void MarkPaid(DateTimeOffset nowUtc)
        {
            if(Status is PaymentStatus.Paid or PaymentStatus.Cancelled)
                throw new InvalidOperationException("Only pending or overdue payments can be marked as paid.");


            Status = PaymentStatus.Paid;
            PaidAtUtc = nowUtc;
        }

        public void Cancel(DateTimeOffset nowUtc)
        {
            if (Status == PaymentStatus.Cancelled)
                throw new InvalidOperationException("Payment already cancelled.");

            if (Status == PaymentStatus.Paid)
                throw new InvalidOperationException("Paid payment cannot be cancelled.");

            Status = PaymentStatus.Cancelled;
            CancelledAtUtc = nowUtc;
        }

        public void MarkOverdue(DateTimeOffset nowUtc)
        {
            if (Status != PaymentStatus.Pending)
                throw new InvalidOperationException("Only pending payments can be marked as overdue.");
            if (DueDateUtc > nowUtc)
                throw new InvalidOperationException("Payment cannot be marked as overdue before due date.");
            Status = PaymentStatus.Overdue;
        }
    }

}
