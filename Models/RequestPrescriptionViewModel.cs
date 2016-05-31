using MyOnlineClinic.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyOnlineClinic.Web.Helper;
namespace MyOnlineClinic.Web.Models
{
    public class RequestPrescriptionViewModel
    {
        public int PatientMedicationHistoryId { get; set; }
        public Guid PatientLoginid { get; set; }
        public Guid? DoctorLoginid { get; set; }
        public int? AppointmentId { get; set; }
        public int Medication_AmtId { get; set; }
        public string strenght { get; set; }
        public string type { get; set; }
        public string Dose { get; set; }
        public int? YearStarted { get; set; }
        public string Rout { get; set; }
        public string PRN { get; set; }
        public string Consideration { get; set; }
        public string NoNeeded { get; set; }
        public int PbsRpbsValue { get; set; }
        public bool IsBrandSubstitue { get; set; }
        public string AuthorityNumber { get; set; }
        public string AuthorityScriptNumber { get; set; }
        public bool HasPrint { get; set; }
        public bool PrinttoAdmin { get; set; }
        public bool IsCompleted { get; set; }
        public string RPTNumber { get; set; }
        public long ScriptId { get; set; }
        public int? quantity { get; set; }
        public string Medicineperiod { get; set; }
        public string RequestType { get; set; }
        public string QRCodeImage { get; set; }
        public string BARCodeImage { get; set; }
        public string AmtMedicationName { get; set; }
        public string Frequency { get; set; }
        public DateTime Modyifydate { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? Modifiedby { get; set; }
        //For Amt Medication
       
        public int ModuleId { get; set; }
        public string MedicationTerm { get; set; }

        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string Avtar { get; set; }
        public string OrganizatioName { get; set; }
        public int Clinicid { get; set; }
        public string ClinicName { get; set; }
        public string Ipaddress { get; set; }
        public DateTime? CreatedDateUTC { get; set; }

        public List<LookUpState> StateList { get; set; }
        public List<LookUpCountry> CountryList { get; set; }
        public List<LookUpCity> CityList { get; set; }


        //Add Request prescription

        public int RequestPrescriptionId { get; set; }
        public string MedicineId { get; set; }
        public bool SeenbyDoctor { get; set; }
        public bool SeenbyAdmin { get; set; }
        public int Doctorid { get; set; }
        public bool Isactive { get; set; }
        public bool IsApproved { get; set; }
        
    }
   
}