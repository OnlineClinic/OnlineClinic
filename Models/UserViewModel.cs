
using MyOnlineClinic.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyOnlineClinic.Web.Models
{
    public class UserViewModel
    {
        public string EmailAddress { get; set; }
        public string LoginName { get; set; }
        public int MemberId { get; set; }
        public Guid LoginId { get; set; }
        
        public int? TitleId { get; set; }


        [Required(ErrorMessage = "Please enter first name")]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string SurName { get; set; }
        [Required(ErrorMessage = "Please enter address 1")]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public int OrgUsreRole { get; set; }
        public int OrgId { get; set; }
        public string TimezoneId { get; set; }
        public string Suburb { get; set; }
        public string PostCode { get; set; }
        public int? CountryId { get; set; }
        public int? State { get; set; }
        public int? City { get; set; }
        [Required(ErrorMessage = "Please select date of birth")]
        public DateTime DOB { get; set; }
        [Required(ErrorMessage = "Please select gender")]
        public string Gender { get; set; }
        public bool IsActive { get; set; }
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "Please enter phone number")]
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ProfilePic { get; set; }
        public string RoleName { get; set; }
        public List<string> Permissions { get; set; }
        public string TitleName { get; set; }
        public List<LookUpCountry> Countries { get; set; }
        public List<LookUpTitle> TitleList { get; set; }
        public List<LookUpCity> Cities { get; set; }
        public List<ModuleListModel> ModuleList { get; set; }
        public List<LookUpUserRoles> UserRoleList { get; set; }

        /// <summary>
        /// Adding this to pass the decrypted values on the add user form
        /// </summary>
        public List<RoleListViewModel> RoleList { get; set; }


        public List<LookUpState> StateList { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string CityName { get; set; }
        public UserViewModel()
        { }

        public UserViewModel(Login login)
        {
            this.EmailAddress =login.Email;
            this.FirstName = login.Member.FirstName;
            this.SurName = login.Member.SurName;
            this.MiddleName = login.Member.MiddleName;
            this.MobileNumber = login.Member.MobileNumber;
            this.PhoneNumber = login.Member.PhoneNumber;
            this.PostCode = login.Member.PostCode;
            this.Suburb = login.Member.Suburb;
            this.TitleId = login.Member.Title;
            this.TitleName = TitleName;
            this.DOB = login.Member.DOB;
            this.FaxNumber = login.Member.FaxNumber;
            this.Address1 = login.Member.Address1;
            this.Address2 = login.Member.Address2;
            this.MemberId = login.Member.MemberId;
            this.LoginName = login.LoginName;
            this.City = login.Member.City;
            this.State = login.Member.State;
            this.CountryId = login.Member.CountryId;
            this.ProfilePic = login.Avatar;
            this.LoginId = login.LoginId;

        }   

       
    }

    
}