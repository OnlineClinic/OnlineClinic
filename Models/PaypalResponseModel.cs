using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyOnlineClinic.Web.Models
{
    public class PaypalResponseModel
    {
        public string ResponseString { get; set; }
        public string Status { get; set; }
        public string StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public string CaptureId { get; set; }
        public string AuthorizationId { get; set; }
    }
}