
using MyOnlineClinic.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyOnlineClinic.Web.Models
{
    public class LoginUserViewModel
    {
        
        public Guid LoginId { get; set; }
        public int MemberId { get; set; }
        public string FullName { get; set; }    
        public string EmailAddress { get; set; }
        public string UserRoleName { get; set; }
        public string UserName { get; set; }
        public string Avatar { get; set; }
        public bool IsSubscribed { get; set; }        
        public DateTime JoinedOn { get; set; }
        public string Permission { get; set; }
        public string OrganizationName { get; set; }
        public string Logo { get; set; }
        public string ModuleNames { get; set; }
        public bool IsAdmin { get; set; }
        public string ClinicName { get; set; }
        public int ClinicId { get; set; }
        public int OrganizationId { get; set; }
        public LoginUserViewModel()
        {
            ClinicName = string.Empty;
        }
    }
}