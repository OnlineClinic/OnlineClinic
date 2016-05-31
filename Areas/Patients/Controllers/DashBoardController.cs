using MyOnlineClinic.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyOnlineClinic.RepositoryService;
using MyOnlineClinic.Entity;
using MyOnlineClinic.Web.Helper;
using MyOnlineClinic.Web.Models;
using System.Collections.ObjectModel;
using System.IO;

namespace MyOnlineClinic.Web.Areas.Patients.Controllers
{
    //[AuthorizeRole("Patient")]
    public class DashBoardController : BaseController
    {
        ILoginHelper _loginService;
        IUserService _userService;

        protected IFormsAuthentication _formsAuth;
        private readonly IOrganizationService _organizationService;
        private readonly ICountryService _countryService;
        private readonly ICityService _cityService;
        private readonly ITimeZoneService _timeZoneService;
        private readonly ITitleService _TitleServices;
        private readonly IGenderService _genderService;
        private readonly IStateService _stateService;
        private readonly IPatientHistoryService _patientHistoryService;
        public DashBoardController(ILoginHelper loginService, IUserService userService,
            IOrganizationService organizationService, ICountryService countryService, ICityService cityService,
            ITimeZoneService timeZoneService, ITitleService TitleServices, IGenderService genderService, IStateService stateService
            , IFormsAuthentication formsAuth, IPatientHistoryService patientHistoryService)
        {
            _loginService = loginService;
            _userService = userService;
            _organizationService = organizationService;
            _countryService = countryService;
            _cityService = cityService;
            _timeZoneService = timeZoneService;
            _TitleServices = TitleServices;
            _genderService = genderService;
            _stateService = stateService;
            _formsAuth = formsAuth;
            _patientHistoryService = patientHistoryService;
        }

        //
        // GET: /Patients/DashBoard/
        public ActionResult Index()
        {
            //Login model = _userService.Find(base.loginUserModel.LoginId);

            ////RegisterViewModel userModel = new RegisterViewModel();
            ////return View(userModel);

            //var cmodel = _userService.GetPatientById(Convert.ToInt32(model.Member.MemberId)).Select(x => new RegisterViewModel
            //{
            //    FirstName = System.Globalization.CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(x.FirstName),
            //    SurName = System.Globalization.CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(x.SurName),
            //    State = x.State,
            //    CountryId = x.CountryId,
            //    City = x.CountryId,
            //    Address1 = System.Globalization.CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(x.Address1),
            //    Address2 = System.Globalization.CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(x.Address2),
            //    PhoneNumber = x.PhoneNumber,
            //    MobileNumber = x.MobileNumber,
            //    FaxNumber = x.FaxNumber,
            //    Email = x.Email,
            //    DOB = x.DOB,
            //    PostCode = x.PostCode,
            //    MedicareNumber = x.MedicareNumber,
            //    MedicareRefNumber = x.MedicareRefNo,
            //    // MedicareExpire=x.Expiry,
            //    DavCardColor = x.DavColorCard,
            //    DavNumber = x.DVANumber,
            //    PrivateFund = x.PrivateFund,
            //    PrivateFundMembershipNo = x.PrivateFundMembershipNo,
            //    HCCPensionNo = x.HCCPensionNo,
            //    HCCPensionNoExpiry = x.HCCPensionNoExpiry,
            //    UsualGPName = x.UsualGPName,
            //    UsualGPAddress = x.UsualGPAddress,
            //    DavDisablities = x.DavDisablities,
            //    UsualGPClinicName = x.UsualGPClinicName,
            //    GPAddress2 = System.Globalization.CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(x.GPAddress2),
            //    UsualGpCity = x.UsualGpCity,
            //    UsualGpCountry = x.UsualGpCountry,
            //    UsualGpState = x.UsualGpState,
            //    UsualGpPostCode = x.UsualGpPostCode,
            //    //organizationName = _organizationService.GetOrganizationById(x.OrgId).OrganizationName,
            //    //CountryName = _countryService.GetCountryById(x.CountryId).CountryName,
            //    //StateName = _stateService.GetStateById(x.State).StateName,
            //    //CityName = _cityService.GetCityById(x.City).LookUpCityName,
            //    LoginId = x.LoginId,
            //    MemberId = x.MemberId
            //}).ToList().FirstOrDefault();

            //cmodel.ProfilePic = _userService.Find(cmodel.LoginId).Avatar;

            //return View(GetInfo());
            return View();
        }

