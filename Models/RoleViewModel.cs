using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyOnlineClinic.Entity;
namespace MyOnlineClinic.Web.Models
{
    public class RoleViewModel
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int RoleType { get; set; }
        public List<LookUpUserRoleType> RoleTypeList { get; set; }
    }
}