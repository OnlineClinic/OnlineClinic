using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyOnlineClinic.Web.Models
{
    public class PathologyViewModel
    {
        public int PathologyId { get; set; }

        public int AppointmentId { get; set; }

        public Guid PatientLoginId { get; set; }

        public Guid DoctorLoginId { get; set; }

        public string TestName { get; set; }

        public string FastingNonFastingValue { get; set; }

        public string ClinicalNotes { get; set; }

        public string InstructionForPatient { get; set; }

        public string RequestNo { get; set; }

        public string Urgent { get; set; }

        public string CopyResultTo { get; set; }

        public bool IsCompleted { get; set; }

        public bool PrintToAdmin { get; set; }

        public bool IsSentToPatient { get; set; }

        public string DoctorName { get; set; }

        public string CreatedDate { get; set; }

        public string OrganizationName { get; set; }

        public string PatientName { get; set; }

        public int OrgId{ get; set; }
    }
}