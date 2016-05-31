using MyOnlineClinic.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyOnlineClinic.Web.Models
{
    public class HomeVisitModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public int? Country { get; set; }
        public int? State { get; set; }
        public int? City { get; set; }
        public string PostCode { get; set; }
        public int PatientId { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }

        public string UsualGPName { get; set; }
        public string UsualGPClinic { get; set; }
        public string UsualGPAddress { get; set; }



        public List<LookUpState> StateList { get; set; }
        public List<LookUpCity> CityList { get; set; }
        public string Problem { get; set; }
        public DateTime? AppointmentDateLocal { get; set; }
        public int AppointmentStatus { get; set; }
       
        
        
        public int MemberId { get; set; }
        //public string language { get; set; }
        //public string postcode { get; set; }
        //public int AppointmentType { get; set; }
        //public string firstname { get; set; }
        //public string surname { get; set; }
        //public Guid PatientId { get; set; }
        //public int org { get; set; }
        //public int type { get; set; }
        //public int cID { get; set; }

        //public DateTime? AppointmentDateUtc { get; set; }
        //[Required(ErrorMessage = "Please select Appointment Date")]
        //public string Timezone { get; set; }
        public string Confirm { get; set; }
        //public string DoctorProfile { get; set; }
        //public string ProfilePic { get; set; }
        //public string Qualification { get; set; }
        //public string PhoneNo { get; set; }
        //public List<ProfessionTypes> ProfessionTypes { get; set; }
        //public string Profession { get; set; }
        //public Guid PatientLoginId { get; set; }
        //public Guid DoctorLoginId { get; set; }
    }
}