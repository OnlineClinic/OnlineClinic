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
    public class TitleViewModel
    {
        public int TitleId { get; set; }

        [Required(ErrorMessage="Please enter Title name")]
        public string TitleName { get; set; }

        [Required(ErrorMessage = "Please Select Title Type")]

        public int TitleType { get; set; }
        
        public string TypeName { get; set; }

        public bool IsActive { get; set; }

        public List<SelectListItem> TitleTypes { get; set; }

        public TitleViewModel()
        { }

        public TitleViewModel(LookUpTitle model)
        {
            this.TitleId = model.TitleId;
            this.TitleName = model.TitleName;
            this.TitleType = model.Type;            
            this.IsActive = model.IsActive;
        }

        public LookUpTitle GetEditModel(TitleViewModel model)
        {
            LookUpTitle titleModel = new LookUpTitle();
            titleModel.TitleId = model.TitleId;
            titleModel.TitleName = StringCipher.Encrypt(model.TitleName);
            titleModel.Type = model.TitleType;
            titleModel.IsActive = model.IsActive;
            return titleModel;
        }
    }
}