        //GET ://patietn Registraion by Admin
        [AllowAnonymous]
        public ActionResult UpdatePatientProfile()
        {
            RegisterViewModel model = new RegisterViewModel();
            //var id = base.loginUserModel.MemberId;
            var id = 1;
            if (id > 0)
            {
                var Cmodel = _userService.GetPatientById(Convert.ToInt32(id)).Select(x => new RegisterViewModel
                {
                    FirstName = StringCipher.Decrypt(x.FirstName),
                    SurName = StringCipher.Decrypt(x.SurName),
                    State = x.State,
                    CountryId = x.CountryId,
                    City = x.City,
                    Address1 = StringCipher.Decrypt(x.Address1),
                    Address2 = StringCipher.Decrypt(x.Address2),
                    PhoneNumber = StringCipher.Decrypt(x.PhoneNumber),
                    MobileNumber = StringCipher.Decrypt(x.MobileNumber),
                    FaxNumber = StringCipher.Decrypt(x.FaxNumber),
                    Email = StringCipher.Decrypt(x.Email),
                    DOB = Convert.ToDateTime(x.DOB.ToString()),
                    PostCode = StringCipher.Decrypt(x.PostCode),
                    MemberId = x.MemberId,
                    MedicareNumber = StringCipher.Decrypt(x.MedicareNumber),
                    MedicareRefNumber = StringCipher.Decrypt(x.MedicareRefNo),
                    //MedicareExpire = Convert.ToDateTime(x.Expiry.ToString()),
                    DavCardColor = x.DavColorCard,
                    DavNumber = StringCipher.Decrypt(x.DVANumber),
                    PrivateFund = StringCipher.Decrypt(x.PrivateFund),
                    PrivateFundMembershipNo = StringCipher.Decrypt(x.PrivateFundMembershipNo),
                    HCCPensionNo = StringCipher.Decrypt(x.HCCPensionNo),
                    HCCPensionNoExpiry = StringCipher.Decrypt(x.HCCPensionNoExpiry),
                    UsualGPName = StringCipher.Decrypt(x.UsualGPName),
                    UsualGPAddress = StringCipher.Decrypt(x.UsualGPAddress),
                    DavDisablities = StringCipher.Decrypt(x.DavDisablities),
                    UsualGPClinicName = StringCipher.Decrypt(x.UsualGPClinicName),
                    GPAddress2 = StringCipher.Decrypt(x.GPAddress2),
                    UsualGpCity = x.UsualGpCity,
                    UsualGpCountry = x.UsualGpCountry,
                    UsualGpState = x.UsualGpState,
                    UsualGpPostCode = StringCipher.Decrypt(x.UsualGpPostCode),
                    //organizationName = _organizationService.GetOrganizationById(x.OrgId).OrganizationName,
                    //CountryName = _countryService.GetCountryById(x.CountryId).CountryName,
                    //StateName = _stateService.GetStateById(x.State).StateName,
                    //CityName = _cityService.GetCityById(x.City).LookUpCityName,                    
                    TitleName = StringCipher.Decrypt(_TitleServices.GetTitleById(x.Title).TitleName),
                    GenderName = StringCipher.Decrypt(_genderService.GetGenderById(x.Gender).GenderName),
                    GenderId = x.Gender,
                    TitleId = x.Title,
                    OrgId = x.OrgId,
                    LoginId = x.LoginId,
                }).FirstOrDefault();
                if (model.State != null)
                {
                    model.StateName = StringCipher.Decrypt(_stateService.GetStateById(model.State).StateName);
                }
                if (model.MiddleName != null)
                {
                    model.MiddleName = StringCipher.Decrypt(model.MiddleName);
                }
                if (model.CountryId != null)
                {
                    model.CountryName = StringCipher.Decrypt(_countryService.GetCountryById(model.CountryId).CountryName);
                }
                if (model.City != null)
                {
                    model.CityName = StringCipher.Decrypt(_cityService.GetCityById(model.City).LookUpCityName);
                }
                if (model.ProfilePic != null)
                {
                    model.ProfilePic = _userService.Find(model.LoginId).Avatar;
                }
                BindDropDown(Cmodel);
                return View(Cmodel);
            }
            else
            {
                BindDropDown(model);
                return View(model);
            }
        }
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult UpdatePatientProfile(RegisterViewModel model, HttpPostedFileBase ProfilePic)
        {
            Patient updateprofile = null;
            updateprofile = model.AdminEditMember(model);
            if (model.MemberId > 0)
            {
                updateprofile.ModifiedBy = base.loginUserModel.LoginId;
                updateprofile.ModifiedDate = DateTime.Now;
                _loginService.UploadImage(model.LoginId, ProfilePic);
                _userService.UpdatePatient(updateprofile);
                if (ProfilePic != null)
                {
                    _loginService.UploadImage(model, ProfilePic);
                }
                if (!string.IsNullOrEmpty(model.ProfilePic))
                {
                    var userid = _userService.Find(model.LoginId);
                    userid.Avatar = model.ProfilePic;
                    _userService.Update(userid);
                }

                BindDropDown(model);
                return RedirectToAction("Index");
            }

            //var updateModel = _userService.Find(model.LoginId);
            ////var userid = _userService.GetPatientById(model.MemberId);
            //Patient updateprofile = null;
            //updateprofile = model.AdminEditMember(model);
            ////var user = model.GetLoginAdmin(profile, model.Email);
            //if (model.MemberId > 0)
            //{
            //    //UploadImage(model, ProfilePic);
            //    //_userService.UpdateMember(updateModel);
            //    _userService.UpdatePatient(updateprofile);                
            //    BindDropDown(model);
            //    return RedirectToAction("Index");
            //}
            return View(model);
        }

