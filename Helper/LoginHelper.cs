using MyOnlineClinic.Entity;
using MyOnlineClinic.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyOnlineClinic.RepositoryService;
using MyOnlineClinic.Web.Controllers;
using System.IO;
using System.Drawing;
using MyOnlineClinic.Emailer;
using System.Net;

namespace MyOnlineClinic.Web.Helper
{
    public class LoginHelper : ILoginHelper
    {
        IUserService _userService;
        public LoginHelper(IUserService userService)
        { _userService = userService; }
        public Login GetOrganizationAdminLogin(BaseMember member, string userName, OrganizationViewModel model)
        {
            var timeZoneString = member.TimeZoneString;
            Login login = new Login
            {
                Email = StringCipher.Encrypt(userName),
                LoginName = StringCipher.Encrypt(userName),
                LookUpRoleId = (int)RoleType.OrganizationAdmin,
                Avatar = model.OrganizationUser.ProfilePic,
                IsActive = true,
                LoginId = Guid.NewGuid()
            };
            member.LoginId = login.LoginId;
            login.Member = member;
            login.Member.SetCreated(login.LoginId, member.TimeZoneString, false);
            login.SetCreated(login.LoginId, timeZoneString, false);
            return login;
        }

        public BaseMember GetOrganizationAdmin(OrganizationViewModel model, int orgId)
        {
            BaseMember profile = new OrganizationUsers
            {
                MemberId = model.OrganizationUser.OrgId,
                OrgUsreRole = model.OrganizationUser.OrgUsreRole,
                Title = model.OrganizationUser.Titleid,
                FirstName = StringCipher.Encrypt(model.OrganizationUser.FirstName),
                //FirstName = StringCipher.Encrypt(model.OrganizationUser.FirstName, StringCipher.PasswordPhrase),
                MiddleName = StringCipher.Encrypt(model.OrganizationUser.MiddleName),
                SurName = StringCipher.Encrypt(model.OrganizationUser.SurName),
                Address1 = StringCipher.Encrypt(model.OrganizationUser.Address1),
                Address2 = StringCipher.Encrypt(model.OrganizationUser.Address2),
                Suburb = StringCipher.Encrypt(model.OrganizationUser.Suburb),
                PostCode = StringCipher.Encrypt(model.OrganizationUser.PostCode),
                CountryId = model.OrganizationUser.CountryId,
                State = model.OrganizationUser.State,
                City = model.OrganizationUser.City,
                DOB = DateTime.Now,
                Gender = model.OrganizationUser.Genderid,
                MobileNumber = StringCipher.Encrypt(model.OrganizationUser.MobileNumber),
                PhoneNumber = StringCipher.Encrypt(model.OrganizationUser.PhoneNumber),
                FaxNumber = StringCipher.Encrypt(model.OrganizationUser.FaxNumber),
                OrgId = orgId,
                TimeZoneString = model.TimeZoneId
            };
            profile = SetMember(profile, model, orgId);
            return profile;
        }

        private BaseMember SetMember(BaseMember member, OrganizationViewModel model, int orgId)
        {
            member.FirstName = StringCipher.Encrypt(model.OrganizationUser.FirstName);
            member.Email = StringCipher.Encrypt(model.OrganizationUser.Email);
            member.SurName = StringCipher.Encrypt(model.OrganizationUser.SurName);
            member.CountryId = model.OrganizationUser.CountryId;
            member.City = model.OrganizationUser.City;
            member.Address1 = StringCipher.Encrypt(model.OrganizationUser.Address1);
            member.Address2 = StringCipher.Encrypt(model.OrganizationUser.Address2);
            member.Suburb = StringCipher.Encrypt(model.OrganizationUser.Suburb);
            member.PostCode = StringCipher.Encrypt(model.OrganizationUser.PostCode);
            member.CountryId = model.OrganizationUser.CountryId;
            member.State = model.OrganizationUser.State;
            member.DOB = DateTime.Now;
            member.Gender = model.OrganizationUser.Genderid;
            member.MobileNumber = StringCipher.Encrypt(model.OrganizationUser.MobileNumber);
            member.PhoneNumber = StringCipher.Encrypt(model.OrganizationUser.PhoneNumber);
            member.FaxNumber = StringCipher.Encrypt(model.OrganizationUser.FaxNumber);
            member.OrgId = orgId;
            member.TimeZoneString = model.TimeZoneId;
            return member;
        }

