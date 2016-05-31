using MyOnlineClinic.Emailer;
using MyOnlineClinic.Entity;
using MyOnlineClinic.RepositoryService;
using MyOnlineClinic.Web.Controllers;
using MyOnlineClinic.Web.Helper;
using MyOnlineClinic.Web.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyOnlineClinic.Web.Areas.Organizations.Controllers
{
    public class DoctorController : BaseController
    {
        protected IFormsAuthentication _formsAuth;
        IDoctorService _doctorservice;
        IClinicService _clinicservice;
        ICountryService _countryService;
        ICityService _cityService;
        IOrganizationService _organizationservice;
        IUserService _userService;
        ITimeZoneService _timezoneservice;
        ILoginHelper _loginHelper;
        IProfessionTypeService _professiontypeservice;
        IStateService _stateservice;
        private readonly ITitleService _TitleServices;
        private readonly IGenderService _genderService;
        IUregisteredOrganizationService _unregisteredOrganizationService;
        IUnregisteredClinicService _unregisteredClinicService;
        IDoctorMembershipService _doctorMembershipService;
        IMembeshipVoucherService _membeshipVoucherService;
        IVoucherService _voucherService;
        IAppointmentService _AppointmentService;

        IPermissionInModuleService _permissioninmoduleService;
        IPatientService _patientServices;
        public DoctorController(ICountryService countryService, ICityService cityService, IFormsAuthentication formsAuth,
            IUserService userService, IDoctorService doctorservice, IOrganizationService organizationservice,
            ITimeZoneService timezoneservice, ILoginHelper loginHelper, IProfessionTypeService professiontypeservice,
            IClinicService clinicservice, ITitleService TitleServices, IGenderService genderService, IStateService stateservice,
            IUregisteredOrganizationService unregisteredOrganizationService, IUnregisteredClinicService unregisteredClinicService,
           IDoctorMembershipService doctorMembershipService, IMembeshipVoucherService membeshipVoucherService, IVoucherService voucherService, IPermissionInModuleService PermissionInModuleService, IPatientService patientServices, IAppointmentService AppointmentService)
        {
            _countryService = countryService;
            _cityService = cityService;
            _formsAuth = formsAuth;
            _userService = userService;
            _doctorservice = doctorservice;
            _organizationservice = organizationservice;
            _timezoneservice = timezoneservice;
            _loginHelper = loginHelper;
            _professiontypeservice = professiontypeservice;
            _clinicservice = clinicservice;
            _TitleServices = TitleServices;
            _genderService = genderService;
            _stateservice = stateservice;
            _unregisteredOrganizationService = unregisteredOrganizationService;
            _unregisteredClinicService = unregisteredClinicService;
            _doctorMembershipService = doctorMembershipService;
            _membeshipVoucherService = membeshipVoucherService;
            _voucherService = voucherService;
            _permissioninmoduleService = PermissionInModuleService;
            _patientServices = patientServices;
            _AppointmentService = AppointmentService;
        }
        //
        // GET: /Doctor/Doctor/
        public ActionResult Add(int? id)
        {
            // RegisterViewModel doctor = new RegisterViewModel();

            if (id.HasValue)
            {
                var cModel = _doctorservice.GetDoctorById(Convert.ToInt32(id)).Select(x => new RegisterViewModel
                {
                    TitleId = x.Title,
                    FirstName = StringCipher.Decrypt(x.FirstName),
                    MiddleName = StringCipher.Decrypt(x.MiddleName),
                    SurName = StringCipher.Decrypt(x.SurName),
                    Email = StringCipher.Decrypt(x.Email),
                    DOB = x.DOB,
                    GenderId = x.Gender,
                    MobileNumber = StringCipher.Decrypt(x.MobileNumber),
                    FaxNumber = StringCipher.Decrypt(x.FaxNumber),
                    ClinicID = Convert.ToInt32(x.ClinicID),
                    City = x.City,
                    State = x.State,
                    CountryId = x.CountryId,
                    Profession = x.Profession,
                    PostCode = StringCipher.Decrypt(x.PostCode),
                    Timezone = StringCipher.Decrypt(x.TimeZoneString),
                    //ClinicID = _clinicService.GetClinicById(id).cli,
                    PhoneNumber = StringCipher.Decrypt(x.PhoneNumber),
                    Address1 = StringCipher.Decrypt(x.Address1),
                    Address2 = StringCipher.Decrypt(x.Address2),
                    OrgId = x.OrgId,
                    //  ProfilePic = _userService.Find(x.LoginId).Avatar,

                    IsActive = x.IsActive,
                    MemberId = x.MemberId,
                    LoginId = x.LoginId,
                    Createdby = (Guid)x.CreatedBy,
                    PhoneCode = x.PhoneCode,
                    LanguageSpoken = StringCipher.Decrypt(x.LanguageSpoken),
                    ProfessionCategory = x.ProfessionCategory,
                    IntrestArea = StringCipher.Decrypt(x.IntrestArea),
                    qualification = StringCipher.Decrypt(x.qualification),
                    AHPRANo = StringCipher.Decrypt(x.AHPRANo),
                    VideoProviderNo = StringCipher.Decrypt(x.VideoProviderNo),
                    HomeVisitProviderNo = StringCipher.Decrypt(x.HomeVisitProviderNo),
                    ClinicProviderNo = StringCipher.Decrypt(x.ClinicProviderNo),
                    PrescriberNo = StringCipher.Decrypt(x.PrescriberNo),
                    DoctorProfile = StringCipher.Decrypt(x.DoctorProfile),
                    TimeZoneId = Common.GetTimeZoneStandardIdAndDisplayName(x.TimeZoneString)
                    // SignaturePic = _doctorservice.GetDoctorById(x.MemberId)

                    //  OrganizationAddress=_organizationservice.GetOrganizationAddressbyId(Convert.ToInt32(x.OrgId)),
                }).FirstOrDefault();
                try
                {

                    cModel.OrgClinicIdsList = _userService.GetClinicList(Convert.ToInt32(id), (int)RoleType.Doctor).Select(x =>
                       new RegisterViewModel
                       {
                           UserClinicId = x.ClinicId,
                           UserClinicName = StringCipher.Decrypt(x.Clinics.ClinicName),
                           UserClinicAddress1 = StringCipher.Decrypt(x.Clinics.Address1),
                           UserClinicAddress2 = StringCipher.Decrypt(x.Clinics.Address2),
                           UserClinicPostalCode = StringCipher.Decrypt(x.Clinics.PostCode),
                           UserClinicCountryId = x.Clinics.Country,
                           UserClinicStateId = x.Clinics.State,
                           UserClinicCityId = x.Clinics.City,
                           ClinicProviderNo = StringCipher.Decrypt(x.ClinicProviderNo)


                       }).ToList();
                }
                catch { }
                if (cModel != null)
                {

                    cModel.ProfilePic = _userService.Find(cModel.LoginId).Avatar;
                    if (_organizationservice.GetOrganizationById(cModel.OrgId) != null)
                    {
                        cModel.organizationName = StringCipher.Decrypt(_organizationservice.GetOrganizationById(cModel.OrgId).OrganizationName);
                    }
                    if (_doctorservice.GetDoctorList().Where(y => y.MemberId == cModel.MemberId).Select(y => y.SignaturePic).FirstOrDefault() != null)
                    {
                        cModel.SignaturePic = _doctorservice.GetDoctorList().Where(y => y.MemberId == cModel.MemberId).Select(y => y.SignaturePic).FirstOrDefault();
                    }
                }
                BindDropDown(cModel);
                return View(cModel);
            }
            else
            {
                RegisterViewModel doctor = new RegisterViewModel();
                BindDropDown(doctor);
                var organization = _organizationservice.GetOrganizationByLoginId(base.loginUserModel.LoginId);

                if (organization != null)
                {
                    doctor.OrganizationId = organization.OrganizationId;
                    doctor.UserorgId = organization.OrganizationId;
                    doctor.OrgId = organization.OrganizationId;
                }
                return View(doctor);
            }
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Add(RegisterViewModel model, HttpPostedFileBase ProfilePic, HttpPostedFileBase Signaturepic)
        {           
                if (model.MemberId > 0)
            {
                if (Signaturepic != null && Signaturepic.ContentLength > 0)
                {
                    UploadSignature(model, Signaturepic);
                }
                else
                {
                    model.SignaturePic = model.SignaturePic;
                }
                if (ProfilePic != null)
                {
                    _loginHelper.UploadImage(model, ProfilePic);
                    var userid = _userService.Find(model.LoginId);
                    userid.Avatar = model.ProfilePic;
                    _userService.Update(userid);
                }
                MyOnlineClinic.Entity.Doctor profile = null;

                profile = model.AdminEditDoctor(model);
                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(model.TimeZoneId);
                TimeSpan offset = tzi.GetUtcOffset(Convert.ToDateTime(DateTime.UtcNow));
                profile.TimeZoneString = Common.GetTimeZoneOffset(offset);
                _doctorservice.Update(profile);
                var length = entitiesDb.UserClinics.Where(w => w.UserId == model.MemberId && w.Type == (int)RoleType.Doctor).ToList();
                if (length != null)
                {
                    for (int i = 0; i < length.Count; i++)
                    {
                        UserClinic studentToDelete = entitiesDb.UserClinics.Where(s => s.UserId == model.MemberId).FirstOrDefault<UserClinic>();
                        entitiesDb.Entry(studentToDelete).State = System.Data.Entity.EntityState.Deleted;
                        entitiesDb.SaveChanges();
                    }
                }
                if (model.OrgClinicIds != null)
                {
                    string[] id = model.OrgClinicIds.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < id.Length; i++)
                    {
                        string[] provider = id[i].ToString().Split('-');
                        var UserClinic = new UserClinic
                        {
                            UserId = model.MemberId,
                            OrganizationId = model.UserorgId,
                            Type = (int)RoleType.Doctor,
                            ClinicId = Convert.ToInt32(provider[0]),
                            ClinicProviderNo = StringCipher.Encrypt(provider[1].ToString())
                        };
                        _unregisteredOrganizationService.AddClinic(UserClinic);
                    }
                }

                DoctorConsultFee feeDelete = entitiesDb.DoctorConsult.Where(s => s.DoctorId == model.MemberId && s.AppointmentType == (int)AppointmentType.VideoConsult).FirstOrDefault<DoctorConsultFee>();
                if (feeDelete != null)
                {
                    entitiesDb.Entry(feeDelete).State = System.Data.Entity.EntityState.Deleted;
                    entitiesDb.SaveChanges();
                }
                if (model.TelemedicineFeeList != null)
                {
                    for (int i = 0; i < model.TelemedicineFeeList.Count; i++)
                    {
                        var consult = new DoctorConsultFee();
                        consult.DoctorId = model.MemberId;
                        consult.AppointmentType = (int)AppointmentType.VideoConsult;
                        consult.StatdardFee = model.TelemedicineFeeList[i].StatdardFee;
                        consult.LongFee = model.TelemedicineFeeList[i].LongFee;
                        consult.VeryLongFee = model.TelemedicineFeeList[i].VeryLongFee;
                        consult.CurrencyType = model.TelemedicineFeeList[i].CurrencyType;
                        consult.RepeatPrescription = model.ClinicVisitFeeList[i].RepeatPrescription;
                        _doctorservice.AddConsultfee(consult);
                    }
                }
                DoctorConsultFee feetoDelete = entitiesDb.DoctorConsult.Where(s => s.DoctorId == model.MemberId && s.AppointmentType == (int)AppointmentType.ClinicVisit).FirstOrDefault<DoctorConsultFee>();
                if (feetoDelete != null)
                {
                    entitiesDb.Entry(feetoDelete).State = System.Data.Entity.EntityState.Deleted;
                    entitiesDb.SaveChanges();
                }
                if (model.ClinicVisitFeeList != null)
                {
                    for (int i = 0; i < model.ClinicVisitFeeList.Count; i++)
                    {
                        var consult = new DoctorConsultFee();
                        consult.DoctorId = model.MemberId;
                        consult.AppointmentType = (int)AppointmentType.ClinicVisit;
                        consult.StatdardFee = model.ClinicVisitFeeList[i].StatdardFee;
                        consult.LongFee = model.ClinicVisitFeeList[i].LongFee;
                        consult.VeryLongFee = model.ClinicVisitFeeList[i].VeryLongFee;
                        consult.RepeatPrescription = model.ClinicVisitFeeList[i].RepeatPrescription;
                        consult.CurrencyType = model.TelemedicineFeeList[i].CurrencyType;
                        _doctorservice.AddConsultfee(consult);
                    }
                }
                var objconsultfee = _doctorservice.GetDoctorConsultFee(model.MemberId).Where(X => X.AppointmentType == (int)AppointmentType.RepeatPrescription).FirstOrDefault();       
                objconsultfee.RepeatPrescription = model.RepeatPrescription;
                objconsultfee.CurrencyType = model.TelemedicineFeeList[0].CurrencyType;
                objconsultfee.AppointmentType = (int)AppointmentType.RepeatPrescription;
                objconsultfee.DoctorId = model.MemberId;
               _doctorservice.UpdateConsultfee(objconsultfee);
                var Account = _doctorservice.GetDoctorAccountDetails(model.LoginId).FirstOrDefault();
                if (Account != null)
                {
                    Account.AccountName = model.DoctorAccountdetail.AccountName;
                    Account.BankName = model.DoctorAccountdetail.BankName;
                    Account.Bsb = model.DoctorAccountdetail.Bsb;
                    Account.AccountNumber = model.DoctorAccountdetail.AccountNumber;
                    Account.DoctorLoginId = model.LoginId;
                    Account.PaypelEmailId = model.DoctorAccountdetail.PaypelEmailId;
                    _doctorservice.Update(Account);
                }
            }
            else
            {
                int Unorgid = 0;
                var userExists = _userService.CheckDuplicateUserName(model.UserName);
                if (userExists)
                {
                    ModelState.AddModelError("UserName", "Username already exists.");
                }
                var userEmailExists = _userService.CheckDuplicateMail(model.Email);
                if (userEmailExists)
                {
                    ModelState.AddModelError("email", "email already exists.");

                    BindDropDown(model);
                    return View(model);
                }
                else
                {
                    string strgender = string.Empty;
                    if (model.GenderId != null)
                    {
                        strgender = _genderService.GetGenderById(model.GenderId).GenderName;
                    }
                    if (ProfilePic != null)
                    {
                        _loginHelper.UploadImage(model, ProfilePic);
                    }
                    else if (strgender == "Male")
                    {
                        model.ProfilePic = "/UploadedFiles/UserProfile/Male.png";

                    }
                    else if (strgender == "Female")
                    {
                        model.ProfilePic = "/UploadedFiles/UserProfile/Female.png";
                    }
                    if (Signaturepic != null && Signaturepic.ContentLength > 0)
                    {
                        UploadSignature(model, Signaturepic);
                    }
                    BaseMember profile = null;
                    model.RoleId = RoleType.Doctor;
                    if (model.UserorgId == 0)
                    {
                        model.OrgId = 1;
                    }
                    else
                    {
                        model.OrgId = model.UserorgId;
                    }
                    profile = model.GetDoctorAdmin();
                    var user = model.GetLogin(profile, model.Email);

                    var Password = RandomPassword.Generate(6, 8);
                    user.LoginPassword = Password;
                    user.IsEmailVerified = true;
                    user.IsActive = true;
                    user.IsApproved = true;
                    _userService.Add(user);
                    Guid DoctorLoginId = user.LoginId;
                    if (model.UserorgId != 0)
                    {
                        var UserOrganization = new UserOrganization
                        {
                            UserId = user.Member.MemberId,
                            OrgId = model.UserorgId,
                            Type = (int)RoleType.Doctor

                        };
                        _unregisteredOrganizationService.AddOrg(UserOrganization);
                    }
                    //Add If Organization not exsit User Unregister Organization By dheeraj
                    //else
                    //{
                    //    var unregisteredOrganizations = new UnregisteredOrganizations
                    //    {
                    //        Name = StringCipher.Encrypt(model.organizationName),
                    //        AddressLine1 = StringCipher.Encrypt(model.OrganizationAddress1),
                    //        AddressLine2 = StringCipher.Encrypt(model.OrganizationAddress2),
                    //        Country = model.OrganizationCountryId,
                    //        State = model.OrganizationStateId,
                    //        City = model.OrganizationCityId,
                    //        PostCode = StringCipher.Encrypt(model.OrganizationPostalCode),
                    //        UserId = user.Member.MemberId,
                    //        Type = (int)RoleType.Doctor,
                    //        IsActive = false,
                    //        TimeZoneString = model.TimeZoneId
                    //    };
                    //    _unregisteredOrganizationService.Add(unregisteredOrganizations);
                    //    Unorgid = unregisteredOrganizations.Id;
                    //}
                    //Add User Clinic By dheeraj
                    if (model.OrgClinicIds != null)
                    {
                        string[] id = model.OrgClinicIds.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                        for (int i = 0; i < id.Length; i++)
                        {
                            string[] provider = id[i].ToString().Split('-');
                            var UserClinic = new UserClinic
                            {
                                UserId = user.Member.MemberId,
                                OrganizationId = model.UserorgId,
                                Type = (int)RoleType.Doctor,
                                ClinicId = Convert.ToInt32(provider[0]),
                                ClinicProviderNo = StringCipher.Encrypt(provider[1].ToString())
                            };
                            _unregisteredOrganizationService.AddClinic(UserClinic);
                        }
                    }
                    //Add If Clinic not exsit User Unregister Clinic By dheeraj
                    else
                    {
                        string Orgtype = string.Empty;
                        if (model.UserorgId != 0)
                        {
                            Orgtype = "R";
                        }
                        else
                        {
                            Orgtype = "U";
                            model.UserorgId = Unorgid;
                        }
                        var UnRegisteredClinic = new UnRegisteredClinic
                        {
                            UserId = user.Member.MemberId,
                            Name = StringCipher.Encrypt(model.UserClinic),
                            OrgId = model.UserorgId,
                            AddressLine1 = StringCipher.Encrypt(model.ClinicAddress1),
                            AddressLine2 = StringCipher.Encrypt(model.ClinicAddress2),
                            Country = model.ClinicCountryId,
                            State = model.ClinicStateId,
                            City = model.ClinicCityId,
                            PostCode = StringCipher.Encrypt(model.ClinicPostCodeCode),
                            Type = (int)RoleType.Doctor,
                            IsActive = false,
                            OrganizationType = Orgtype,
                            TimeZoneString = model.TimeZoneId

                        };
                        _unregisteredClinicService.Add(UnRegisteredClinic);
                    }
                    if (model.TelemedicineFeeList != null)
                    {
                        for (int i = 0; i < model.TelemedicineFeeList.Count; i++)
                        {
                            var consult = new DoctorConsultFee();
                            consult.DoctorId = user.Member.MemberId;
                            consult.AppointmentType = (int)AppointmentType.VideoConsult;
                            if (consult.StatdardFee==null)
                            {
                                consult.StatdardFee = 0;
                            }
                            if (consult.LongFee == null)
                            {
                                consult.LongFee = 0;
                            }
                            if (consult.VeryLongFee == null)
                            {
                                consult.VeryLongFee = 0;
                            }
                            consult.StatdardFee = model.TelemedicineFeeList[i].StatdardFee;
                            consult.LongFee = model.TelemedicineFeeList[i].LongFee;
                            consult.VeryLongFee = model.TelemedicineFeeList[i].VeryLongFee;
                            consult.CurrencyType = model.TelemedicineFeeList[i].CurrencyType;
                            consult.RepeatPrescription = model.TelemedicineFeeList[i].RepeatPrescription;
                            _doctorservice.AddConsultfee(consult);
                        }
                    }
                    if (model.ClinicVisitFeeList != null)
                    {
                        for (int i = 0; i < model.ClinicVisitFeeList.Count; i++)
                        {
                            var consult = new DoctorConsultFee();
                            consult.DoctorId = user.Member.MemberId;
                            if (consult.StatdardFee == null)
                            {
                                consult.StatdardFee = 0;
                            }
                            if (consult.LongFee == null)
                            {
                                consult.LongFee = 0;
                            }
                            if (consult.VeryLongFee == null)
                            {
                                consult.VeryLongFee = 0;
                            }
                            consult.AppointmentType = (int)AppointmentType.ClinicVisit;
                            consult.StatdardFee = model.ClinicVisitFeeList[i].StatdardFee;
                            consult.LongFee = model.ClinicVisitFeeList[i].LongFee;
                            consult.VeryLongFee = model.ClinicVisitFeeList[i].VeryLongFee;
                            consult.RepeatPrescription = model.TelemedicineFeeList[i].RepeatPrescription;
                            consult.CurrencyType = model.ClinicVisitFeeList[i].CurrencyType;
                            _doctorservice.AddConsultfee(consult);
                        }
                    }
                    var objconsultfee = new DoctorConsultFee();
                    if (model.TelemedicineFeeList[0].RepeatPrescription ==null)
                    {
                        model.TelemedicineFeeList[0].RepeatPrescription = 0;
                        
                    }
                    if (model.Prescriptionmodel != null)
                    {
                        if (model.Prescriptionmodel.RepeatPrescription == null)
                        {
                            model.Prescriptionmodel.RepeatPrescription = 0;
                        }
                        objconsultfee.RepeatPrescription = model.Prescriptionmodel.RepeatPrescription;
                        objconsultfee.CurrencyType = model.TelemedicineFeeList[0].CurrencyType;
                        objconsultfee.AppointmentType = (int)AppointmentType.RepeatPrescription;
                        objconsultfee.DoctorId = user.Member.MemberId;
                        _doctorservice.AddConsultfee(objconsultfee);
                    }
                    DoctorAccountDetails Acc = new DoctorAccountDetails();
                    Acc.AccountName = model.DoctorAccountdetail.AccountName;
                    Acc.BankName = model.DoctorAccountdetail.BankName;
                    Acc.Bsb = model.DoctorAccountdetail.Bsb;
                    Acc.AccountNumber = model.DoctorAccountdetail.AccountNumber;
                    Acc.DoctorLoginId = DoctorLoginId;
                    Acc.PaypelEmailId = model.DoctorAccountdetail.PaypelEmailId;
                    _doctorservice.Add(Acc);
                    //string activationToken = user.ActivationToken.ToString();
                    //string link = ConfigurationManager.AppSettings["SitePath"] + "Account/VerifyEmail/" + activationToken;
                    var fileUrl = Server.MapPath("~/EmailTemplates/PatientRegistration.html");
                    string html = System.IO.File.ReadAllText(fileUrl);
                    html = html.Replace("@@Password", Password);
                    //html = html.Replace("@@Password", Password).Replace("@@ActivationLink", link);
                    EmailService objEmail = new EmailService();
                    objEmail.SendEmail("User Registration", html, model.Email, "noreply@myonlineclinic.com.au");

                }
            }
            return RedirectToAction("DoctorList");

        }

        public IEnumerable<SelectListItem> CurrencyCode
        {
            get
            {
                return new[]
            {
                 new SelectListItem { Value = "0", Text = "--Select Country Code--" },
                new SelectListItem { Value = "1", Text = "AUD" },
                new SelectListItem { Value = "2", Text = "EUR" },
                new SelectListItem { Value = "3", Text = "INR" },
                new SelectListItem { Value = "4", Text = "SGD" },
                new SelectListItem { Value = "5", Text = "EUR" },
                new SelectListItem { Value = "6", Text = "GBP" },
               
            };
            }

        }

        public RegisterViewModel BindDropDown(RegisterViewModel model)
        {

            var countrylist = _countryService.GetCountryList().Where(x => x.IsActive == true).ToList();
            for (int i = 0; i < countrylist.Count; i++)
            {
                countrylist[i].CountryName = StringCipher.Decrypt(countrylist[i].CountryName);
            }
            model.CountryList = countrylist;
            var Citylist = _cityService.GetCityList();
            for (int i = 0; i < Citylist.Count; i++)
            {
                Citylist[i].LookUpCityName = StringCipher.Decrypt(Citylist[i].LookUpCityName);
            }
            model.CityList = model.MemberId > 0 ? Citylist : new List<LookUpCity>();

            var Statelist = _stateservice.GetStateList();
            for (int i = 0; i < Statelist.Count; i++)
            {
                Statelist[i].StateName = StringCipher.Decrypt(Statelist[i].StateName);
            }
            model.StateList = model.MemberId > 0 ? Statelist : new List<LookUpState>();
            var genderlist = _genderService.GetGenderList().Where(x => x.Type == (int)RoleType.Doctor && x.IsActive == true).ToList();
            for (int i = 0; i < genderlist.Count; i++)
            {
                genderlist[i].GenderName = StringCipher.Decrypt(genderlist[i].GenderName);
            }
            model.GenderList = genderlist;
            var titlelist = _TitleServices.GetTitleList().Where(x => x.Type == (int)RoleType.Doctor && x.IsActive == true).ToList();
            for (int i = 0; i < titlelist.Count; i++)
            {
                titlelist[i].TitleName = StringCipher.Decrypt(titlelist[i].TitleName);
            }
            model.TitleList = titlelist;
            var subtype = _professiontypeservice.GetProfessionSubTypesList();
            for (int i = 0; i < subtype.Count; i++)
            {
                subtype[i].ProfessionSub = StringCipher.Decrypt(subtype[i].ProfessionSub);
            }
            model.ProfessionSubType = model.MemberId > 0 ? subtype : new List<ProfessionSubType>();
            model.OrganizationCountryList = countrylist;
            model.OrganizationCityList = Citylist;
            model.OrganizationStateList = Statelist;


            model.UserClinicCountryList = countrylist;
            model.UserClinicCityList = model.MemberId > 0 ? Citylist : new List<LookUpCity>();
            model.UserClinicStateList = model.MemberId > 0 ? Statelist : new List<LookUpState>();
            ViewBag.Organization = _organizationservice.GetOrganizationList().ToList();
            model.ClinicList = _clinicservice.GetClinicList();
            for (int i = 0; i < model.ClinicList.Count; i++)
            {
                model.ClinicList[i].ClinicName = StringCipher.Decrypt(model.ClinicList[i].ClinicName);
            }
            ViewBag.CurrencyCode = CurrencyCode;
            ReadOnlyCollection<TimeZoneInfo> tz;
            tz = TimeZoneInfo.GetSystemTimeZones();
            var timeZoneList = tz.Select(p => new SelectListItem { Value = p.Id, Text = p.DisplayName }).ToList();
            ViewBag.Timezone = timeZoneList;

            ViewBag.Clinic = new List<SelectListItem>();
            model.OrganizationList = _organizationservice.GetOrganizationList();
            for (int i = 0; i < model.OrganizationList.Count; i++)
            {
                model.OrganizationList[i].OrganizationName = StringCipher.Decrypt(model.OrganizationList[i].OrganizationName);
            }
            model.ProfessionTypes = _professiontypeservice.GetProfessionTypesList();
            for (int i = 0; i < model.ProfessionTypes.Count; i++)
            {
                model.ProfessionTypes[i].ProfessionName = StringCipher.Decrypt(model.ProfessionTypes[i].ProfessionName);
            }
            model.ClinicCountryList = countrylist;
            model.ClinicCityList = model.MemberId > 0 ? Citylist : new List<LookUpCity>();
            model.ClinicStateList = model.MemberId > 0 ? Statelist : new List<LookUpState>();
            if (model.MemberId > 0)
            {
                model.clinicmodellist = _clinicservice.GetClinicList().Select(y => new DoctorClinicModel { ClinicId = y.ClinicId, ClinicName = y.ClinicName, Checked = false }).ToList();
                model.clinicProviderlist = model.clinicmodellist;
                var fh = _userService.GetClinicList(model.MemberId, Convert.ToInt32(RoleType.Doctor)).ToList();
                for (int i = 0; i < model.clinicmodellist.Count; i++)
                {
                    foreach (var item in fh)
                    {
                        if (Convert.ToInt32(item.ClinicId) == model.clinicmodellist[i].ClinicId)
                        {
                            model.clinicmodellist[i].Checked = true;
                            model.clinicProviderlist[i].ClinicVoucherNo = StringCipher.Decrypt(item.ClinicProviderNo);
                        }
                    }
                }
                List<DoctorConsultFee> listDoctorConsultFee = new List<DoctorConsultFee>();
                listDoctorConsultFee.Add(new DoctorConsultFee { VeryLongFee = null });
                var telemedicine = entitiesDb.DoctorConsult.Where(x => x.DoctorId == model.MemberId && x.AppointmentType == (int)AppointmentType.VideoConsult).ToList();
                if (telemedicine.Count == 0)
                {
                    model.TelemedicineFeeList = listDoctorConsultFee;
                }
                else
                {
                    model.TelemedicineFeeList = telemedicine;
                    // model.ClinicVisitFeeList = telemedicine;
                }
                var ClinicVisit = entitiesDb.DoctorConsult.Where(x => x.DoctorId == model.MemberId && x.AppointmentType == (int)AppointmentType.ClinicVisit).ToList();
                if (ClinicVisit.Count == 0)
                {
                    model.ClinicVisitFeeList = listDoctorConsultFee;
                }
                else
                {
                    model.ClinicVisitFeeList = ClinicVisit;
                    // model.TelemedicineFeeList = ClinicVisit;
                }
                var objprescription = entitiesDb.DoctorConsult.Where(x => x.DoctorId == model.MemberId && x.AppointmentType == (int)AppointmentType.RepeatPrescription).FirstOrDefault();
                if (objprescription != null)
                {
                    model.Prescriptionmodel = new DoctorConsultFee();
                    model.Prescriptionmodel.RepeatPrescription = objprescription.RepeatPrescription;
                }

                var Account = _doctorservice.GetDoctorAccountDetails(model.LoginId).FirstOrDefault();
                if (Account != null)
                {
                    model.DoctorAccountdetail = new DoctorAccountDetails();
                    model.DoctorAccountdetail.AccountName = Account.AccountName;
                    model.DoctorAccountdetail.BankName = Account.BankName;
                    model.DoctorAccountdetail.Bsb = Account.Bsb;
                    model.DoctorAccountdetail.AccountNumber = Account.AccountNumber;
                    model.DoctorAccountdetail.DoctorLoginId = Account.DoctorLoginId;
                    model.DoctorAccountdetail.PaypelEmailId = Account.PaypelEmailId;
                }
                else
                {
                    model.DoctorAccountdetail = new DoctorAccountDetails();
                }
                var om = _organizationservice.GetOrganizationList().Where(x => x.OrganizationId == model.OrgId).FirstOrDefault();
                //.Select(y => new OrganizationViewModel { OrganizationId = y.OrganizationId, OrganizationName = y.OrganizationName, City= _cityService.GetCityById(y.City).LookUpCityId,State=_stateservice.GetStateById(y.State).Id ,Country=_countryService.GetCountryById(y.Country).CountryId}).FirstOrDefault();
                model.organizationName = om.OrganizationName;
                model.OrganizationId = om.OrganizationId;
                model.OrganizationAddress1 = om.Address1;
                model.OrganizationAddress2 = om.Address2;
                model.OrganizationCountryId = om.Country;
                model.OrganizationStateId = om.State;
                model.OrganizationCityId = om.City;
                model.OrganizationPostalCode = om.PostCode;

                //model.Orgmodel = organizatinmdoel;
            }
            else
            {
                model.clinicmodellist = _clinicservice.GetClinicList().Select(y => new DoctorClinicModel { ClinicId = y.ClinicId, ClinicName = y.ClinicName, Checked = false }).ToList();
                model.clinicProviderlist = model.clinicmodellist;
                List<DoctorConsultFee> listDoctorConsultFee = new List<DoctorConsultFee>();
                listDoctorConsultFee.Add(new DoctorConsultFee { VeryLongFee = null });
                model.ClinicVisitFeeList = listDoctorConsultFee;
                model.TelemedicineFeeList = listDoctorConsultFee;
            }
            var acc = _doctorservice.GetDoctorAccountDetails(model.LoginId).FirstOrDefault();
            if (acc != null)
            {
                model.DoctorAccountdetail = new DoctorAccountDetails();
                model.DoctorAccountdetail.AccountName = acc.AccountName;
                model.DoctorAccountdetail.BankName = acc.BankName;
                model.DoctorAccountdetail.Bsb = acc.Bsb;
                model.DoctorAccountdetail.AccountNumber = acc.AccountNumber;
                //model.DoctorAccountdetail.LoginId = acc.DoctorLoginId;
                model.DoctorAccountdetail.PaypelEmailId = acc.PaypelEmailId;
            }
            model.Prescriptionmodel = new DoctorConsultFee();
            return model;
        }
        public ActionResult DoctorList(int? id)
        {
            var organization = _organizationservice.GetOrganizationById(base.loginUserModel.OrganizationId);
            RegisterViewModel model = new RegisterViewModel();
            if (id.HasValue) { ViewBag.message = true; }
            #region recently Doctors

            var doc = _doctorservice.GetDoctorList().Where(t => organization != null && t.OrgId == organization.OrganizationId).ToList();
            for (int i = 0; i < doc.Count; i++)
            {
                doc[i].IsApproved = _userService.Find(doc[i].LoginId).IsApproved;
            }
            var doctor = doc.Where(x => x.IsApproved == false).Select(x =>
            {
                // var clinicUsers = _organizationservice.GetOrganizationById(x.OrgId);
                return new RegisterViewModel
                {
                    ProfilePic = _userService.Find(x.LoginId).Avatar,
                    FullName = StringCipher.Decrypt(x.FirstName) + " " + StringCipher.Decrypt(x.SurName),
                    MemberId = x.MemberId,
                    Email = StringCipher.Decrypt(x.Email),
                    IsActive = x.IsActive,
                    OrgId = x.OrgId,
                    LastUpdateDateInString = x.ModifiedDate == null ? "" : Common.UtcToLocal(Convert.ToDateTime(x.ModifiedDate), x.TimeZoneString).ToString("dd/MM/yyyy h:mm tt"),
                    TimeZoneDisplayName = Common.GetTimeZoneStandardIdAndDisplayName(x.TimeZoneString, true),
                    VoucherId = id,

                };
            }).OrderByDescending(tbl => tbl.ModyfiedDate).ToList();
            //
            for (int i = 0; i < doctor.Count; i++)
            {
                var organizationName = string.Empty;
                try
                {
                    if (_organizationservice.GetOrganizationById(doctor[i].OrgId) != null)
                    {
                        organizationName = StringCipher.Decrypt(_organizationservice.GetOrganizationById(doctor[i].OrgId).OrganizationName);
                        // + "/" + _clinicservice.GetClinicById(doctor[i].OrgId).ClinicName);
                    }
                }
                catch { }

                if (!string.IsNullOrEmpty(organizationName))
                {
                    doctor[i].organizationName = organizationName;
                }
            }
            model.RecentlyDoctors = doctor;
            #endregion
            #region Activate Doctors
            var Actiavatedoc = _doctorservice.GetDoctorList().Where(t => organization != null && t.OrgId == organization.OrganizationId).ToList();
            for (int i = 0; i < Actiavatedoc.Count; i++)
            {
                Actiavatedoc[i].IsApproved = _userService.Find(Actiavatedoc[i].LoginId).IsApproved;
            }
            var Activatedoctor = Actiavatedoc.Where(x => x.IsApproved == true).Select(x =>
            {

                // var clinicUsers = _organizationservice.GetOrganizationById(x.OrgId);
                return new RegisterViewModel
                {
                    ProfilePic = _userService.Find(x.LoginId).Avatar,
                    FullName = StringCipher.Decrypt(x.FirstName) + " " + StringCipher.Decrypt(x.SurName),
                    MemberId = x.MemberId,
                    Email = StringCipher.Decrypt(x.Email),
                    IsActive = x.IsActive,
                    OrgId = x.OrgId,
                    LoginId = x.LoginId,
                    LastUpdateDateInString = x.ModifiedDate == null ? "" : Common.UtcToLocal(Convert.ToDateTime(x.ModifiedDate), x.TimeZoneString).ToString("dd/MM/yyyy h:mm tt"),
                    TimeZoneDisplayName = Common.GetTimeZoneStandardIdAndDisplayName(x.TimeZoneString, true),
                    VoucherId = id,
                    //organizationName = System.Globalization.CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(_organizationservice.GetOrganizationById(x.OrgId).OrganizationName + "/" + _clinicservice.GetClinicById(x.OrgId).ClinicName)
                }; //: new RegisterViewModel();
            }).OrderByDescending(tbl => tbl.ModyfiedDate).ToList();
            for (int i = 0; i < Activatedoctor.Count; i++)
            {
                var organizationName = string.Empty;
                try
                {
                    if (_organizationservice.GetOrganizationById(Activatedoctor[i].OrgId) != null)
                    {
                        organizationName = StringCipher.Decrypt(_organizationservice.GetOrganizationById(Activatedoctor[i].OrgId).OrganizationName);
                        // + "/" + _clinicservice.GetClinicById(doctor[i].OrgId).ClinicName);
                    }
                }
                catch { }

                if (!string.IsNullOrEmpty(organizationName))
                {
                    Activatedoctor[i].organizationName = organizationName;
                }
            }
            model.ActivateDoctors = Activatedoctor;
            #endregion
            return View(model);
        }
        public ActionResult RecentDoctor()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DoctorList(int? id, SearchParametersViewModel searchModel)
        {
            var organization = _organizationservice.GetOrganizationById(base.loginUserModel.OrganizationId);
            RegisterViewModel model = new RegisterViewModel();
            if (id.HasValue) { ViewBag.message = true; }
            #region recently Doctors

            var doc = _doctorservice.GetDoctorList().Where(t => organization != null && t.OrgId == organization.OrganizationId).ToList();
            for (int i = 0; i < doc.Count; i++)
            {
                doc[i].IsApproved = _userService.Find(doc[i].LoginId).IsApproved;
            }
            var doctor = doc.Where(x => x.IsApproved == false).Select(x =>
            {
                // var clinicUsers = _organizationservice.GetOrganizationById(x.OrgId);
                return new RegisterViewModel
                {
                    ProfilePic = _userService.Find(x.LoginId).Avatar,
                    FullName = x.Title != null ? StringCipher.Decrypt(_TitleServices.GetTitleByUserId(x.Title, Convert.ToInt32(RoleType.Doctor)).TitleName) + " " + StringCipher.Decrypt(x.FirstName) + " " + StringCipher.Decrypt(x.SurName) : StringCipher.Decrypt(x.FirstName) + " " + StringCipher.Decrypt(x.SurName),
                    MemberId = x.MemberId,
                    Email = StringCipher.Decrypt(x.Email),
                    IsActive = x.IsActive,
                    OrgId = x.OrgId,
                    LastUpdateDateInString = x.ModifiedDate == null ? "" : Common.UtcToLocal(Convert.ToDateTime(x.ModifiedDate), x.TimeZoneString).ToString("dd/MM/yyyy h:mm tt"),
                    TimeZoneDisplayName = Common.GetTimeZoneStandardIdAndDisplayName(x.TimeZoneString, true),
                    VoucherId = id,
                    CountryId = x.CountryId,
                    State = x.State,

                };
            }).OrderByDescending(tbl => tbl.ModyfiedDate).ToList();
            //
            for (int i = 0; i < doctor.Count; i++)
            {
                var cliniccount = _doctorservice.GetUserclinicList().Where(x => x.Type == (int)RoleType.Doctor && x.UserId == doctor[i].MemberId).ToList().GroupBy(x => x.ClinicId).Select(group => group.First()).ToList();
                //var cliniccount = _userService.GetClinicList(Convert.ToInt32(model.MemberId), (int)RoleType.Doctor);
                var clinicname = string.Empty;
                if (cliniccount != null)
                {
                    for (int j = 0; j < cliniccount.Count; j++)
                    {
                        clinicname += StringCipher.Decrypt(_clinicservice.GetClinicById(cliniccount[j].ClinicId).ClinicName) + ",";
                    }
                }
                var organizationName = string.Empty;
                try
                {
                    if (_organizationservice.GetOrganizationById(doctor[i].OrgId) != null)
                    {
                        organizationName = StringCipher.Decrypt(_organizationservice.GetOrganizationById(doctor[i].OrgId).OrganizationName);
                        // + "/" + _clinicservice.GetClinicById(doctor[i].OrgId).ClinicName);
                    }
                }
                catch { }

                if (!string.IsNullOrEmpty(organizationName))
                {
                    doctor[i].organizationName = organizationName;
                }
                doctor[i].ClinicName = clinicname;
            }
            //model.RecentlyDoctors = doctor;
            #endregion
            #region Activate Doctors
            var Actiavatedoc = _doctorservice.GetDoctorList().Where(t => organization != null && t.OrgId == organization.OrganizationId).ToList(); 
            for (int i = 0; i < Actiavatedoc.Count; i++)
            {
                Actiavatedoc[i].IsApproved = _userService.Find(Actiavatedoc[i].LoginId).IsApproved;
            }
            var Activatedoctor = Actiavatedoc.Where(x => x.IsApproved == true).Select(x =>
            {
                // var clinicUsers = _organizationservice.GetOrganizationById(x.OrgId);
                return new RegisterViewModel
                {
                    ProfilePic = _userService.Find(x.LoginId).Avatar,
                    FullName = x.Title != null ? StringCipher.Decrypt(_TitleServices.GetTitleByUserId(x.Title, Convert.ToInt32(RoleType.Doctor)).TitleName) + " " + StringCipher.Decrypt(x.FirstName) + " " + StringCipher.Decrypt(x.SurName) : StringCipher.Decrypt(x.FirstName) + " " + StringCipher.Decrypt(x.SurName),
                    MemberId = x.MemberId,
                    Email = StringCipher.Decrypt(x.Email),
                    IsActive = x.IsActive,
                    OrgId = x.OrgId,
                    LoginId = x.LoginId,
                    LastUpdateDateInString = x.ModifiedDate == null ? "" : Common.UtcToLocal(Convert.ToDateTime(x.ModifiedDate), x.TimeZoneString).ToString("dd/MM/yyyy h:mm tt"),
                    TimeZoneDisplayName = Common.GetTimeZoneStandardIdAndDisplayName(x.TimeZoneString, true),
                    VoucherId = id,
                    CountryId = x.CountryId,
                    State = x.State,
                    //organizationName = System.Globalization.CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(_organizationservice.GetOrganizationById(x.OrgId).OrganizationName + "/" + _clinicservice.GetClinicById(x.OrgId).ClinicName)
                }; //: new RegisterViewModel();
            }).OrderByDescending(tbl => tbl.ModyfiedDate).ToList();
            for (int i = 0; i < Activatedoctor.Count; i++)
            {
                var cliniccount = _doctorservice.GetUserclinicList().Where(x => x.Type == (int)RoleType.Doctor && x.UserId == Activatedoctor[i].MemberId).ToList().GroupBy(x => x.ClinicId).Select(group => group.First()).ToList();
                //var cliniccount = _userService.GetClinicList(Convert.ToInt32(model.MemberId), (int)RoleType.Doctor);
                var clinicname = string.Empty;
                if (cliniccount != null)
                {
                    for (int j = 0; j < cliniccount.Count; j++)
                    {
                        clinicname += StringCipher.Decrypt(_clinicservice.GetClinicById(cliniccount[j].ClinicId).ClinicName) + ",";
                    }
                }
                var organizationName = string.Empty;
                try
                {
                    if (_organizationservice.GetOrganizationById(Activatedoctor[i].OrgId) != null)
                    {
                        organizationName = StringCipher.Decrypt(_organizationservice.GetOrganizationById(Activatedoctor[i].OrgId).OrganizationName);
                        // + "/" + _clinicservice.GetClinicById(doctor[i].OrgId).ClinicName);
                    }
                }
                catch { }

                if (!string.IsNullOrEmpty(organizationName))
                {
                    Activatedoctor[i].organizationName = organizationName;
                }
                Activatedoctor[i].ClinicName = clinicname;
            }
            //model.ActivateDoctors = Activatedoctor;
            if (doctor != null)
            {
                doctor = doctor.Where(x =>
                     (
                      (searchModel.OrgName != null && x.organizationName != null && x.organizationName.IndexOf(searchModel.OrgName, StringComparison.OrdinalIgnoreCase) >= 0) || searchModel.OrgName == null)
                        && ((searchModel.ClinicName != null && x.ClinicName.Contains(searchModel.ClinicName)) || searchModel.ClinicName == null)
                  && ((searchModel.PostCode != null && x.PostCode.IndexOf(searchModel.PostCode, StringComparison.OrdinalIgnoreCase) >= 0) || searchModel.PostCode == null)
                      && ((searchModel.CountryId != 0 && x.CountryId == searchModel.CountryId) || searchModel.CountryId == 0)
                      && ((searchModel.StateId != 0 && x.State == searchModel.StateId) || searchModel.StateId == 0)
                         //&& ((searchModel.OrgAdmin != 0 && x.FaxNumber == Convert.ToString(searchModel.OrgAdmin)) || searchModel.OrgAdmin == 0)
                      && ((searchModel.ActivationStatus != null && x.IsActive == Convert.ToBoolean(searchModel.ActivationStatus)) ||
                            searchModel.ActivationStatus == null)
                              && ((searchModel.DoctorName != null && x.FullName.IndexOf(searchModel.DoctorName, StringComparison.OrdinalIgnoreCase) >= 0) ||
                           searchModel.DoctorName == null)
                     && ((searchModel.Email != null && x.FullName.IndexOf(searchModel.Email, StringComparison.OrdinalIgnoreCase) >= 0) ||
                           searchModel.Email == null)

                     ).ToList();
            }
            //var cliniccount = _doctorservice.GetUserclinicList().Where(x => x.Type == (int)RoleType.Doctor).ToList();
            ////var cliniccount = _userService.GetClinicList(Convert.ToInt32(model.MemberId), (int)RoleType.Doctor);
            //var clinicname = string.Empty;
            //if (cliniccount != null)
            //{
            //    for (int i = 0; i < cliniccount.Count; i++)
            //    {
            //        clinicname += StringCipher.Decrypt(_clinicservice.GetClinicById(cliniccount[i].ClinicId).ClinicName) + ",";
            //    }
            //}
            // model.ClinicName = clinicname;
            Activatedoctor = Activatedoctor.Where(x =>
                 (
                  (searchModel.OrgName != null && x.organizationName != null && x.organizationName.IndexOf(searchModel.OrgName, StringComparison.OrdinalIgnoreCase) >= 0) || searchModel.OrgName == null)
                  &&
                  ((searchModel.ClinicName != null && x.ClinicName.Contains(searchModel.ClinicName)) || searchModel.ClinicName == null)

                  && ((searchModel.PostCode != null && x.PostCode.IndexOf(searchModel.PostCode, StringComparison.OrdinalIgnoreCase) >= 0) || searchModel.PostCode == null)
                  && ((searchModel.CountryId != 0 && x.CountryId == searchModel.CountryId) || searchModel.CountryId == 0)
                  && ((searchModel.StateId != 0 && x.State == searchModel.StateId) || searchModel.StateId == 0)
                  && ((searchModel.OrgAdmin != 0 && x.FaxNumber == Convert.ToString(searchModel.OrgAdmin)) || searchModel.OrgAdmin == 0)
                  && ((searchModel.ActivationStatus != null && x.IsActive == Convert.ToBoolean(searchModel.ActivationStatus)) ||
                        searchModel.ActivationStatus == null)
                  && ((searchModel.DoctorName != null && x.FullName.IndexOf(searchModel.DoctorName, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        searchModel.DoctorName == null)
                  && ((searchModel.Email != null && x.Email.IndexOf(searchModel.Email, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        searchModel.Email == null)
                //&& ((searchModel.ClinicName != null && x.ClinicName.IndexOf(searchModel.ClinicName, StringComparison.OrdinalIgnoreCase) >= 0) ||
                //      searchModel.ClinicName == null)
                 ).ToList();
            model.RecentlyDoctors = doctor;
            model.ActivateDoctors = Activatedoctor;
            #endregion
            return View(model);
        }
        public ActionResult getClinic(int Id)
        {
            var OrgClinic = _clinicservice.GetClinicInOrganization(Id).Select(x => x.ClinicId).ToList();
            return Json(OrgClinic, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetOrganizationAddress(int Id)
        {
            var OrgAddress = StringCipher.Decrypt(_organizationservice.GetOrganizationById(Id).Address1.ToString());
            // Returns string "Electronic" or "Mail"
            return Json(OrgAddress, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetClinicByID(int Id)
        {
            var Clinic = _clinicservice.GetClinicInOrganization(Id).Select(x => x.ClinicId).ToList();
            // Returns string "Electronic" or "Mail"
            return Json(Clinic, JsonRequestBehavior.AllowGet);
        }
        public RegisterViewModel UploadSignature(RegisterViewModel model, HttpPostedFileBase SignaturPic)
        {
            if (SignaturPic != null && SignaturPic.ContentLength > 0)
            {
                var fileName = Path.GetFileName(SignaturPic.FileName);
                string extension = Path.GetExtension(fileName);
                if (extension != null && (extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".gif"))
                {
                    var imageHelper = new ImageHelper();
                    if (!string.IsNullOrEmpty(model.SignaturePic))
                    {
                        //Delete exising File
                        var file = Server.MapPath(model.SignaturePic);
                        imageHelper.Delete(file);
                    }
                    string fName = DateTime.Now.Ticks + fileName;
                    var path = Path.Combine(Server.MapPath("~/UploadedFiles/OrganizationLogo/"), fName);
                    var bitmap = new Bitmap(SignaturPic.InputStream);
                    imageHelper.Save(bitmap, 150, 150, 250, path);
                    SignaturPic.SaveAs(path);
                    model.SignaturePic = "/UploadedFiles/OrganizationLogo/" + fName;
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
                if (model.SignaturePic != null)
                {
                    model.SignaturePic = model.SignaturePic;
                }
            }
            return model;
        }
        public ActionResult DoctorDetail(int? id)
        {
            if (id.HasValue)
            {
                //var users = new List<RegisterViewModel>();
                #region Detail
                var model = _doctorservice.GetDoctorById(Convert.ToInt32(id)).Select(x => new RegisterViewModel
                {
                    FirstName = StringCipher.Decrypt(x.FirstName),
                    MiddleName = StringCipher.Decrypt(x.MiddleName),
                    SurName = StringCipher.Decrypt(x.SurName),
                    Email = StringCipher.Decrypt(x.Email),
                    DOB = x.DOB,
                    GenderId = Convert.ToInt32(x.Gender),
                    MobileNumber = x.MobileNumber == null ? " " : StringCipher.Decrypt(x.MobileNumber),
                    FaxNumber = x.FaxNumber == null ? " " : StringCipher.Decrypt(x.FaxNumber),
                    City = Convert.ToInt32(x.City),
                    State = Convert.ToInt32(x.State),
                    CountryId = Convert.ToInt32(x.CountryId),
                    Profession = Convert.ToInt32(x.Profession),
                    PostCode = x.PostCode == null ? " " : StringCipher.Decrypt(x.PostCode),
                    Timezone = x.TimeZoneString == null ? " " : StringCipher.Decrypt(x.TimeZoneString),
                    PhoneNumber = x.PhoneNumber == null ? " " : StringCipher.Decrypt(x.PhoneNumber),
                    Address1 = StringCipher.Decrypt(x.Address1 == null ? " " : x.Address1),
                    Address2 = StringCipher.Decrypt(x.Address2 == null ? " " : x.Address2),
                    OrgId = Convert.ToInt32(x.OrgId),
                    LoginId = x.LoginId,
                    IsActive = x.IsActive,
                    PhoneCode = x.PhoneCode == 0 ? 1 : x.PhoneCode,
                    LanguageSpoken = x.LanguageSpoken == null ? " " : StringCipher.Decrypt(x.LanguageSpoken),
                    ProfessionCategory = Convert.ToInt32(x.ProfessionCategory),
                    IntrestArea = x.IntrestArea == null ? " " : StringCipher.Decrypt(x.IntrestArea),
                    qualification = x.qualification == null ? " " : StringCipher.Decrypt(x.qualification),
                    AHPRANo = x.AHPRANo == null ? " " : StringCipher.Decrypt(x.AHPRANo),
                    VideoProviderNo = x.VideoProviderNo == null ? " " : StringCipher.Decrypt(x.VideoProviderNo),
                    HomeVisitProviderNo = x.HomeVisitProviderNo == null ? " " : StringCipher.Decrypt(x.HomeVisitProviderNo),
                    ClinicProviderNo = x.ClinicProviderNo == null ? " " : StringCipher.Decrypt(x.ClinicProviderNo),
                    PrescriberNo = x.PrescriberNo == null ? " " : StringCipher.Decrypt(x.PrescriberNo),
                    DoctorProfile = x.DoctorProfile == null ? " " : StringCipher.Decrypt(x.DoctorProfile),
                    SignaturePic = x.SignaturePic,
                    MemberId = x.MemberId,
                    UserorgId = x.OrgId,
                    DoctorLoginId = x.LoginId,
                    ModyfiedDate = (DateTime)x.ModifiedDate,
                    GenderName = x.Gender == null ? "" : _genderService.GetGenderById(x.Gender).GenderName
                }).ToList().FirstOrDefault();
                model.FullName = model.MiddleName == null ? StringCipher.Decrypt(model.FirstName) + " " + StringCipher.Decrypt(model.SurName) : StringCipher.Decrypt(model.FirstName) + " " + StringCipher.Decrypt(model.MiddleName) + " " + StringCipher.Decrypt(model.SurName);

                if (model.OrgId != 0)
                {
                    try
                    {
                        model.organizationName = StringCipher.Decrypt(_organizationservice.GetOrganizationById(model.OrgId).OrganizationName);
                        model.OrganizationAddress1 = StringCipher.Decrypt(_organizationservice.GetOrganizationById(model.OrgId).Address1);
                        model.OrganizationPostalCode = StringCipher.Decrypt(_organizationservice.GetOrganizationById(model.OrgId).PostCode);
                        model.OrganizationAddress2 = StringCipher.Decrypt(_organizationservice.GetOrganizationById(model.OrgId).Address1);
                        //model.Address2 = StringCipher.Decrypt(_organizationservice.GetOrganizationById(model.OrgId).Address2);
                        var orgCountry = _organizationservice.GetOrganizationById(model.OrgId).Country;
                        model.OrgCountryName = _countryService.GetCountryById(orgCountry).CountryName;
                        var ClinicState = _organizationservice.GetOrganizationById(model.OrgId).State;
                        model.OrgStateName = _stateservice.GetStateById(orgCountry).StateName;
                        var ClinicCity = _organizationservice.GetOrganizationById(model.OrgId).City;
                        model.OrgCityName = _cityService.GetCityById(ClinicCity).LookUpCityName;
                    }
                    catch { }
                }
                if (model.MiddleName != null)
                {
                    model.FullName = StringCipher.Decrypt(model.FirstName) + " " + StringCipher.Decrypt(model.MiddleName) + " " + StringCipher.Decrypt(model.SurName);
                }
                else
                {
                    model.FullName = StringCipher.Decrypt(model.FirstName) + " " + StringCipher.Decrypt(model.SurName);
                }
                if (model.ClinicID != 0)
                {
                    model.ClinicName = StringCipher.Decrypt(_clinicservice.GetClinicById(model.ClinicID).ClinicName);
                    model.ClinicAddress = StringCipher.Decrypt(_clinicservice.GetClinicById(model.ClinicID).Address1);
                }
                if (model.GenderId != 0)
                {
                    model.GenderName = StringCipher.Decrypt(_genderService.GetGenderById(model.GenderId).GenderName);
                }
                if (model.CountryId != 0)
                {
                    model.CountryName = StringCipher.Decrypt(_countryService.GetCountryById(model.CountryId).CountryName);
                }
                if (model.State != 0)
                {
                    model.StateName = StringCipher.Decrypt(_stateservice.GetStateById(model.State).StateName);
                }
                if (model.City != 0)
                {
                    model.CityName = StringCipher.Decrypt(_cityService.GetCityById(model.City).LookUpCityName);
                }

                if (model.MemberId > 0)
                {

                    model.clinicmodellist = _doctorservice.GetclinicByDoctorId(model.MemberId, Convert.ToInt32(RoleType.Doctor)).Select(y => new DoctorClinicModel { ClinicId = y.ClinicId, ClinicName = StringCipher.Decrypt(_clinicservice.GetClinicById(y.ClinicId).ClinicName), ClinicAddress1 = StringCipher.Decrypt(_clinicservice.GetClinicById(y.ClinicId).Address1), ClinicAddress2 = StringCipher.Decrypt(_clinicservice.GetClinicById(y.ClinicId).Address2), ClinicVoucherNo = StringCipher.Decrypt(y.ClinicProviderNo), Postcode = StringCipher.Decrypt(_clinicservice.GetClinicById(y.ClinicId).PostCode), PhoneNumber = StringCipher.Decrypt(_clinicservice.GetClinicById(y.ClinicId).PhoneNumber), CityName = StringCipher.Decrypt(model.CityName), StateName = StringCipher.Decrypt(model.StateName), CountryName = StringCipher.Decrypt(model.CountryName), Checked = false }).ToList();
                    var telemedicine = entitiesDb.DoctorConsult.Where(x => x.DoctorId == model.MemberId && x.AppointmentType == (int)AppointmentType.VideoConsult).ToList();
                    if (telemedicine.Count > 0)
                    {
                        model.TelemedicineFeeList = telemedicine;
                    }
                    var ClinicVisit = entitiesDb.DoctorConsult.Where(x => x.DoctorId == model.MemberId && x.AppointmentType == (int)AppointmentType.ClinicVisit).ToList();
                    if (ClinicVisit.Count > 0)
                    {
                        model.ClinicVisitFeeList = ClinicVisit;
                    }


                }

                if (Convert.ToInt32(model.Profession) != 0)
                {
                    model.ProfessionName = System.Globalization.CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(_professiontypeservice.GetProfessionTypesById(Convert.ToInt32(model.Profession)).ProfessionName);
                }
                try
                {
                    model.ProfilePic = _userService.Find(model.LoginId).Avatar;
                }
                catch { }
                #endregion
                //// future consult
                #region FutureConsult
                var commonUtilities = new CommonUtilities();
                var FutureConsult = _AppointmentService.GetAppointmentList().Select(x => new AppointmentViewModel
                {
                    AppointmentDateLocalText = x.AppointmentDateLocal.ToString("dd/MM/yyyy h:mm tt"),
                    AppointmentId = x.AppointmentId,
                    AppointmentTypeText = (x.AppointmentType == (int)AppointmentType.VideoConsult ?
                                          commonUtilities.GetDisplayName(AppointmentType.VideoConsult) :
                                          x.AppointmentType == (int)AppointmentType.ClinicVisit ?
                                          commonUtilities.GetDisplayName(AppointmentType.ClinicVisit) :
                                          commonUtilities.GetDisplayName(AppointmentType.HomeVisit)),
                    PatientName = StringCipher.Decrypt(_userService.Find(x.PatientLoginId).Member.FirstName) + " " + StringCipher.Decrypt(_userService.Find(x.PatientLoginId).Member.SurName),
                    AppointmentDateUtc = x.AppointmentDateUtc,
                    AppointmentStatus = x.AppointmentStatus,
                    Address1 = StringCipher.Decrypt(_userService.Find(x.PatientLoginId).Member.Address1),
                    Address2 = StringCipher.Decrypt(_userService.Find(x.PatientLoginId).Member.Address2),
                    Country = _userService.Find(x.PatientLoginId).Member.CountryId,
                    CountryName = StringCipher.Decrypt(_countryService.GetCountryById(_userService.Find(x.PatientLoginId).Member.CountryId).CountryName),
                    AppointmentStatusName = (x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.Pending ?
                                                              commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.Pending) :
                                                              x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.Approved ?
                                                              commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.Approved) :
                                                              commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.Completed)),
                    MemberId = _userService.Find(x.PatientLoginId).Member.MemberId,
                    IsSoapNotesAdded = _AppointmentService.CheckSoapNotes(x.AppointmentId) ? true : false

                }).Where(x => x.AppointmentStatus == (int)AppointmentStatus.Approved || (x.IsSoapNotesAdded == false && x.AppointmentStatus == (int)AppointmentStatus.Completed)).ToList();
                #endregion
                #region PastConsult
                var commonUtility = new CommonUtilities();

                var Pastconsult = _AppointmentService.GetAppointmentList().Select(x => new AppointmentViewModel
                {
                    AppointmentDateLocalText = x.AppointmentDateLocal.ToString("dd/MM/yyyy h:mm tt"),
                    AppointmentId = x.AppointmentId,
                    AppointmentTypeText = (x.AppointmentType == (int)AppointmentType.VideoConsult ?
                                          commonUtility.GetDisplayName(AppointmentType.VideoConsult) :
                                          x.AppointmentType == (int)AppointmentType.ClinicVisit ?
                                          commonUtility.GetDisplayName(AppointmentType.ClinicVisit) :
                                          commonUtility.GetDisplayName(AppointmentType.HomeVisit)),


                    PatientName = StringCipher.Decrypt(_userService.Find(x.PatientLoginId).Member.FirstName) + " " + StringCipher.Decrypt(_userService.Find(x.PatientLoginId).Member.SurName),
                    AppointmentDateUtc = x.AppointmentDateUtc,
                    AppointmentStatus = x.AppointmentStatus,
                    IsSoapNotesAdded = _AppointmentService.CheckSoapNotes(x.AppointmentId) ? true : false

                }).Where(x => x.AppointmentStatus != (int)AppointmentStatus.Pending && x.AppointmentStatus != (int)AppointmentStatus.Approved || (x.IsSoapNotesAdded == true && x.AppointmentStatus == (int)AppointmentStatus.Completed)).ToList();
                // x.DoctorLoginId == base.loginUserModel.LoginId &&
                var a = (int)AppointmentStatus.Pending;
                #endregion
                // var dsfdsf = _userService.GetPatientById(1).Select(y => y.FirstName + " " + y.SurName).FirstOrDefault().ToList();
                var mypatient = _doctorservice.GetPatientList(Convert.ToInt32(id)).Select(x => new RegisterViewModel
                {
                    MemberId = x.PatientId,
                    FullName = StringCipher.Decrypt(_userService.GetPatientById(x.PatientId).Select(y => y.FirstName).FirstOrDefault()) + " " + StringCipher.Decrypt(_userService.GetPatientById(x.PatientId).Select(y => y.SurName).FirstOrDefault()),
                    LoginId = _userService.GetPatientById(x.PatientId).FirstOrDefault().LoginId
                    //LoginId=
                }).ToList().GroupBy(x => x.MemberId).Select(group => group.First()).ToList();

               
                //for (int i = 0; i < mypatient.Count;i++ )
                //{
                //    mypatient[i].FullName = StringCipher.Decrypt(mypatient[i].FullName);
                //}
                model.Mypatientlist = mypatient;
                model.membership = _doctorservice.GetDoctorMemebershipbyLoginId(model.LoginId);
                if (model.membership != null)
                {
                    model.paymentType = (model.membership.MembershipPaidBy == (int)MembershipTakenBy.PayPal ? commonUtilities.GetDisplayName(MembershipTakenBy.PayPal) : commonUtilities.GetDisplayName(MembershipTakenBy.Voucher));
                }
                model.Appointmentmodel = FutureConsult;
                model.PastConsultModel = Pastconsult;


                var Account = _doctorservice.GetDoctorAccountDetails(model.LoginId).FirstOrDefault();
                if (Account != null)
                {
                    model.BankName = Account.BankName;
                    model.AccountName = Account.AccountName;
                    model.Bsb = Account.Bsb;
                    model.AccountNumber = Account.AccountNumber;
                    model.PaypelEmailId = Account.PaypelEmailId;
                }
                return View(model);

            }
            else
            {
                RegisterViewModel model = new RegisterViewModel();
                return View(model);
            }
        }
        public ActionResult Acitvate(FormCollection collection)
        {
            var ids = collection["ActivateIds"];
            _doctorservice.Activate(ids);
            return RedirectToAction("DoctorList");
        }
        public ActionResult Delete(FormCollection collection)
        {
            var ids = collection[0];
            _doctorservice.Delete(ids);
            return RedirectToAction("DoctorList");
        }
        [HttpGet]
        [ChildActionOnly]
        public ActionResult DoctorMembership(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction("DoctorList", "Doctor", new { @area = "admin" });
            }

            var doctorMembershipViewModel = new DoctorMembershipViewModel();
            //doctorMembershipViewModel.DoctorId = Convert.ToInt32(id);
            doctorMembershipViewModel.LoginId = (Guid)id;
            // doctorMembershipViewModel.LoginId=
            return View(doctorMembershipViewModel);
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult DoctorMembership(DoctorMembershipViewModel doctorMembershipViewModel)
        {
            string bodfi = StringCipher.Decrypt("TemB4U6RIFkawteezjmUbQ==");
            var voucher = _voucherService.GetVoucherByNumber(StringCipher.Encrypt((doctorMembershipViewModel.VoucherNo)));
            if (voucher == null)
            {
                TempData["ErrorMessage"] = "Invalid voucher number";
                return View();
            }
            else if (voucher != null)
            {
                //  var voucherAssign = _voucherService.GetVoucherDetailsById(voucher.VoucherID);
                var voucherAssign = _voucherService.GetVoucherDetailsByuserId(voucher.VoucherID);
                if (voucherAssign == null)
                {
                    TempData["ErrorMessage"] = "Invalid voucher number";
                    return View();
                }
                else
                {
                    if (voucherAssign.UserID != doctorMembershipViewModel.LoginId)
                    {
                        TempData["ErrorMessage"] = "Invalid voucher number";
                        return View();
                    }
                    else if (voucher.Status == (int)VoucherStatus.Used)
                    {
                        TempData["ErrorMessage"] = "Voucher number already used";
                        return View();
                    }
                    else if (Convert.ToDateTime(voucher.ExpiryDate).Date < DateTime.Now.Date)
                    {
                        TempData["ErrorMessage"] = "Voucher number has expired";
                        return View();
                    }
                    else
                    {
                        //GetDoctorMembershipByLoginId
                        //var doctorMembership = _doctorMembershipService.GetDoctorMembershipByDoctorId(doctorMembershipViewModel.DoctorId);
                        var doctorMembership = _doctorMembershipService.GetDoctorMembershipByLoginId(doctorMembershipViewModel.LoginId);
                        if (doctorMembership != null)
                        {
                            if (Convert.ToDateTime(doctorMembership.EndDate) > DateTime.Now.Date)
                            {
                                TempData["ErrorMessage"] = "Your membership is not expired yet";
                                return View();
                            }
                            else
                            {
                                //Renew
                            }
                        }
                        else
                        {
                            doctorMembership = new DoctorMembership();
                            doctorMembership.DoctorId = doctorMembershipViewModel.DoctorId;
                            doctorMembership.StartDate = (DateTime)doctorMembershipViewModel.StartDate;
                            doctorMembership.EndDate = doctorMembership.StartDate.AddMonths(voucher.ValidityMonth);
                            doctorMembership.MembershipPaidBy = (int)MembershipTakenBy.Voucher;
                            _doctorMembershipService.Add(doctorMembership);
                            var memberShipVoucher = new MemberShipVoucher();
                            memberShipVoucher.DoctorMembershipId = doctorMembership.DoctorMembershipId;
                            memberShipVoucher.VoucherID = voucher.VoucherID;
                            _membeshipVoucherService.Add(memberShipVoucher);
                            voucher.Status = (int)VoucherStatus.Used;
                            _voucherService.Update(voucher);
                        }
                    }
                }
            }
            return RedirectToAction("DoctorList", "Doctor", new { @area = "admin" });
        }
        [HttpPost]
        public ActionResult GetClinic(int id)
        {
            return View();
        }
        public JsonResult GetOrganizationList(string query = "")
        {
            query = query.Trim();

            var org = string.IsNullOrEmpty(query) ? _organizationservice.GetOrganizationList().Select(x =>
                 new OrganizationViewModel
                 {
                     OrganizationName = StringCipher.Decrypt(x.OrganizationName),
                     OrganizationId = x.OrganizationId,
                     OrganizationType = x.OrganizationType,
                     LoginId = x.LoginId,
                     Address1 = StringCipher.Decrypt(x.Address1),
                     FaxNumber = x.FaxNumber,
                     PostCode = StringCipher.Decrypt(x.PostCode),
                     Address2 = StringCipher.Decrypt(x.Address2),
                     PhoneNumber = x.PhoneNumber,
                     OrganizationLogo = x.OrganizationLogo,
                     Country = x.Country,
                     State = x.State,
                     City = x.City,
                     ModifiedDate = x.ModifiedDate,
                     ClinicList = _clinicservice.GetClinicInOrganization(x.OrganizationId).ToList(),
                     IsActive = x.IsActive,
                     // OrganizationTypeName = _organizationTypeService.GetOrganizarionTypesById(x.OrganizationType).OrganizationTypeName,
                 }).ToList()
            : _organizationservice.GetOrganizationList().Where(x => x.OrganizationName.Contains(query)).Select(x =>
                 new OrganizationViewModel
                 {
                     OrganizationName = StringCipher.Decrypt(x.OrganizationName),
                     OrganizationId = x.OrganizationId,
                     OrganizationType = x.OrganizationType,
                     LoginId = x.LoginId,
                     Address1 = StringCipher.Decrypt(x.Address1),
                     FaxNumber = x.FaxNumber,
                     PostCode = StringCipher.Decrypt(x.PostCode),
                     Address2 = StringCipher.Decrypt(x.Address2),
                     PhoneNumber = x.PhoneNumber,
                     OrganizationLogo = x.OrganizationLogo,
                     Country = x.Country,
                     State = x.State,
                     City = x.City,
                     ModifiedDate = x.ModifiedDate,
                     ClinicList = _clinicservice.GetClinicInOrganization(x.OrganizationId).ToList(),
                     IsActive = x.IsActive,

                 }).ToList();

            return Json(org, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetClinicListByOrganization(string query = "", string OrgId = "", string ClinicIds = "")
        {
            query = query.Trim();
            var clinic = string.IsNullOrEmpty(query) ? _clinicservice.GetClinicInByOrg(OrgId).Select(x =>
                new ClinicModel
                {
                    ClinicName = StringCipher.Decrypt(x.ClinicName),
                    ClinicId = x.ClinicId,
                    LoginId = x.LoginId,
                    Address1 = StringCipher.Decrypt(x.Address1),
                    FaxNumber = x.FaxNumber,
                    PostCode = StringCipher.Decrypt(x.PostCode),
                    Address2 = StringCipher.Decrypt(x.Address2),
                    PhoneNumber = x.PhoneNumber,
                    Country = x.Country,
                    State = x.State,
                    City = x.City,
                    IsActive = x.IsActive,
                    OrganizationId = x.organization.OrganizationId,

                }).ToList()
            : _clinicservice.GetClinicInByOrg(OrgId).Where(x => x.ClinicName.Contains(query)).Select(x =>
                new ClinicModel
                {
                    ClinicName = StringCipher.Decrypt(x.ClinicName),
                    ClinicId = x.ClinicId,
                    LoginId = x.LoginId,
                    Address1 = StringCipher.Decrypt(x.Address1),
                    FaxNumber = x.FaxNumber,
                    PostCode = StringCipher.Decrypt(x.PostCode),
                    Address2 = StringCipher.Decrypt(x.Address2),
                    PhoneNumber = x.PhoneNumber,
                    Country = x.Country,
                    State = x.State,
                    City = x.City,
                    IsActive = x.IsActive,
                    OrganizationId = x.organization.OrganizationId,

                }).ToList();

            clinic = clinic.Where(x => !ClinicIds.Contains(Convert.ToString(x.ClinicId))).ToList();
            return Json(clinic, JsonRequestBehavior.AllowGet);
        }
        public ActionResult MyPatient(int? id)
        {
            try
            {   
                var model = _doctorservice.GetPatientList(Convert.ToInt32(id)).Select(x => new RegisterViewModel { MemberId = x.PatientId, FullName = _userService.GetPatientById(x.PatientId).Select(y => y.FirstName + " " + y.SurName).FirstOrDefault() }).ToList().GroupBy(x => x.MemberId).Select(group => group.First()).ToList();
                return View(model);
            }
            catch
            {
                RegisterViewModel model = new RegisterViewModel();
                return View();
            }
        }

    }
}