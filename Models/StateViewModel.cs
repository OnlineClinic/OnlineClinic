using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MyOnlineClinic.Entity;
using MyOnlineClinic.Web.Helper;
namespace MyOnlineClinic.Web.Models
{
    public class StateViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter state name")]
        public string StateName { get; set; }

        public int CountryId { get; set; }

        public List<LookUpCountry> Countries { get; set; }

        public List<LookUpState> States { get; set; }
        public bool IsActive { get; set; }
       

         public StateViewModel()
        { }

         public StateViewModel(LookUpState model)
        {
            this.Id = model.Id;
            this.StateName = model.StateName;
            this.CountryId = model.CountryId;
            this.IsActive = model.IsActive;
       
        }

         public LookUpState GetEditModel(LookUpState stateModel)
        {
           
            stateModel.Id = this.Id;
            stateModel.StateName = StringCipher.Encrypt(this.StateName);
            stateModel.CountryId = this.CountryId;
            return stateModel;
        }
    }
}