        public RegisterViewModel BindDropDown(RegisterViewModel model)
        {
            var title = _TitleServices.GetTitleList();
            for (int i = 0; i < title.Count; i++)
            {
                title[i].TitleName = StringCipher.Decrypt(title[i].TitleName);
            }
    
            model.TitleList = title.Where(x => x.Type == (int)RoleType.ADMIN).ToList();
            var countrylist = _countryService.GetCountryList();
            for (int i = 0; i < countrylist.Count; i++)
            {
                countrylist[i].CountryName = StringCipher.Decrypt(countrylist[i].CountryName);
            }      
            var Citylist = _cityService.GetCityList();
            for (int i = 0; i < Citylist.Count; i++)
            {
                Citylist[i].LookUpCityName = StringCipher.Decrypt(Citylist[i].LookUpCityName);
            }         
            var Statelist = _stateService.GetStateList();
            for (int i = 0; i < Statelist.Count; i++)
            {
                Statelist[i].StateName = StringCipher.Decrypt(Statelist[i].StateName);
            }
            model.OrganizationList = _organizationService.GetOrganizationList();
            for (int i = 0; i < model.OrganizationList.Count; i++)
            {
                model.OrganizationList[i].OrganizationName = StringCipher.Decrypt(model.OrganizationList[i].OrganizationName);
            }
            model.GenderList = _genderService.GetGenderList();
            model.CountryList = countrylist;
            model.CityList = model.MemberId > 0 ? Citylist : new List<LookUpCity>();
           
            model.TitleList = title;
            model.StateList = model.MemberId > 0 ? Statelist : new List<LookUpState>();
            ReadOnlyCollection<TimeZoneInfo> tz;
            tz = TimeZoneInfo.GetSystemTimeZones();
            var timeZoneList = tz.Select(p => new SelectListItem { Value = p.DisplayName, Text = p.DisplayName }).ToList();
            ViewBag.Timezone = timeZoneList;
            var clinic = entitiesDb.Clinics.Select(p => new SelectListItem { Value = p.ClinicName, Text = StringCipher.Decrypt(p.ClinicName) }).ToList();
            ViewBag.Clinic = clinic;
            return model;
        }
        //
        // POST: /Account/LogOff
        //[HttpPost]
        [AllowAnonymous]
        public ActionResult LogOff()
        {
            _formsAuth.SignOut();
            //return RedirectToAction("Index", "Home", new { @area = "Patients" });
            return RedirectToAction("Index", "");
        }

