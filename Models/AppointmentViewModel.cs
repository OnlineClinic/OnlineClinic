using MyOnlineClinic.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyOnlineClinic.Web.Models
{
    public class AppointmentViewModel
    {
        public int AppointmentId { get; set; }

        public int AppointmentType { get; set; }

        public Guid PatientLoginId { get; set; }

        public Guid DoctorLoginId { get; set; }

        public DateTime AppointmentDateUtc { get; set; }

        public DateTime AppointmentDateLocal { get; set; }

        public int AppointmentStatus { get; set; }

        public int PatientUserId { get; set; }
        public int countryid { get; set; }
        public int cityid { get; set; }
        public int stateid { get; set; }
        public string language { get; set; }
        public string postcode { get; set; }
        public string DoctorFullName { get; set; }
        public string DoctorProfile { get; set; }
        public string ProfilePic { get; set; }
        public string Qualification { get; set; }
        public string PhoneNo { get; set; }

        public string latitude { get; set; }
        public string longitude { get; set; }

        public string ProfessionName { get; set; }

        public List<Doctor> ListDoctor { get; set; }

        public List<AvailabilityDays> ListAvailabileDoctor { get; set; }
        public List<AvailabilityDays> AvailabileDoctorList { get; set; }
        public List<AvailabilityDays> OtherDoctorList { get; set; }
        public List<HomeVisitAppointment> HomeVisitDetail { get; set; }

        public int ProfessionType { get; set; }

        public List<SelectListItem> AppointmentTypeList { get; set; }
        public List<SelectListItem> ProfessionList { get; set; }
        public List<LookUpState> StateList { get; set; }
        public List<LookUpCountry> CountryList { get; set; }
        public List<LookUpCity> CityList { get; set; }


        public string AppointmentDateLocalText { get; set; }

        public string AppointmentTypeText { get; set; }
        public string AppointmentStatusName { get; set; }


        public string PatientName { get; set; }
        public int hdnAppId { get; set; }
        public int AppId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public int? Country { get; set; }
        public string CountryName { get; set; }
        public string OrganizationName { get; set; }
        public DateTime? FroMDate { get; set; }
        public DateTime? TOdate { get; set; }

        public string PatientTimezone { get; set; }

        public string PatientAddress { get; set; }
        public int? PatientCountry { get; set; }

        public string PatientProfilePic { get; set; }

        public string DoctorName { get; set; }
        public string DoctorTimezone { get; set; }

        public string DoctorAddress { get; set; }
        public int? DoctorCountry { get; set; }

        public string DoctorProfilePic { get; set; }
        public string PatientCountryname { get; set; }
        public string DoctorCountryname { get; set; }

        public string Location { get; set; }
        public string ADminTimezone { get; set; }
        public int Appointmentstatusid { get; set; }
        public bool IsSoapNotesAdded { get; set; }
        public bool IsActive { get; set; }
        public List<ReportProblemViewModel> ReportProblem { get; set; }
        public List<AppointmentViewModel> AppointmentList { get; set; }
        public List<CommunicationViewModel> ContactusList { get; set; }



        /// <summary>
        /// Using For DoctorId
        /// </summary>
        public int MemberId { get; set; }

        public AppointmentViewModel()
        {
            ListDoctor = new List<Doctor>();
            AppointmentDateLocal = DateTime.Now;
        }

        public AppointmentViewModel AppointmentSearchModel(AppointmentViewModel model)
        {
            this.DoctorFullName = model.DoctorFullName;
            this.ProfessionType = model.ProfessionType;
            this.countryid = model.countryid;
            this.cityid = model.cityid;
            this.stateid = model.stateid;
            this.language = language;
            this.postcode = postcode;
            this.ProfilePic = model.ProfilePic;
            this.Qualification = Qualification;
            this.PhoneNo = PhoneNo;
            this.DoctorProfile = DoctorProfile;
            return model;
        }
    }
    public class SearchViewModel
    {
        public List<SearchViewModel> AppointmentList { get; set; }
        public string DoctorFullName { get; set; }
        public List<SelectListItem> AppointmentTypeList { get; set; }
        public List<SelectListItem> ProfessionList { get; set; }
        public List<ProfessionSubType> ProfessionSubList { get; set; }
        public List<LookUpState> StateList { get; set; }
        public List<LookUpCountry> CountryList { get; set; }
        public List<LookUpCity> CityList { get; set; }
        public List<Clinic> ClinicList { get; set; }
        public int? countryid { get; set; }
        public int? cityid { get; set; }
        public int? stateid { get; set; }
        public int? Clinicid { get; set; }
        public int ProfessionType { get; set; }
        public int ConsultType { get; set; }
        public int ProfessionSub { get; set; }
        public int MemberId { get; set; }
        public string language { get; set; }
        public string postcode { get; set; }
        public int AppointmentType { get; set; }
        public string firstname { get; set; }
        public string surname { get; set; }
        public Guid PatientId { get; set; }
        public int org { get; set; }
        public int type { get; set; }
        public int AppointmentSelectionType { get; set; }
        public string TreatingDoctorName { get; set; }
        public int cID { get; set; }
        public List<Entity.Doctor> Search { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public string FromAMPM { get; set; }
        public string HdnDoctorId { get; set; }
        public DateTime? AppointmentDateUtc { get; set; }
        [Required(ErrorMessage = "Please select Appointment Date")]
        public DateTime? AppointmentDateLocal { get; set; }
        public string AppointmentDateLocalText { get; set; }
        public int AppointmentStatus { get; set; }
        public string Timezone { get; set; }
        public string Confirm { get; set; }
        public string DoctorProfile { get; set; }
        public string ProfilePic { get; set; }
        public string Qualification { get; set; }
        public string PhoneNo { get; set; }
        public List<ProfessionTypes> ProfessionTypes { get; set; }
        public string Profession { get; set; }
        public Guid PatientLoginId { get; set; }
        public Guid DoctorLoginId { get; set; }
        public string TimeZoneValue { get; set; }
        public string HIddenAppointmentType { get; set; }
        public string hdnAppointmentDateForAval { get; set; }
        public string hdnAppointmentTimeForAval { get; set; }
        public int hdnDocIdwhenAval { get; set; }
        public DateTime AvaliFromTime { get; set; }
        public DateTime AvaliTotime { get; set; }
        public int AppointmentId { get; set; }


        /// <summary>
        /// For Store Procedure
        public int DoctorUserId { get; set; }
        public string MobileNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public string ClinicName { get; set; }
        public int Doctorclinicid { get; set; }
        public string OrganizationName { get; set; }
        public int OrgId { get; set; }
        public string Avtar { get; set; }
        public Guid DoctorLogin { get; set; }
        public CreditCardViewModel Payment{ get; set; }


        public SearchViewModel AppointmentSearchModel(SearchViewModel model)
        {
            this.DoctorFullName = model.DoctorFullName;
            this.ProfessionType = model.ProfessionType;
            this.countryid = model.countryid;
            this.cityid = model.cityid;
            this.stateid = model.stateid;
            this.language = language;
            this.postcode = postcode;
            this.PatientId = PatientId;
            this.Timezone = Timezone;
            this.Clinicid = Clinicid;
            this.ProfessionSub = ProfessionSub;
            this.ConsultType = ConsultType;
            this.DoctorLoginId = DoctorLoginId;
            return model;
        }

        //public Appointment AdminEditMember()
        //{
        //    Appointment Appoint = new Appointment();
        //    Appoint = AdminSetMember(Appoint);
        //    return Appoint;

        //}
        //public Appointment AdminSetMember(Appointment Appointment)
        //{
        //    Appointment.AppointmentDateLocal = AppointmentDateLocal;
        //    Appointment.AppointmentDateUtc = AppointmentDateUtc;
        //    return Appointment;

        //}




    }

    public class PaymentViewModel
    {
        public Guid patientid { get; set; }
        public Guid DoctorId { get; set; }
        public int AppointmentId { get; set; }
        public int AppointmentType { get; set; }
        public int ConsultTime { get; set; }
        public bool AlreadyExists { get; set; }
        public bool Booked { get; set; }
        public string VoucherNo { get; set; }
    }

    public class TreatingdoctorsViewModel
    {
        public string firstname { get; set; }
        public string surname { get; set; }
        public int OrgId { get; set; }
        public int MemberId { get; set; }
    }
    public class DoctorSearchViewModel
    {
        public string DoctorName { get; set; }
        public string timeZone { get; set; }
        public int? countryId { get; set; }
        public int? stateId { get; set; }
        public string postalCode { get; set; }
        public string availableDateTime { get; set; }
        public string Language { get; set; }
        public string ConsultTime { get; set; }
        public int? Doctortypeid { get; set; }
        public int? ConsultType { get; set; }
        public int? ClinicID { get; set; }
        public Guid PatientLoginId { get; set; }
    }
    public class DoctorSearchResponseModel
    {
        // Doctorname, title, profilepic,consult fee , profesion type name, Complete Address postcode ,language ,qualification,DoctorProfile
        public string TitleName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string SurName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PostCode { get; set; }
        public string qualification { get; set; }
        public string LanguageSpoken { get; set; }
        public string DoctorProfile { get; set; }
        public string ProfessionName { get; set; }
        public string Avatar { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }
        public string ClinicName { get; set; }
        public string OrganizationName { get; set; }
        public string DoctorFullName { get; set; }
        public string AreaofIntrest { get; set; }
        public Guid DoctorLoginId { get; set; }
        public Guid LoginId { get; set; }
        public int DoctorUserId { get; set; }


        public List<DoctorClinicResponseModel> Clinics { get; set; }
    }
    public class DoctorClinicResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }

}