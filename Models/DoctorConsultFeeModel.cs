using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyOnlineClinic.Web.Models
{
    public class DoctorConsultFeeModel
    {
        public int AppointmentType
        {
            get;
            set;
        }
        public decimal StandardFee
        {
            get;
            set;
        }
        public decimal LongFee
        {
            get;
            set;
        }
        public decimal VeryLongFee
        {
            get;
            set;
        }
        public int CurrencyType
        {
            get;
            set;
        }
        public decimal RepeatPrescription
        {
            get;
            set;
        }
    }
}