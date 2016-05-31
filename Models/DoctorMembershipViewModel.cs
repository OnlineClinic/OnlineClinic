using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyOnlineClinic.Web.Models
{
    public class DoctorMembershipViewModel
    {
        public int DoctorId { get; set; }

        public int MembershipPaidBy { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int VoucherID { get; set; }

        public string  VoucherNo { get; set; }
        public Guid LoginId { get; set; }
    }
}