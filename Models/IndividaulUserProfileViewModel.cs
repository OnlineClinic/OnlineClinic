using MyOnlineClinic.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyOnlineClinic.Web.Models
{
    public class IndividaulUserProfileViewModel
    {   
        public Guid LoginId { get; set; }
        public string LoginName { get; set; }
        public string LoginPassword { get; set; }
        public string LoginEmail { get; set; }
        public int LookUpSubscriptionId { get; set; }
        public string NIF { get; set; }
        public string MemberName { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string PostalLocal { get; set; }
        public short? CountryId { get; set; }
        public short? DistritosId { get; set; }
        public int? LookUpStateId { get; set; }
        public int? ConcelhosId { get; set; }
        public int FreguesiaId { get; set; }
        public string Address2 { get; set; }
        public string EmailId { get; set; }
        public string Email2 { get; set; }
        public string website { get; set; }
        public string telephone1 { get; set; }
        public string telephone2 { get; set; }
        public string Fax { get; set; }
        public string Description { get; set; }
        public RoleType RoleId { get; set; }
        public string Avatar { get; set; }
        public int MemberNumber { get; set; }
        public decimal? Capital { get; set; }
        public bool ShowTelephone1 { get; set; }
        public bool ShowTelephone2 { get; set; }
        public bool ShowFax { get; set; }
        public bool ShowEmail { get; set; }

        public DateTime CreationDate { get; set; }

        
        

    }
}