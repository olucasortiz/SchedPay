using SchedPay.Domain.Enums;

namespace SchedPay.Domain.Entities
{
    public class PaymentSchedule
    {
        public Guid Id { get; private set; }

        public Guid ClientId { get; private set; }

        public ScheduleType Type { get; private set; }

        public RecurrencePattern? Pattern { get; private set; }

        public int? RecurrenceCount { get; private set; }

        public DateTimeOffset StartDateUtc { get; private set; }

        public DateTimeOffset CreatedAtUtc { get; private set; }

        public DateTimeOffset? DeletedAtUtc { get; private set; }

        public bool IsDeleted => DeletedAtUtc.HasValue;

        public PaymentSchedule(Guid id, Guid clientId, ScheduleType type, DateTimeOffset startDateUtc, DateTimeOffset createdAtUtc, RecurrencePattern? recurrencePattern = null, int? recurrenceCount = null)
        {
            if (type == ScheduleType.OneOff)
                if (recurrencePattern != null && recurrenceCount != null)
                    throw new InvalidOperationException("OneOff schedule cannot have recurrence.");
            if(type == ScheduleType.Recurring)
                if(recurrencePattern == null)
                    throw new InvalidOperationException("Recurring schedule must have a recurrence pattern.");
                if (recurrenceCount == null || recurrenceCount < 1 || recurrenceCount > 500)
                    throw new InvalidOperationException("Recurrence count must be between 1 and 500.");
            ClientId = clientId;
            Type = type;
            StartDateUtc = startDateUtc;
            CreatedAtUtc = createdAtUtc;
            Pattern = recurrencePattern;
            RecurrenceCount = recurrenceCount;
        }

        public List<Payment> GeneratePayments(decimal amount,Func<Guid> idGenerator,DateTimeOffset nowUtc)
        {   
            if(IsDeleted)
                throw new InvalidOperationException("Cannot generate payments for a deleted schedule.");
            if(amount <=0)
                throw new InvalidOperationException("Amount must be greater than zero.");
            if(StartDateUtc < nowUtc)
                throw new InvalidOperationException("Start date must be in the future.");
            var payments = new List<Payment>();
            if (Type == ScheduleType.OneOff)
            {
                var payment = new Payment(
                    id: idGenerator(),
                    clientId: ClientId,
                    scheduleId: Id,
                    reference: $"{Id:N}-001",
                    amount: amount,
                    dueDateUtc: StartDateUtc,
                    createdAtUtc: nowUtc
                );
                payments.Add(payment);
            }
            else if (Type == ScheduleType.Recurring)
            {
                if (Pattern == null || RecurrenceCount == null)
                    throw new InvalidOperationException("Recurring schedule must have a recurrence pattern and count.");

                var count = RecurrenceCount.Value;

                for (int i = 1; i <= count; i++)
                {
                    var offsetDays = Pattern.Value == RecurrencePattern.Weekly
                        ? 7 * (i - 1)
                        : 30 * (i - 1); // Monthly MVP = 30 fixo

                    var dueDate = StartDateUtc.AddDays(offsetDays);

                    var payment = new Payment(
                        id: idGenerator(),
                        clientId: ClientId,
                        scheduleId: Id,
                        reference: $"{Id:N}-{i:000}",
                        amount: amount,
                        dueDateUtc: dueDate,
                        createdAtUtc: nowUtc
                    );

                    payments.Add(payment);
                }


            }
            return payments;
        }
    }
}