        public Login GetOrganiztionUserLogin(BaseMember member, string userName, OrganizationUserViewModel model, RoleType role)
        {
            Login login = new Login
            {
                Email = StringCipher.Encrypt(userName),
                LoginName = StringCipher.Encrypt(userName),
                LookUpRoleId = (int)role,
                Avatar = model.ProfilePic,
                IsActive = true,
            };
            var timeZoneString = member.TimeZoneString;
            member.LoginId = login.LoginId;
            login.Member = member;
            login.Member.SetCreated(login.LoginId, member.TimeZoneString, false);
            login.SetCreated(login.LoginId, timeZoneString, false);
            return login;

        }

        public Login GetAminUserLogin(BaseMember member, string userName, UserViewModel model, RoleType role)
        {
            Login login = new Login
            {
                Email = StringCipher.Encrypt(userName),
                LoginName = StringCipher.Encrypt(userName),

                LookUpRoleId = (int)role,
                Avatar = model.ProfilePic,
                IsActive = true,
            };
            var SelectTimeZone = member.TimeZoneString;
            member.LoginId = login.LoginId;
            login.Member = member;
            login.Member.SetCreated(login.LoginId, SelectTimeZone, false);
            login.SetCreated(login.LoginId, SelectTimeZone, false);

            return login;

        }

        public BaseMember GetClinicUser(dynamic model, int clinicId, int orgId)
        {
            BaseMember profile = new ClinicUsers
            {
                MemberId = model.OrgId,
                Title = model.Titleid,
                FirstName = StringCipher.Encrypt(model.FirstName),
                MiddleName = StringCipher.Encrypt(model.MiddleName),
                SurName = StringCipher.Encrypt(model.SurName),
                Address1 = StringCipher.Encrypt(model.Address1),
                Address2 = StringCipher.Encrypt(model.Address2),
                Suburb = StringCipher.Encrypt(model.Suburb),
                PostCode = StringCipher.Encrypt(model.PostCode),
                CountryId = model.CountryId,
                State = model.State,
                City = model.City,
                DOB = DateTime.Now,
                Gender = model.Gender,
                MobileNumber = StringCipher.Encrypt(model.MobileNumber),
                PhoneNumber = StringCipher.Encrypt(model.PhoneNumber),
                FaxNumber = StringCipher.Encrypt(model.FaxNumber),
                OrgId = orgId,
                ClinicId = clinicId,
                ClinicUsreRole = model.OrgUsreRole,
                TimeZoneString = model.TimeZoneId


            };
            profile = SetMember(profile, model, orgId);
            return profile;

        }

        public BaseMember GetUser(dynamic model, int orgId)
        {
            BaseMember profile = new OrganizationUsers
            {
                MemberId = model.OrgId,
                Title = model.Title,
                FirstName = StringCipher.Encrypt(model.FirstName),
                MiddleName = StringCipher.Encrypt(model.MiddleName),
                SurName = StringCipher.Encrypt(model.SurName),
                Address1 = StringCipher.Encrypt(model.Address1),
                Address2 = StringCipher.Encrypt(model.Address2),
                Suburb = StringCipher.Encrypt(model.Suburb),
                PostCode = StringCipher.Encrypt(model.PostCode),
                CountryId = model.CountryId,
                State = model.State,
                City = model.City,
                DOB = DateTime.Now,
                Gender = model.Gender,
                MobileNumber = StringCipher.Encrypt(model.MobileNumber),
                PhoneNumber = StringCipher.Encrypt(model.PhoneNumber),
                FaxNumber = StringCipher.Encrypt(model.FaxNumber),
                OrgId = orgId,
                OrgUsreRole = model.OrgUsreRole,
                TimeZoneString = model.TimeZone



            };
            profile = SetMember(profile, model, orgId);
            return profile;
        }

