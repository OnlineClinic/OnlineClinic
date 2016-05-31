using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyOnlineClinic.Web.Models;

namespace MyOnlineClinic.Web.Helper
{
   public interface IPaymentHelper
    {
       PaypalResponseModel PayWithCreditCard(CreditCardViewModel model);

       PaypalResponseModel CapturePayment(CaptureViewModel model);
       PaypalResponseModel RefundPayment(RefundViewModel model);
    }
}
