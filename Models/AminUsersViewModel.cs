using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyOnlineClinic.Web.Models
{
    public class AdminUsersViewModel
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public Guid LoginId { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
    }


    public class AddUsersViewModel
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public Guid LoginId { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
    }
}