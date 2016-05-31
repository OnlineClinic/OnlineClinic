using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyOnlineClinic.Web.Models
{
    public class DoctorClinicModel
    {
        public int ClinicId
        {
            get;
            set;
        }
        public string ClinicName
        {
            get;
            set;
        }
        public string ClinicAddress1
        {
            get;
            set;
        }
        public string ClinicAddress2
        {
            get;
            set;
        }
        public string Postcode
        {
            get;
            set;
        }
        public string PhoneNumber
        {
            get;
            set;
        }
        public string CityName
        {
            get;
            set;
        }
        public string StateName
        {
            get;
            set;
        }
        public string CountryName
        {
            get;
            set;
        }
        public bool Checked
        {
            get;
            set;
        }
        public string ClinicVoucherNo
        {
            get;
            set;
        }  
    }
}