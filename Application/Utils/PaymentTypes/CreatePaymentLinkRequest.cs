using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utils.PaymentTypes
{
    public record CreatePaymentLinkRequest
    (
        int userId,
        string productName,
        string description,
        double price,
        string returnUrl,
        string cancelUrl
    );
}
