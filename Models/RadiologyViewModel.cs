using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyOnlineClinic.Web.Models
{
    public class RadiologyViewModel
    {
        public int RadiologyId { get; set; }
        public int AppointmentId { get; set; }
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public int RadiologyTypeId { get; set; }
        public string Allign { get; set; }
        public int RegionId { get; set; }
        public string RequestPrint { get; set; }
        public string ClinicalNotes { get; set; }
        public string CopyResult { get; set; }
        public string RadiologyTypeName { get; set; }
        public string RegionName { get; set; }
        public int Iscompleted { get; set; }
        public int RptNumber { get; set; }
        public int PrintToAdmin { get; set; }
        public int RequestNo { get; set; }
        public int IsSentToPatient { get; set; }  
        public string DoctorName { get; set; }
        public string PatientName { get; set; }
        public string OrganizationName { get; set; }
    }
}