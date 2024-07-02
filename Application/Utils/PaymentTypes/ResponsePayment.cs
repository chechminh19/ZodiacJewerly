using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utils.PaymentTypes
{
    public record ResponsePayment
    (int error,
    String message,
    object? data);
}