        public BaseMember GetAdminUser(dynamic model)
        {
            BaseMember profile = new AdminUser
            {
                MemberId = model.OrgId,
                Title = model.TitleId,
                FirstName = StringCipher.Encrypt(model.FirstName),
                MiddleName = StringCipher.Encrypt(model.MiddleName),
                SurName = StringCipher.Encrypt(model.SurName),
                Address1 = StringCipher.Encrypt(model.Address1),
                Address2 = StringCipher.Encrypt(model.Address2),
                Suburb = StringCipher.Encrypt(model.Suburb),
                PostCode = StringCipher.Encrypt(model.PostCode),
                CountryId = model.CountryId,
                State = model.State,
                City = model.City,
                DOB = DateTime.Now,
                Gender = model.Gender,
                MobileNumber = StringCipher.Encrypt(model.MobileNumber),
                PhoneNumber = StringCipher.Encrypt(model.PhoneNumber),
                FaxNumber = StringCipher.Encrypt(model.FaxNumber),
                TimeZoneString = model.TimezoneId
            };
            profile = SetAdminMember(profile, model);
            return profile;
        }

        private BaseMember SetAdminMember(BaseMember member, dynamic model)
        {

            member.FirstName = StringCipher.Encrypt(model.FirstName);
            member.Email = StringCipher.Encrypt(model.EmailAddress);
            member.SurName = StringCipher.Encrypt(model.SurName);
            member.CountryId = model.CountryId;
            member.City = model.City;
            member.Address1 = StringCipher.Encrypt(model.Address1);
            member.Address2 = StringCipher.Encrypt(model.Address2);
            member.Suburb = StringCipher.Encrypt(model.Suburb);
            member.PostCode = StringCipher.Encrypt(model.PostCode);
            member.CountryId = model.CountryId;
            member.State = model.State;
            member.DOB = DateTime.Now;
            member.Gender = model.Gender;
            member.MobileNumber = StringCipher.Encrypt(model.MobileNumber);
            member.PhoneNumber = StringCipher.Encrypt(model.PhoneNumber);
            member.FaxNumber = StringCipher.Encrypt(model.FaxNumber);
            member.Title = model.TitleId;
            return member;
        }

        private BaseMember SetMember(BaseMember member, dynamic model, int orgId)
        {
            member.Title = model.Titleid;
            member.FirstName = StringCipher.Encrypt(model.FirstName);
            member.MiddleName = StringCipher.Encrypt(model.MiddleName);
            member.Email = StringCipher.Encrypt(model.Email);
            member.SurName = StringCipher.Encrypt(model.SurName);
            member.CountryId = model.CountryId;
            member.City = model.City;
            member.Address1 = StringCipher.Encrypt(model.Address1);
            member.Address2 = StringCipher.Encrypt(model.Address2);
            member.Suburb = StringCipher.Encrypt(model.Suburb);
            member.PostCode = StringCipher.Encrypt(model.PostCode);
            member.CountryId = model.CountryId;
            member.State = model.State;
            member.DOB = DateTime.Now;
            member.Gender = model.Gender;
            member.MobileNumber = StringCipher.Encrypt(model.MobileNumber);
            member.PhoneNumber = StringCipher.Encrypt(model.PhoneNumber);
            member.FaxNumber = StringCipher.Encrypt(model.FaxNumber);
            member.OrgId = orgId;
            return member;
        }

