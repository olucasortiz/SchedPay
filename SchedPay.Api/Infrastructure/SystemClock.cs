using SchedPay.Application.Common;

namespace SchedPay.Api.Infrastructure
{
    public sealed class SystemClock : IClock
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}