        //organization details
        public ActionResult Details(int? id)
        {
            var model = _userService.GetPatientById(Convert.ToInt32(id)).Select(x => new RegisterViewModel
            {
                FirstName = StringCipher.Decrypt(x.FirstName),
                SurName = StringCipher.Decrypt(x.SurName),
                State = x.State,
                CountryId = x.CountryId,
                City = x.CountryId,
                Address1 = StringCipher.Decrypt(x.Address1),
                Address2 = StringCipher.Decrypt(x.Address2),
                PhoneNumber = StringCipher.Decrypt(x.PhoneNumber),
                MobileNumber = StringCipher.Decrypt(x.MobileNumber),
                FaxNumber = StringCipher.Decrypt(x.FaxNumber),
                Email = StringCipher.Decrypt(x.Email),
                DOB = x.DOB,
                PostCode = StringCipher.Decrypt(x.PostCode),

                MedicareNumber = StringCipher.Decrypt(x.MedicareNumber),
                MedicareRefNumber = StringCipher.Decrypt(x.MedicareRefNo),
                // MedicareExpire=x.Expiry,
                DavCardColor = x.DavColorCard,
                DavNumber = StringCipher.Decrypt(x.DVANumber),
                PrivateFund = StringCipher.Decrypt(x.PrivateFund),
                PrivateFundMembershipNo = StringCipher.Decrypt(x.PrivateFundMembershipNo),
                HCCPensionNo = StringCipher.Decrypt(x.HCCPensionNo),
                HCCPensionNoExpiry = x.HCCPensionNoExpiry,
                UsualGPName = x.UsualGPName,
                UsualGPAddress = x.UsualGPAddress,
                DavDisablities = x.DavDisablities,
                UsualGPClinicName = x.UsualGPClinicName,
                GPAddress2 = x.GPAddress2,
                UsualGpCity = x.UsualGpCity,
                UsualGpCountry = x.UsualGpCountry,
                UsualGpState = x.UsualGpState,
                UsualGpPostCode = x.UsualGpPostCode,
                organizationName = _organizationService.GetOrganizationById(x.OrgId).OrganizationName,
                CountryName = _countryService.GetCountryById(x.CountryId).CountryName,
                StateName = _stateService.GetStateById(x.State).StateName,
                CityName = _cityService.GetCityById(x.City).LookUpCityName,
                // UserId = x.MemberId,
                //  OrgId = x.OrgId,
                LoginId = x.LoginId
                //Gender = _countryService.GetCountryById(Convert.ToInt32(x.CountryId)).CountryName
            }).ToList().FirstOrDefault();

            model.ProfilePic = _userService.Find(model.LoginId).Avatar;


            return View(model);

        }


