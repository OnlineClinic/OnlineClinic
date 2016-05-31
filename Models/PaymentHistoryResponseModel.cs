using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyOnlineClinic.Web.Models
{
    public class PaymentHistoryResponseModel
    {
        public string PatientOrganization { get; set; }
        public string DoctorOrganization { get; set; }
        public int PaymentHistoryId { get; set; }
        public string PatientName { get; set; }
        public string doctorname { get; set; }
        public string AppointmentDateUtc { get; set; }
        public string CaptureStatus { get; set; }
        public string RefundStatus { get; set; }
        public string AuthorizationId { get; set; }
        public decimal ConsultFee { get; set; }
    }
}