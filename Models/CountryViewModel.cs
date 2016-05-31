using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MyOnlineClinic.Entity;
using MyOnlineClinic.Web.Helper;
namespace MyOnlineClinic.Web.Models
{
    public class CountryViewModel
    {

        public int CountryId { get; set; }

        [Required(ErrorMessage="Please enter company name")]
        public string CountryName { get; set; }

        public string CountryCode { get; set; }

        public bool IsActive { get; set; }

        public CountryViewModel()
        { }

        public CountryViewModel(LookUpCountry country)
        {
            this.CountryName = country.CountryName;
            this.CountryCode = country.CountryCode;
            this.CountryId = country.CountryId;
            this.IsActive = country.IsActive;
        }

        public LookUpCountry GetEditModel(LookUpCountry model)
        {
            model.CountryName = StringCipher.Encrypt(this.CountryName);
            model.CountryCode = this.CountryCode;
            model.CountryId = this.CountryId;
            model.IsActive = this.IsActive;
            model.IsDeleted = false;
            return model;
        }
    }
}