        public RegisterViewModel UploadImage(RegisterViewModel model, HttpPostedFileBase profilePic)
        {
            if (profilePic != null && profilePic.ContentLength > 0)
            {
                var fileName = Path.GetFileName(profilePic.FileName);
                string extension = Path.GetExtension(fileName);
                if (extension != null && (extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".gif"))
                {
                    string fName = Convert.ToString(DateTime.Now.ToString("ddMMyyyy-HHmmss-fff")) + extension;
                    var path = Path.Combine(Server.MapPath("~/UploadedFiles/UserProfile/"), fName);
                    profilePic.SaveAs(path);
                    model.ProfilePic = "/UploadedFiles/UserProfile/" + fName;
                }
                else
                {
                    ModelState.AddModelError("Error", "Not a valid file");
                    ViewBag.CurrentMenu = "Publicidade";
                    ViewBag.isError = 1;
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
        public ActionResult History(int? id)
        {



            return View(GetInfo());
        }
        public RegisterViewModel GetInfo()
        {
            Login model = _userService.Find(base.loginUserModel.LoginId);
            //RegisterViewModel userModel = new RegisterViewModel();
            //return View(userModel);
            var cmodel = _userService.GetPatientById(Convert.ToInt32(model.Member.MemberId)).Select(x => new RegisterViewModel
            {
                FirstName = StringCipher.Decrypt(x.FirstName) ?? "",
                SurName = StringCipher.Decrypt(x.SurName) ?? "",
                State = x.State,
                CountryId = x.CountryId,
                City = x.CountryId,
                Address1 = StringCipher.Decrypt(x.Address1) ?? "",
                Address2 = StringCipher.Decrypt(x.Address2) ?? "",
                PhoneNumber = StringCipher.Decrypt(x.PhoneNumber),
                MobileNumber = StringCipher.Decrypt(x.MobileNumber),
                FaxNumber = StringCipher.Decrypt(x.FaxNumber),
                Email = StringCipher.Decrypt(x.Email),
                DOB = x.DOB,
                PostCode = StringCipher.Decrypt(x.PostCode),
                MedicareNumber = StringCipher.Decrypt(x.MedicareNumber),
                MedicareRefNumber = StringCipher.Decrypt(x.MedicareRefNo),
                // MedicareExpire=x.Expiry,
                DavCardColor = x.DavColorCard,
                DavNumber = StringCipher.Decrypt(x.DVANumber),
                PrivateFund = StringCipher.Decrypt(x.PrivateFund),
                PrivateFundMembershipNo = StringCipher.Decrypt(x.PrivateFundMembershipNo),
                HCCPensionNo = StringCipher.Decrypt(x.HCCPensionNo),
                HCCPensionNoExpiry = StringCipher.Decrypt(x.HCCPensionNoExpiry),
                UsualGPName = StringCipher.Decrypt(x.UsualGPName),
                UsualGPAddress = StringCipher.Decrypt(x.UsualGPAddress),
                DavDisablities = StringCipher.Decrypt(x.DavDisablities),
                UsualGPClinicName = StringCipher.Decrypt(x.UsualGPClinicName),
                GPAddress2 = x.GPAddress2 == null ? "" :  StringCipher.Decrypt(x.GPAddress2),
                UsualGpCity = x.UsualGpCity,
                UsualGpCountry = x.UsualGpCountry,
                UsualGpState = x.UsualGpState,
                UsualGpPostCode = StringCipher.Decrypt(x.UsualGpPostCode),
                //organizationName = _organizationService.GetOrganizationById(x.OrgId).OrganizationName,
                //CountryName = _countryService.GetCountryById(x.CountryId).CountryName,
                //StateName = _stateService.GetStateById(x.State).StateName,
                //CityName = _cityService.GetCityById(x.City).LookUpCityName,
                LoginId = x.LoginId,
                MemberId = x.MemberId
            }).ToList().FirstOrDefault();

            cmodel.ProfilePic = _userService.Find(cmodel.LoginId).Avatar ?? "";
            return cmodel;
        }
    }
}