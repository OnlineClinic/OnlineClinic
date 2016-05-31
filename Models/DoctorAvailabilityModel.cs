using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyOnlineClinic.Entity;
using System.Web.Mvc;
using MyOnlineClinic.Web.Helper;
namespace MyOnlineClinic.Web.Models
{
    public class DoctorAvailabilityModel
    {
        public int DoctorID { get; set; }
        public int AvailabilityId { get; set; }        
        public DateTime? FromDateLocal { get; set; }
        public DateTime? ToDateLocal { get; set; }
        public DateTime FromDateUTC { get; set; }
        public DateTime ToDateUTC { get; set; }
        public int AvailabilityType { get; set; }
        public int? ClinicID { get; set; }
        public string TimeZone { get; set; }
        public string TimeZoneId { get; set; }
        public string FullName { get; set; }
        public string ProfilePic { get; set; }

        public string FromDateLocalString { get; set; }
        public string ToDateLocalString { get; set; }

        public int CountryId { get; set; }
        public int State { get; set; }
        public int City { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
        public string OrgName { get; set; }
        public string PostCode { get; set; }
        public List<LookUpState> StateList { get; set; }
        public List<LookUpCountry> CountryList { get; set; }
        public List<LookUpCity> CityList { get; set; }
        public List<TimeZones> TimeZoneList { get; set; }

        public List<AvailabilityDays> AvailabilityDaysList { get; set; }

        public List<ClinicModel> Clinics { get; set; }
        public DayOfWeekEnum Weekdays { get; set; }
        public int DayId { get; set; }
        public string DayName { get; set; }

        public string DoctorName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public bool DayChecked { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }

        public string Status { get; set; }

        public string GeneratedCode { get; set; }
        
        public int AppointmentType { get; set; }
        public string AvailabilityTypeName { get; set; }
        public List<SelectListItem> AppointmentTypeList { get; set; }
        public IEnumerable<string> WeekDay { get; set; }
        public List<AvailabilityDays> Availabilitytimelist { get; set; }

        public List<DoctorAvailabilityModel> Type1 { get; set; }
        public List<DoctorAvailabilityModel> Type2 { get; set; }
        public List<DoctorAvailabilityModel> Type3 { get; set; }

        public DoctorAvailabilityModel()
        {
            WeekDay = new[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thrusday", "Friday", "Saturday" };
        }
        public DoctorAvailabilityModel(DoctorAvailability model)
        {
            this.AvailabilityId = model.AvailabilityId;
            this.DoctorID = model.DoctorID;
            this.FromDateLocal = model.FromDateLocal;
            this.ToDateLocal = model.ToDateLocal;
            this.FromDateUTC = model.FromDateUTC;
            this.ToDateUTC = model.ToDateUTC;
            this.AvailabilityType = model.AvailabilityType;
            //this.ClinicID = model.ClinicID;
            this.TimeZone = model.TimeZoneString;
            this.TimeZoneId = TimeZoneId;
            this.CountryId = model.CountryId;
            this.State = model.StateId;
            this.City = model.CityId;
            this.PostCode= model.Postcode;
        }

        public DoctorAvailability GetEditModel(DoctorAvailabilityModel model, int doctorID)
        {
            DoctorAvailability rModel = new DoctorAvailability();
            rModel.DoctorID = doctorID;
            rModel.FromDateLocal = Convert.ToDateTime(model.FromDateLocal);
            rModel.ToDateLocal = Convert.ToDateTime(model.ToDateLocal);
            rModel.FromDateUTC = model.FromDateUTC;
            rModel.ToDateUTC = model.ToDateUTC;
            rModel.AvailabilityType = model.AvailabilityType;
            rModel.CountryId= model.CountryId;
            rModel.StateId= model.State;
            rModel.CityId= model.City;
            rModel.Postcode = StringCipher.Encrypt(model.PostCode);
            rModel.TimeZoneString = model.TimeZoneId;
            return rModel;
        }

        public AvailabilityDays GetEditModelInner(DoctorAvailabilityModel model)
        {
            AvailabilityDays rModel = new AvailabilityDays();
            rModel.FromDateLocal = Convert.ToDateTime(model.FromDateLocal);
            rModel.ToDateLocal = Convert.ToDateTime(model.ToDateLocal);
            rModel.FromDateUTC = model.FromDateUTC;
            rModel.ToDateUTC = model.ToDateUTC;
            rModel.TimeZoneString = model.TimeZoneId;
            return rModel;
        }
    }

}