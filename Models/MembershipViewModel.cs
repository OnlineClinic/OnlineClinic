using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MyOnlineClinic.Entity;
using MyOnlineClinic.Web.Helper;
namespace MyOnlineClinic.Web.Models
{
    public class MembershipViewModel
    {
        public int MemberShipId { get; set; }
        [Required(ErrorMessage = "Please enter name")]
        public string Name { get; set; }
        public int Duration { get; set; }
        public double Price { get; set; }
        public bool IsActive { get; set; }
        public DateTime ModifiedDate { get; set; }
        public MembershipViewModel()
        {
        }
        public MembershipViewModel(Membership model)
        {
            this.MemberShipId = model.Id;
            this.Name = model.Name;
            this.IsActive = model.IsActive;
            this.Duration = model.Duration;
            this.Price = model.Price;

        }

        public Membership GetEditModel(MembershipViewModel model)
        {
            Membership memberShipModel = new Membership();
            memberShipModel.Id = model.MemberShipId;
            memberShipModel.Name =StringCipher.Encrypt(model.Name);
            memberShipModel.IsActive = model.IsActive;
            memberShipModel.Price = model.Price;
            memberShipModel.Duration = model.Duration;
            return memberShipModel;
        }
    }
}