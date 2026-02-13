using System;
using System.Collections.Generic;
using System.Text;

namespace SchedPay.Domain.Enums
{
    public enum PaymentStatus
    {
        Pending,
        Paid,
        Overdue,
        Cancelled
    }
}
