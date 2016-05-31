using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MyOnlineClinic.Entity;
using MyOnlineClinic.Web.Helper;
namespace MyOnlineClinic.Web.Models
{
    public class CityViewModel
    {
        public int CityId { get; set; }

        [Required(ErrorMessage = "Please enter city name")]
        public string CityName { get; set; }


        public int StateId { get; set; }

        //public List<LookUpProvince> ConcelhosList { get; set; }

        public string PostalCode { get; set; }

        public bool IsActive { get; set; }

        public int? CountryId { get; set; }

        public List<LookUpCountry> Countries { get; set; }

        public List<LookUpCity> Cities { get; set; }

        public List<LookUpState> States { get; set; }

        public CityViewModel()
        { }

        public CityViewModel(LookUpCity model)
        {
            this.CityId = model.LookUpCityId;
            this.CityName = model.LookUpCityName;
            this.CountryId = model.CountryId;
            this.IsActive = model.IsActive;
            this.StateId = model.LookUpProvinceId;

        }

        public LookUpCity GetEditModel(CityViewModel model)
        {
            LookUpCity cityModel = new LookUpCity();
            cityModel.LookUpCityId = model.CityId;
            cityModel.LookUpCityName = StringCipher.Encrypt(model.CityName);
            cityModel.CountryId = model.CountryId ?? 1;
            cityModel.IsActive = model.IsActive;
            cityModel.LookUpProvinceId = this.StateId;
            return cityModel;
        }


    }
}