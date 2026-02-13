using System;
using System.Collections.Generic;
using System.Text;

namespace SchedPay.Domain.ValueObjects;
public class TimeRange
{
    public DateTimeOffset? EndUtc { get; }
    public DateTimeOffset? StartUtc { get; }

    public TimeRange(DateTimeOffset? startUtc, DateTimeOffset? endUtc)
    {
        if(endUtc <= startUtc)
            throw new ArgumentException("End time must be greater than start time.");
        StartUtc = startUtc;
        EndUtc = endUtc;
    }

    public bool Overlaps(TimeRange other)
    {
        return StartUtc < other.EndUtc && EndUtc > other.StartUtc;
    }
}
