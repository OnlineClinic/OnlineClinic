using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyOnlineClinic.Web.Models
{
    public class CreditCardViewModel
    {
        /// <summary>
        /// ID of the credit card being saved for later use.
        /// </summary>       
        public string id { get; set; }

        /// <summary>
        /// Card number.
        /// </summary>        
        public string number { get; set; }

        /// <summary>
        /// Type of the Card (eg. Visa, Mastercard, etc.).
        /// </summary>       
        public string type { get; set; }

        /// <summary>
        /// 2 digit card expiry month.
        /// </summary>       
        public int expire_month { get; set; }

        /// <summary>
        /// 4 digit card expiry year
        /// </summary>       
        public int expire_year { get; set; }

        /// <summary>
        /// Card validation code. Only supported when making a Payment but not when saving a credit card for future use.
        /// </summary>       
        public string cvv2 { get; set; }

        /// <summary>
        /// Card holder's first name.
        /// </summary>      
        public string first_name { get; set; }

        /// <summary>
        /// Card holder's last name.
        /// </summary>       
        public string last_name { get; set; }

        /// <summary>
        /// Billing Address associated with this card.
        /// </summary>        
        public Address billing_address { get; set; }

        /// <summary>
        /// A unique identifier of the payer generated and provided by the facilitator. This is required when creating or using a tokenized funding instrument.
        /// </summary>        
        public string payer_id { get; set; }

        /// <summary>
        /// A unique identifier of the customer to whom this bank account belongs to. Generated and provided by the facilitator. This is required when creating or using a stored funding instrument in vault.
        /// </summary>        
        public string external_customer_id { get; set; }

        /// <summary>
        /// A user provided, optional convenvience field that functions as a unique identifier for the merchant on behalf of whom this credit card is being stored for. Note that this has no relation to PayPal merchant id
        /// </summary>       
        public string merchant_id { get; set; }

        /// <summary>
        /// A unique identifier of the bank account resource. Generated and provided by the facilitator so it can be used to restrict the usage of the bank account to the specific merchant.
        /// </summary>
        public string external_card_id { get; set; }

        /// <summary>
        /// State of the funding instrument.
        /// </summary>        
        public string state { get; set; }

        /// <summary>
        /// Resource creation time  as ISO8601 date-time format (ex: 1994-11-05T13:15:30Z) that indicates creation time.
        /// </summary>        
        public string create_time { get; set; }

        /// <summary>
        /// Resource creation time  as ISO8601 date-time format (ex: 1994-11-05T13:15:30Z) that indicates the updation time.
        /// </summary>        
        public string update_time { get; set; }

        /// <summary>
        /// Date/Time until this resource can be used fund a payment.
        /// </summary>        
        public string valid_until { get; set; }

        public string VoucherNumber { get; set; }

        public int AppointmentId { get; set; }

        public int PatientId { get; set; }
        public Guid LoginId { get; set; }
      
        public Guid PatientLoginId { get; set; }
        public Guid DoctorLoginId { get; set; }
        public int AppointmentType { get; set; }
        public int ConsultTimeId { get; set; }
        public decimal ConsultFee { get; set; }
        public string ConsultType { get; set; }
        public string Currency { get; set; }
        public bool Booked { get; set; }
    }

    public class CaptureViewModel
    {
        public string AuthorizationId { get; set; }
        public string Amount { get; set; }
        public string Currency { get; set; }
        public int PaymentHistoryId { get; set; }
    }
    public class RefundViewModel
    {
        public string CaptureId { get; set; }
        public string Amount { get; set; }
        public string Currency { get; set; }
        public string RefundId { get; set; }
    }
}