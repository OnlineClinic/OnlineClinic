using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyOnlineClinic.Entity;
using MyOnlineClinic.Web.Helper;
namespace MyOnlineClinic.Web.Models
{
    public class GenderViewModel
    {
        public int GenderId { get; set; }

        [Required(ErrorMessage="Please enter Gender name")]
        public string GenderName { get; set; }

        [Required(ErrorMessage = "Please Select Gender Type")]

        public int GenderType { get; set; }
        
        public string TypeName { get; set; }

        public List<SelectListItem> GenderTypes { get; set; }

        public bool IsActive { get; set; }
        
        public GenderViewModel()
        { }

        public GenderViewModel(LookUpGender model)
        {
            this.GenderId = model.GenderId;
            this.GenderName = model.GenderName;
            this.GenderType = model.Type;            
            this.IsActive = model.IsActive;
        }
        public LookUpGender GetEditModel(GenderViewModel model)
        {
            LookUpGender GenderModel = new LookUpGender();
            GenderModel.GenderId = model.GenderId;
            GenderModel.GenderName = StringCipher.Encrypt(model.GenderName);
            GenderModel.Type = model.GenderType;
            GenderModel.IsActive = model.IsActive;
            return GenderModel;
        }
    }
}