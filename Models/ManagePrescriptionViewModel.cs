using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyOnlineClinic.Web.Models
{
    public class ManagePrescriptionViewModel
    {
        public int ManagePrescriptionId { get; set; }
        public int AppointmentId { get; set; }
        public Guid PatientLoginid { get; set; }
        public Guid DoctorLoginid { get; set; }
        public int Status { get; set; }
        public int PrintFor { get; set; }
        public string PatientFullName { get; set; }
        public string DoctorFullName { get; set; }
        public DateTime AppointmentDatetime{ get; set; }
        public string OrganizatioName { get; set; }
        public string AppointmentStatus { get; set; }
        public int OrgId { get; set; }
        public List<ManagePrescriptionViewModel> AppointmentPrescription { get; set; }
        public List<ManagePrescriptionViewModel> MyPatientPrescription { get; set; }

    }
}