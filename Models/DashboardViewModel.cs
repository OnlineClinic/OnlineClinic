using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyOnlineClinic.Entity;

namespace MyOnlineClinic.Web.Models
{
    public class DashboardViewModel
    {
        public int ClinicCount { get; set; }
        public int OrganizationCount { get; set; }
        public int PatientCount { get; set; }
        public int DoctorCount { get; set; }
        public int AppointmentCount { get; set; }
        public List<PermissionInModule> ModuleList { get; set; }
    }
}