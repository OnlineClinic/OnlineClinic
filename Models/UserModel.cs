using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyOnlineClinic.Web.Models
{
    public class UserModel
    {
        public int MemberId { get; set; }
        public Guid LoginId { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string SurName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Suburb { get; set; }
        public string PostCode { get; set; }
        public int? CountryId { get; set; }
        public int? State { get; set; }
        public int? City { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
        public string MobileNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
    }
}