        public Login GetClinicUserLogin(BaseMember member, string userName, ClinicUserViewModel model, RoleType role)
        {
            Login login = new Login
            {
                Email = StringCipher.Encrypt(userName),
                LoginName = StringCipher.Encrypt(userName),
                LookUpRoleId = (int)role,
                Avatar = model.ProfilePic,
                IsActive = true,
            };
            member.LoginId = login.LoginId;
            login.Member = member;
            var SelectTimeZone = member.TimeZoneString;
            login.Member.SetCreated(login.LoginId, SelectTimeZone, false);
            login.SetCreated(login.LoginId, SelectTimeZone, false);
            return login;
        }

        public dynamic UploadImage(dynamic model, HttpPostedFileBase profilePic)
        {
            if (profilePic != null && profilePic.ContentLength > 0)
            {
                var fileName = Path.GetFileName(profilePic.FileName);
                string extension = Path.GetExtension(fileName);
                if (extension != null && (extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".gif"))
                {
                    var imageHelper = new ImageHelper();
                    if (!string.IsNullOrEmpty(model.ProfilePic))
                    {
                        //Delete exising File
                        var file = HttpContext.Current.Server.MapPath(model.ProfilePic);
                        imageHelper.Delete(file);
                    }
                    string fName = Convert.ToString(DateTime.Now.ToString("ddMMyyyy-HHmmss-fff")) + extension;
                    var path = Path.Combine(HttpContext.Current.Server.MapPath("~/UploadedFiles/UserProfile/"), fName);
                    var bitmap = new Bitmap(profilePic.InputStream);
                    imageHelper.Save(bitmap, 150, 150, 250, path);
                    model.ProfilePic = "/UploadedFiles/UserProfile/" + fName;
                }
                else
                {
                    return model;
                }
            }
            else
            {
                if (model.ProfilePic != null)
                {
                    model.ProfilePic = model.ProfilePic;
                }

            }
            return model;
        }

        public void UpdateProfile(UserViewModel model, HttpPostedFileBase profilePic, Guid loginId)
        {
            var updateModel = _userService.Find(model.LoginId);
            //updateModel.Member.FirstName = model.FirstName;
            //updateModel.Member.MiddleName = model.MiddleName;
            //updateModel.Member.SurName = model.SurName;
            //updateModel.Member.Suburb = model.Suburb;
            //updateModel.Member.Title = model.TitleId;
            //updateModel.Member.State = model.State;
            //updateModel.Member.CountryId = model.CountryId;
            //updateModel.Member.City = model.City;
            //updateModel.Member.Address1 = model.Address1;
            //updateModel.Member.Address2 = model.Address2;
            //updateModel.Member.DOB = model.DOB;
            //updateModel.Member.PhoneNumber = model.PhoneNumber;
            //updateModel.Member.MobileNumber = model.MobileNumber;
            //updateModel.Member.FaxNumber = model.FaxNumber;
            //updateModel.Member.PostCode = model.PostCode;
            updateModel.Member.MemberId = model.MemberId;
            updateModel.LoginId = loginId;
            UploadImage(model, profilePic);

            if (model.MemberId > 0)
            {

                if (!string.IsNullOrEmpty(model.ProfilePic))
                {
                    updateModel.Avatar = model.ProfilePic;
                    _userService.Update(updateModel);
                }
            }
        }

        public Login GetPatientDoctorLogin(BaseMember member, string userName, RegisterViewModel model)
        {
            Login login = new Login
            {
                LoginName = userName,
                LoginPassword = "patient",
                //Add role accordingly
                LookUpRoleId = (Int32)model.RoleId,
                Email = model.Email,
                Newsletter = model.Newsletter,
            };
            member.LoginId = login.LoginId;
            login.Member = member;
            login.Member.SetCreated(login.LoginId, string.Empty, false);
            login.SetCreated(login.LoginId, string.Empty, false);
            return login;
        }
    }
}