using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyOnlineClinic.Entity;
using System.ComponentModel.DataAnnotations;


namespace MyOnlineClinic.Web.Models
{
    public class UserProfileViewModel
    {
        public Guid LoginId { get; set; }

        public string MemberName { get; set; }

        [Display(Name = "Fregusia")]
        public int FreguesiaId { get; set; }

        public string CompanyName { get; set; }

        public string Address { get; set; }

        public string Address2 { get; set; }

        public string PostalCode { get; set; }

        public string PostalLocal { get; set; }

        public short? CountryId { get; set; }

        [Display(Name = "Distrito")]
        public short? DistritosId { get; set; }

         [Display(Name = "Concelho")]
        public int ConcelhoId { get; set; }

        public string EmailId { get; set; }

        public string Email2 { get; set; }

        public string website { get; set; }

        public string telephone1 { get; set; }

        public string telephone2 { get; set; }

        public string Fax { get; set; }

        public RoleType RoleId { get; set; }

        public string LoginName { get; set; }

        public string Description { get; set; }

        public string Avatar { get; set; }

        public string NIF { get; set; }

        public DateTime CreationDate { get; set; }

        public string UserName { get; set; }

        public string UserNumber { get; set; }

        public List<LookUpCountry> CountryList { get; set; }

        //public List<LookUpState> StateList { get; set; }

        //public List<LookUpProvince> ConcelhoList { get; set; }

        //public List<LookUpCity> CityList { get; set; }

        //public Member GetIndividualUser(int memberID)
        //{
        //    Member profile = new IndividualUser
        //    {
        //       NIF = NIF,
        //       MemberId=memberID
        //    };
        //    profile = SetMember(profile,memberID);
        //    return profile;
        //}
        //public Member SetMember(Member member, int memberId)
        //{
        //    member.LoginId = LoginId;
        //    member.MemberId = memberId;
        //    member.MemberName = MemberName;
        //    member.EmailId = EmailId;
        //    member.CompanyName = CompanyName;
        //    member.CountryId = CountryId;
        //    member.ConcelhosId = ConcelhoId;
        //    member.DistritosId = DistritosId;
        //    member.FreguesiaId = FreguesiaId;
        //    member.telephone1 = telephone2;
        //    member.telephone2 = telephone1;
        //    member.Fax = Fax;
        //    member.Address = Address;
        //    member.PostalCode = PostalCode;
        //    member.PostalLocal = PostalLocal;
        //    member.Description = Description;
        //    member.website = website;
        //    member.Email2 = Email2;
        //    member.Address2 = Address2;
        //    return member;
        //}
    }
}