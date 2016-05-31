using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyOnlineClinic.Entity;

namespace MyOnlineClinic.Web.Models
{
    public class AvailabilityDaysModel
    {
        public int AvailabilityId { get; set; }
        public int DayID { get; set; }
        public AvailabilityDaysModel() { }
        public DayOfWeekEnum Availability { get; set; }
        public AvailabilityDaysModel(AvailabilityDaysModel model)
        {
            this.AvailabilityId = model.AvailabilityId;
            this.DayID = model.DayID;
        }
       
        //public AvailabilityDaysModel GetEditModel(DoctorAvailabilityModel model, int doctorID)
        //{
        //    DoctorAvailability rModel = new DoctorAvailability();
        //    rModel.DoctorID = doctorID;
        //    rModel.FromDate = model.FromDate;
        //    rModel.ToDate = model.ToDate;
        //    rModel.FromTime = model.FromTime;
        //    rModel.ToTime = model.ToTime;
        //    rModel.AvailabilityType = model.AvailabilityType;
        //    rModel.ClinicID = model.ClinicID;
        //    //rModel.TimeZone = model.TimeZone;
        //    rModel.TimeZone = model.TimeZoneId;
        //    return rModel;
        //}
    }

      
}