using MyOnlineClinic.Emailer;
using MyOnlineClinic.Entity;
using MyOnlineClinic.RepositoryService;
using MyOnlineClinic.Web.Controllers;
using MyOnlineClinic.Web.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyOnlineClinic.Web.Helper;
using System.Globalization;
using System.Net;
using PayPal.Api;
using System.Web.Routing;
using System.Threading.Tasks;
using System.Data.SqlClient;
using MyOnlineClinic.Migrator;

namespace MyOnlineClinic.Web.Areas.Clinic.Controllers
{
    public class PatientController : BaseController
    {
        private readonly IAppointmentService appointmentService = null;
        private readonly IHomeVisitAppointmentService _homeVisitAppointmentService = null;
        private readonly IDoctorService doctorService = null;
        private readonly IUserService userService = null;
        protected readonly IUserService UserService;
        private readonly IOrganizationService _organiztionService;
        private readonly IClinicService _clinicService;
        private readonly ILoginHelper _loginHelper;
        private readonly ICountryService _countryService;
        private readonly ICityService _cityService;
        private readonly ILookUpModuleService _moduleService;
        private readonly IPermissionsService _permissionService;
        private readonly ILookUpUserRolesService _roleService;
        private readonly IPermissionInModuleService _permissioninmoduleService;
        private readonly IAdminUserService _adminuserService;
        private readonly ITimeZoneService _timeZoneService;
        private readonly ITitleService _TitleServices;
        private readonly IGenderService _genderService;
        private readonly IStateService _stateService;
        public readonly IPatientHistoryService _patientHistoryService;
        private readonly IUregisteredOrganizationService _unregisteredOrganizationService;
        IProfessionTypeService _professiontypeservice;
        IPaymentHelper _paymentService;
        private readonly IPatientService _patientServices;
        private readonly IUnregisteredClinicService _unregisteredClinicService;
        private readonly IPaymentService _servicePayment;
        private readonly IClinicUserService _clinicUserService;


        public PatientController(IAppointmentService appointmentService, IDoctorService doctorService, IUserService userService, IOrganizationService organiztionService, IClinicService clinicService,
          ICountryService countryService, ICityService cityService, ILookUpModuleService moduleService, IPermissionsService permissionService, ILookUpUserRolesService roleService,
          IPermissionInModuleService permissioninmoduleService, IAdminUserService adminuserService, ITimeZoneService timezoneService, ITitleService TitleServices, IGenderService genderService, IStateService stateService, ILoginHelper loginHelper, IUregisteredOrganizationService unregisteredOrganizationService,
          IProfessionTypeService professiontypeservice, IPaymentHelper paymentService, IPatientService patientServices, IPaymentService servicePayment, IUnregisteredClinicService unregisteredClinicService, IHomeVisitAppointmentService homeVisitAppointmentService, IPatientHistoryService patientHistoryService)
        {
            this.appointmentService = appointmentService;
            this.doctorService = doctorService;
            this.userService = userService;
            UserService = userService;
            _organiztionService = organiztionService;
            _clinicService = clinicService;
            _loginHelper = loginHelper;
            _countryService = countryService;
            _cityService = cityService;
            _moduleService = moduleService;
            _permissionService = permissionService;
            _roleService = roleService;
            _permissioninmoduleService = permissioninmoduleService;
            _adminuserService = adminuserService;
            _timeZoneService = timezoneService;
            _TitleServices = TitleServices;
            _genderService = genderService;
            _stateService = stateService;
            _unregisteredOrganizationService = unregisteredOrganizationService;
            _professiontypeservice = professiontypeservice;
            _paymentService = paymentService;
            _patientServices = patientServices;
            _servicePayment = servicePayment;
            _unregisteredClinicService = unregisteredClinicService;
            _homeVisitAppointmentService = homeVisitAppointmentService;
            _patientHistoryService = patientHistoryService;
        }

        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult BookAppointment()
        {
            SearchViewModel model = new SearchViewModel();
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult BookAppointment(FormCollection collection, SearchViewModel model)
        {


            int DoctorId = 0;
            var Docid = model.MemberId;

            DoctorId = Convert.ToInt32(Docid);

            Appointment cModel = new Appointment();
            var AppointmentExisit = appointmentService.CheckAppointment(Convert.ToDateTime(TempData["AppointmentLocal"]));
            if (AppointmentExisit)
            {
                model.Confirm = "Exist";
                return View(model);
            }

            if (DoctorId > 0)
            {
                var doctorResult = doctorService.GetDoctorById(DoctorId).FirstOrDefault();
                if (doctorResult != null)
                {
                    cModel.DoctorLoginId = doctorResult.LoginId;

                }

                cModel.PatientLoginId = Guid.Parse(TempData["PatientId"].ToString());
                cModel.AppointmentDateLocal = Convert.ToDateTime(TempData["AppointmentLocal"]);
                cModel.AppointmentStatus = (int)AppointmentStatus.Pending;
                cModel.AppointmentDateUtc = DateTime.Now;
                cModel.AppointmentType = (int)AppointmentType.VideoConsult;
                appointmentService.Add(cModel);
                model.Confirm = "Book";
            }
            var patient = userService.GetPatientByLoginId(Guid.Parse(TempData["PatientId"].ToString())).MemberId;
            return RedirectToAction("PayWithCreditCard", new
            {
                id = cModel.AppointmentId,
                patientId = patient
            });

        }

        public ActionResult Search(int? id, string name)//, string name
        {
            SearchViewModel model = new SearchViewModel();
            ReadOnlyCollection<TimeZoneInfo> tz;
            tz = TimeZoneInfo.GetSystemTimeZones();
            var timeZoneList = tz.Select(p => new SelectListItem { Value = p.DisplayName, Text = p.DisplayName }).ToList();
            ViewBag.Timezone = timeZoneList;
            model.MemberId = Convert.ToInt32(id);
            if (id.HasValue)
            {
                var Cmodel = UserService.GetPatientById(Convert.ToInt32(id)).Select(x => new SearchViewModel
                   {
                       PatientId = x.LoginId,
                       org = x.OrgId,
                       Timezone = x.TimeZoneString,

                   }).FirstOrDefault();

                var Omodel = _organiztionService.GetOrganizationById(Cmodel.org);
                TempData["PatientId"] = Cmodel.PatientId;
                TempData["OrgId"] = Cmodel.org;
                TempData["orgType"] = Omodel.OrganizationType;
                TempData["Timezone"] = Cmodel.Timezone;
                TempData.Keep("PatientId");
                TempData.Keep("OrgId");
                TempData.Keep("orgType");
                TempData.Keep("Timezone");
            }
            BindType(model);
            //    model.AppointmentType = (int)AppointmentType.VideoConsult;
            if (!string.IsNullOrEmpty(name))
            {
                model.AppointmentType = Convert.ToInt32(name);

            }
            return View(model);

        }

        //[HttpPost]
        //[AllowAnonymous]
        //public ActionResult Search(SearchViewModel model, FormCollection collection)
        //{

        //    ReadOnlyCollection<TimeZoneInfo> tz;
        //    tz = TimeZoneInfo.GetSystemTimeZones();
        //    var timeZoneList = tz.Select(p => new SelectListItem { Value = p.DisplayName, Text = p.DisplayName }).ToList();
        //    ViewBag.Timezone = timeZoneList;
        //    var appointmentlocal = collection.Get("AppointmentDateLocal");
        //    string AppointmentTimeCHeck = collection.Get("FromHour");
        //    if (model.AppointmentDateLocal == null && AppointmentTimeCHeck == "")
        //    {
        //        BindType(model);
        //        ModelState.AddModelError("Error", "Please Select Appointment Date Or time");
        //        return View(model);
        //    }
        //    // var doctorIdList = _clinicService.GetDoctorListByClinicId(Convert.ToInt32(model.Clinicid), (int)RoleType.Doctor);
        //    //List<Entity.Doctor> listDoctor = new List<Entity.Doctor>();
        //    //var doctorIdList = _clinicService.GetDoctorListByClinicId(Convert.ToInt32(clinicid), (int)RoleType.Doctor);

        //    //for (int i = 0; i < doctorIdList.Count; i++)
        //    //{
        //    //    var doctor = doctorService.GetDoctorById(doctorIdList[i].UserId).FirstOrDefault();
        //    //    listDoctor.Add(doctor);
        //    //}

        //    var doctor = doctorService.GetQueryableDoctors();

        //    if (model.DoctorFullName != null)
        //    {
        //        doctor = doctor.FilterByFullName(model.DoctorFullName);
        //    }
        //    if (model.Clinicid > 0)
        //    {
        //        doctor = doctor.FilterByClinic(model.Clinicid);


        //    }
        //    if (model.ProfessionType > 0)
        //    {
        //        doctor = doctor.FilterByCategory(model.ProfessionType);
        //    }
        //    if (model.ProfessionSub > 0)
        //    {
        //        doctor = doctor.FilterByProfessionSubCategory(model.ProfessionSub);
        //    }
        //    if (model.countryid > 0)
        //    {
        //        doctor = doctor.FilterByCountry(model.countryid);
        //    }
        //    if (model.stateid > 0)
        //    {
        //        doctor = doctor.FilterByCountry(model.stateid);
        //    }

        //    if (model.cityid > 0)
        //    {
        //        doctor = doctor.FilterByCity(model.cityid);
        //    }
        //    if (model.language != null)
        //    {
        //        doctor = doctor.FilterByLanguage(model.language);
        //    }

        //    var search = doctor.Select
        //        (x => new SearchViewModel
        //        {
        //            DoctorFullName = x.FirstName,
        //            Qualification = x.qualification,
        //            PhoneNo = x.PhoneNumber,
        //            DoctorProfile = x.DoctorProfile,
        //            DoctorLoginId = x.LoginId,
        //            MemberId = x.MemberId
        //        }).ToList();

        //    //var cityResult = search.Join(
        //    //                             _unregisteredClinicService.GetClinicById(model.Clinicid),
        //    //                             clinic => clinic.Clinicid,
        //    //                             (news, city) => new { News = news, City = city }
        //    //                           )
        //    //                           .GroupBy(g => g.City)
        //    //                            .Select(g => new FilterObject
        //    //                            {
        //    //                                Id = g.Key.LookUpCityId,
        //    //                                Title = g.Key.LookUpCityName,
        //    //                                Count = g.Count()
        //    //                            }).ToList();

        //    for (int i = 0; i < search.Count; i++)
        //    {
        //        var ProfilePic = string.Empty;
        //        try
        //        {
        //            if (doctorService.GetDoctorById(search[i].MemberId) != null)
        //            {

        //                search[i].ProfilePic = UserService.Find(search[i].DoctorLoginId).Avatar;

        //            }

        //        }
        //        catch { }


        //    }

        //    model.AppointmentList = search;
        //    string AppointmentTime = collection.Get("FromHour") + ":" + collection.Get("FromMin") + " " + collection.Get("FromAMPM");
        //    DateTime dtAppointdate = DateTime.ParseExact(appointmentlocal, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //    string AppointmentLocal = dtAppointdate.ToString("dd/MM/yyyy").ToString() + " " + AppointmentTime;
        //    var timezone = TempData["Timezone"];
        //    TempData["AppointmentLocal"] = AppointmentLocal;
        //    TempData.Keep("AppointmentLocal");
        //    BindType(model);
        //    return View(model);
        //}

        //GET ://patietn Registraion by Admin

        [AllowAnonymous]
        public ActionResult Add(int? id)
        {
            RegisterViewModel model = new RegisterViewModel();
            if (id.HasValue)
            {
                var Cmodel = UserService.GetPatientById(Convert.ToInt32(id)).Select(x => new RegisterViewModel
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
                    DOB = x.DOB,
                    PostCode = StringCipher.Decrypt(x.PostCode),
                    MemberId = x.MemberId,
                    MiddleName = StringCipher.Decrypt(x.MiddleName),
                    MedicareNumber = StringCipher.Decrypt(x.MedicareNumber),
                    MedicareRefNumber = StringCipher.Decrypt(x.MedicareRefNo),
                    MedicareExpire = x.Expiry,
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
                    // organizationName = _organiztionService.GetOrganizationById(x.OrgId).OrganizationName,
                    PharmacyName = StringCipher.Decrypt(x.PharmacyName),
                    PharmacyAddress1 = StringCipher.Decrypt(x.PharmacyAddress1),
                    PharmacyAddress2 = StringCipher.Decrypt(x.PharmacyAddress2),
                    PharmacySuburb = StringCipher.Decrypt(x.PharmacySuburb),
                    PharmacyCity = x.PharmacyCity,
                    PharmacyCountryId = x.PharmacyCountryId,
                    PharmacyFaxNumber = StringCipher.Decrypt(x.PharmacyFaxNumber),
                    PharmacyMobileNumber = StringCipher.Decrypt(x.PharmacyMobileNumber),
                    PharmacyPhoneNumber = StringCipher.Decrypt(x.PharmacyPhoneNumber),
                    PharmacyPostCode = StringCipher.Decrypt(x.PharmacyPostCode),
                    PharmacyState = x.PharmacyState,
                    GernealNotes = x.GernealNotes,
                    TreatingDoctors = StringCipher.Decrypt(x.TreatingDoctors),
                    GenderId = x.Gender,
                    TitleId = x.Title,
                    OrgId = x.OrgId,
                    LoginId = x.LoginId,


                }).FirstOrDefault();

                Cmodel.OrgClinicIdsList = _patientServices.GetClinicById(Convert.ToInt32(id)).Select(x =>
                    new RegisterViewModel
                    {
                        UserClinicId = x.ClinicId,
                        UserClinicName = StringCipher.Decrypt(x.Clinics.ClinicName),
                        UserClinicAddress1 = StringCipher.Decrypt(x.Clinics.Address1),
                        UserClinicAddress2 = StringCipher.Decrypt(x.Clinics.Address2),
                        UserClinicPostalCode = StringCipher.Decrypt(x.Clinics.PostCode),
                        UserClinicCountryId = x.Clinics.Country,
                        UserClinicStateId = x.Clinics.State,
                        UserClinicCityId = x.Clinics.City


                    }).ToList();
                for (int i = 0; i < Cmodel.OrgClinicIdsList.Count; i++)
                {
                    Cmodel.UpdateOrgClinicIds += Cmodel.OrgClinicIdsList[i].UserClinicId + ",";
                }

                if (model.State != null)
                {
                    model.StateName = StringCipher.Decrypt(_stateService.GetStateById(model.State).StateName);

                }

                if (model.CountryId != null)
                {
                    model.CountryName = StringCipher.Decrypt(_countryService.GetCountryById(model.CountryId).CountryName);

                }
                if (model.City != null)
                {
                    model.CityName = StringCipher.Decrypt(_cityService.GetCityById(model.City).LookUpCityName);

                }
                if (model.PharmacyState != null)
                {

                    model.PharmacyStateName = StringCipher.Decrypt(_stateService.GetStateById(model.PharmacyState).StateName);
                }

                if (model.PharmacyCountryId != null)
                {

                    model.PharmacyCountryName = StringCipher.Decrypt(_countryService.GetCountryById(model.PharmacyCountryId).CountryName);
                }
                if (model.PharmacyCity != null)
                {

                    model.PharmacyCityName = StringCipher.Decrypt(_cityService.GetCityById(model.PharmacyCity).LookUpCityName);
                }
                if (model.ProfilePic != null)
                {
                    model.ProfilePic = UserService.Find(model.LoginId).Avatar;
                }
                if (Cmodel.ProfilePic == null)
                {
                    Cmodel.ProfilePic = UserService.Find(Cmodel.LoginId).Avatar;
                }
                BindDropDown(Cmodel);

                return View(Cmodel);
            }
            else
            {
                BindDropDown(model);
                var clinic = _clinicService.GetClinicByLoginId(base.loginUserModel.LoginId);
                if (clinic != null)
                {
                    model.ClinicID = clinic.ClinicId;
                    model.UserorgId = clinic.OrganizationId;
                }

                return View(model);
            }
        }
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult Add(RegisterViewModel model, HttpPostedFileBase ProfilePic)
        {
            Patient updateprofile = null;
            int Unorgid = 0;
            updateprofile = model.AdminEditMember(model);
            if (model.MemberId > 0)
            {
                updateprofile.DOB = DateTime.Now;
                updateprofile.ModifiedBy = base.loginUserModel.LoginId;
                updateprofile.ModifiedDate = DateTime.Now;
                if (ProfilePic != null)
                {
                    _loginHelper.UploadImage(model, ProfilePic);
                }
                UserService.UpdatePatient(updateprofile);

                if (model.MemberId > 0)
                {

                    if (!string.IsNullOrEmpty(model.ProfilePic))
                    {
                        var userid = UserService.Find(model.LoginId);
                        userid.Avatar = model.ProfilePic;
                        UserService.Update(userid);
                    }
                } //Add User Organization By dheeraj

                BindDropDown(model);

            }
            else
            {

                var userExists = UserService.CheckDuplicateUserName(model.UserName);
                if (userExists)
                {
                    ModelState.AddModelError("UserName", "Username already exists.");
                }
                var userEmailExists = UserService.CheckDuplicateMail(model.Email);
                if (userEmailExists)
                {
                    ModelState.AddModelError("Email", "User with this email id already exists.");
                    ViewBag.ErrorMessage = "This email address already exists.";
                    BindDropDown(model);
                    return View(model);
                }
                if (Convert.ToInt32(model.MedicareRefNumber) >= 20)
                {
                    ModelState.AddModelError("Number", "Medicare Refernce Number between 1 to 20.");
                    ViewBag.ErrorMessage = "Please Enter Medicare Refernce Number between 1 to 20.";
                    BindDropDown(model);
                    return View(model);
                }

                if (ProfilePic != null)
                {
                    _loginHelper.UploadImage(model, ProfilePic);
                }


                BindDropDown(model);
                Patient profile = null;
                model.RoleId = RoleType.Patient;
                profile = model.GetAdminPatient(model.UserorgId);
                var user = model.GetLoginAdmin(profile, model.Email);
                var Password = RandomPassword.Generate(6, 8);
                user.IsEmailVerified = true;
                user.IsActive = true;
                user.IsApproved = true;
                user.LoginPassword = Password;
                UserService.Add(user);

                //Add Multiple Organization And treating Doctor
                if (model.OrgClinicDoctorsIds != null)
                {
                    string[] id = model.OrgClinicDoctorsIds.Split(new[] { "$", "#" }, StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < id.Length; j = j + 2)
                    {
                        TreatingDoctors treatingDoctors = new TreatingDoctors();
                        int DoctorId = Convert.ToInt32(id[j + 1].Replace("DoctorId=", ""));
                        string ClinicDocIds = id[j].Replace("ClinicId=", "");
                        string[] arrClinicId = ClinicDocIds.Split(',');

                        for (int i = 0; i < arrClinicId.Length; i++)
                        {
                            treatingDoctors.DoctorId = DoctorId;
                            treatingDoctors.PatientId = user.Member.MemberId;
                            //  treatingDoctors.ClinicId = Convert.ToInt32(arrClinicId[i]);
                            treatingDoctors.CreatedBy = base.loginUserModel.LoginId;
                            _patientServices.AddTreating(treatingDoctors);
                        }
                    }
                }

                var fileUrl = Server.MapPath("~/EmailTemplates/PatientRegistration.html");
                string html = System.IO.File.ReadAllText(fileUrl);
                html = html.Replace("@@Password", Password);
                EmailService objEmail = new EmailService();

                Task taskA = new Task(() => objEmail.SendEmail("User Registration", html, model.Email, "noreply@myonlineclinic.com.au"));
                // Start the task.
                taskA.Start();
            }


            return RedirectToAction("FindPatient");
        }
        public ActionResult FindPatient(int? id)
        {

            if (id.HasValue) { ViewBag.message = true; }
            Session["VoucherID"] = id;
            var activatepatient = userService.GetPatientList();
            for (int i = 0; i < activatepatient.Count; i++)
            {
                activatepatient[i].IsApproved = userService.Find(activatepatient[i].LoginId).IsApproved;
            }
            var patient = activatepatient.Where(x => x.IsApproved == true).Select(x =>
                {
                    return new RegisterViewModel()
                {
                    FullName = StringCipher.Decrypt(x.FirstName) + " " + StringCipher.Decrypt(x.SurName),
                    MemberId = x.MemberId,
                    Email = StringCipher.Decrypt(x.Email),
                    IsActive = x.IsActive,
                    OrgId = x.OrgId,

                    LoginId = x.LoginId,

                    VoucherId = id,

                    LastUpdateDateInString = x.ModifiedDate == null ? "" : Common.UtcToLocal(Convert.ToDateTime(x.ModifiedDate), x.TimeZoneString).ToString("dd/MM/yyyy h:mm tt"),
                    TimeZoneDisplayName = Common.GetTimeZoneStandardIdAndDisplayName(x.TimeZoneString, true),
                    ProfilePic = UserService.Find(x.LoginId).Avatar
                };
                }).OrderByDescending(y => y.ModyfiedDate).ToList();


            for (int i = 0; i < patient.Count; i++)
            {
                var organizationName = string.Empty;
                try
                {
                    if (_organiztionService.GetOrganizationById(patient[i].OrgId) != null)
                    {
                        organizationName = StringCipher.Decrypt(_organiztionService.GetOrganizationById(patient[i].OrgId).OrganizationName);
                    }
                }
                catch { }

                if (!string.IsNullOrEmpty(organizationName))
                {
                    patient[i].organizationName = organizationName;
                }
            }

            ViewBag.permissionmodulelist = _permissioninmoduleService.GetPermissionInModuleByUserId(base.loginUserModel.LoginId).Where(x => x.ModuleId == 9 && x.IsActive == true && x.IsDeleted == false && x.IsApproved == true).ToList();
            return View(patient);
        }
        public ActionResult RecentPatient(int? id)
        {

            if (id.HasValue) { ViewBag.message = true; }
            Session["VoucherID"] = id;
            var activatepatient = userService.GetPatientList();
            for (int i = 0; i < activatepatient.Count; i++)
            {
                activatepatient[i].IsApproved = userService.Find(activatepatient[i].LoginId).IsApproved;
            }
            var patient = activatepatient.Where(x => x.IsApproved == false).Select(x =>
            {
                return new RegisterViewModel()
                {
                    FullName = StringCipher.Decrypt(x.FirstName) + " " + StringCipher.Decrypt(x.SurName),
                    MemberId = x.MemberId,
                    Email = StringCipher.Decrypt(x.Email),
                    IsActive = x.IsActive,
                    OrgId = x.OrgId,
                    VoucherId = id,
                    LastUpdateDateInString = x.ModifiedDate == null ? "" : Common.UtcToLocal(Convert.ToDateTime(x.ModifiedDate), x.TimeZoneString).ToString("dd/MM/yyyy h:mm tt"),
                    TimeZoneDisplayName = Common.GetTimeZoneStandardIdAndDisplayName(x.TimeZoneString, true),
                    ProfilePic = UserService.Find(x.LoginId).Avatar
                };
            }).OrderByDescending(y => y.ModyfiedDate).ToList();
            for (int i = 0; i < patient.Count; i++)
            {
                var organizationName = string.Empty;
                try
                {
                    if (_organiztionService.GetOrganizationById(patient[i].OrgId) != null)
                    {
                        organizationName = StringCipher.Decrypt(_organiztionService.GetOrganizationById(patient[i].OrgId).OrganizationName);
                    }
                }
                catch { }

                if (!string.IsNullOrEmpty(organizationName))
                {
                    patient[i].organizationName = organizationName;
                }
            }

            ViewBag.permissionmodulelist = _permissioninmoduleService.GetPermissionInModuleByUserId(base.loginUserModel.LoginId).Where(x => x.ModuleId == 9 && x.IsActive == true && x.IsDeleted == false && x.IsApproved == true).ToList();
            return View(patient);
        }
        [HttpPost]
        public ActionResult RecentPatient(int? id, SearchParametersViewModel searchModel)
        {
            if (id.HasValue) { ViewBag.message = true; }
            Session["VoucherID"] = id;
            var query = userService.GetPatientList();
            //var query = _organiztionService.GetOrganizationList();
            for (int i = 0; i < query.Count; i++)
            {
                query[i].IsApproved = userService.Find(query[i].LoginId).IsApproved;
            }

            var patient = query.Where(x => x.IsApproved == false).Select(x =>
            {
                return new RegisterViewModel()
                {
                    FullName = StringCipher.Decrypt(x.FirstName + " " + x.SurName),
                    MemberId = x.MemberId,
                    Email = StringCipher.Decrypt(x.Email),
                    IsActive = x.IsActive,
                    OrgId = x.OrgId,
                    VoucherId = id,
                    LastUpdateDateInString = x.ModifiedDate == null ? "" : Common.UtcToLocal(Convert.ToDateTime(x.ModifiedDate), x.TimeZoneString).ToString("dd/MM/yyyy h:mm tt"),
                    TimeZoneDisplayName = Common.GetTimeZoneStandardIdAndDisplayName(x.TimeZoneString, true),
                    ProfilePic = UserService.Find(x.LoginId).Avatar
                };
            }).OrderByDescending(y => y.ModyfiedDate).ToList();
            for (int i = 0; i < patient.Count; i++)
            {
                var organizationName = string.Empty;
                try
                {
                    if (_organiztionService.GetOrganizationById(patient[i].OrgId) != null)
                    {
                        organizationName = StringCipher.Decrypt(_organiztionService.GetOrganizationById(patient[i].OrgId).OrganizationName);
                    }
                }
                catch { }

                if (!string.IsNullOrEmpty(organizationName))
                {
                    patient[i].organizationName = organizationName;
                }
            }

            ViewBag.permissionmodulelist = _permissioninmoduleService.GetPermissionInModuleByUserId(base.loginUserModel.LoginId).Where(x => x.ModuleId == 9 && x.IsActive == true && x.IsDeleted == false && x.IsApproved == true).ToList();
            return View(patient);

        }
        public SearchViewModel BindType(SearchViewModel model)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            List<SelectListItem> Prof = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = AppointmentType.VideoConsult.ToString(), Value = ((int)AppointmentType.VideoConsult).ToString() });
            items.Add(new SelectListItem { Text = AppointmentType.HomeVisit.ToString(), Value = ((int)AppointmentType.HomeVisit).ToString() });
            items.Add(new SelectListItem { Text = AppointmentType.ClinicVisit.ToString(), Value = ((int)AppointmentType.ClinicVisit).ToString() });
            Prof.Add(new SelectListItem { Text = Profession.Clinic.ToString(), Value = ((int)Profession.Clinic).ToString() });
            Prof.Add(new SelectListItem { Text = Profession.GP.ToString(), Value = ((int)Profession.GP).ToString() });
            Prof.Add(new SelectListItem { Text = Profession.Allied_Health.ToString(), Value = ((int)Profession.Allied_Health).ToString() });
            Prof.Add(new SelectListItem { Text = Profession.Specialist.ToString(), Value = ((int)Profession.Specialist).ToString() });
            model.AppointmentTypeList = items;
            model.ProfessionList = Prof;
            model.CountryList = _countryService.GetCountryList();
            model.CityList = model.MemberId > 0 ? _cityService.GetCityList() : new List<LookUpCity>();
            model.StateList = model.MemberId > 0 ? _stateService.GetStateList() : new List<LookUpState>();
            model.ProfessionTypes = _professiontypeservice.GetProfessionTypesList();
            model.ProfessionSubList = _professiontypeservice.GetProfessionSubTypesList();
            var ClinicDecrypt = _clinicService.GetClinicsListbyOrgid(Convert.ToInt32(TempData["OrgId"]), Convert.ToInt32(TempData["orgType"])).ToList();
            for (int i = 0; i < ClinicDecrypt.Count; i++)
            {
                ClinicDecrypt[i].ClinicName = StringCipher.Decrypt(ClinicDecrypt[i].ClinicName);
            }
            model.ClinicList = ClinicDecrypt;
            return model;

        }
        public RegisterViewModel BindDropDown(RegisterViewModel model)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            var genderlist = _genderService.GetGenderList().Where(t => t.Type == (int)RoleType.Patient).ToList();
            for (int i = 0; i < genderlist.Count; i++)
            {
                genderlist[i].GenderName = StringCipher.Decrypt(genderlist[i].GenderName);
            }
            model.GenderList = genderlist;

            // = _countryService.GetCountryList();
            var countrylist = _countryService.GetCountryList();
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
            //model.CityList = model.MemberId > 0 ? _cityService.GetCityList() : new List<LookUpCity>();
            //model.StateList = model.MemberId > 0 ? _stateService.GetStateList() : new List<LookUpState>();          
            var Statelist = _stateService.GetStateList();
            for (int i = 0; i < Statelist.Count; i++)
            {
                Statelist[i].StateName = StringCipher.Decrypt(Statelist[i].StateName);
            }
            model.StateList = model.MemberId > 0 ? Statelist : new List<LookUpState>();
            model.OrganizationList = _organiztionService.GetOrganizationList();
            for (int i = 0; i < model.OrganizationList.Count; i++)
            {
                model.OrganizationList[i].OrganizationName = StringCipher.Decrypt(model.OrganizationList[i].OrganizationName);
            }


            var titlelist = _TitleServices.GetTitleList().Where(t => t.Type == (int)RoleType.Patient).ToList();
            for (int i = 0; i < titlelist.Count; i++)
            {
                titlelist[i].TitleName = StringCipher.Decrypt(titlelist[i].TitleName);
            }
            model.TitleList = titlelist;

            items.Add(new SelectListItem { Text = DVACardcolor.Gold.ToString(), Value = ((int)DVACardcolor.Gold).ToString() });
            items.Add(new SelectListItem { Text = DVACardcolor.White.ToString(), Value = ((int)DVACardcolor.White).ToString() });
            items.Add(new SelectListItem { Text = DVACardcolor.Orange.ToString(), Value = ((int)DVACardcolor.Orange).ToString() });
            model.dvacolor = items;
            ReadOnlyCollection<TimeZoneInfo> tz;
            tz = TimeZoneInfo.GetSystemTimeZones();
            var timeZoneList = tz.Select(p => new SelectListItem { Value = p.Id, Text = p.DisplayName }).ToList();

            ViewBag.Timezone = timeZoneList;
            var clinic = entitiesDb.Clinics.Select(p => new SelectListItem { Value = p.ClinicName, Text = p.ClinicName }).ToList();
            ViewBag.Clinic = clinic;

            model.OrganizationCountryList = _countryService.GetCountryList();
            model.OrganizationCityList = model.MemberId > 0 ? Citylist : new List<LookUpCity>();
            // model.OrganizationCityList = model.MemberId > 0 ? _cityService.GetCityList() : new List<LookUpCity>();
            //model.OrganizationStateList = model.MemberId > 0 ? _stateService.GetStateList() : new List<LookUpState>();
            model.OrganizationStateList = model.MemberId > 0 ? Statelist : new List<LookUpState>();
            model.ClinicCountryList = countrylist;
            model.ClinicCityList = model.MemberId > 0 ? Citylist : new List<LookUpCity>();
            // model.ClinicCityList = model.MemberId > 0 ? _cityService.GetCityList() : new List<LookUpCity>();
            model.ClinicStateList = model.MemberId > 0 ? Statelist : new List<LookUpState>();
            return model;
        }
        public ActionResult GetClinicByID(int Id)
        {
            var Clinic = _clinicService.GetClinicInOrganization(Id).ToList();

            return Json(Clinic, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Details(int? id)
        {
            #region Detail
            var model = UserService.GetPatientById(Convert.ToInt32(id)).Select(x => new RegisterViewModel
            {
                FirstName = StringCipher.Decrypt(x.FirstName),
                SurName = StringCipher.Decrypt(x.SurName),
                State = x.State,
                CountryId = x.CountryId,
                City = x.CountryId,
                Address1 = StringCipher.Decrypt(x.Address1),
                MiddleName = StringCipher.Decrypt(x.MiddleName),
                Address2 = StringCipher.Decrypt(x.Address2),
                PhoneNumber = StringCipher.Decrypt(x.PhoneNumber),
                MobileNumber = StringCipher.Decrypt(x.MobileNumber),
                FaxNumber = StringCipher.Decrypt(x.FaxNumber),
                Email = StringCipher.Decrypt(x.Email),
                DOB = x.DOB,
                PostCode = StringCipher.Decrypt(x.PostCode),
                OrgId = x.OrgId,
                MedicareNumber = StringCipher.Decrypt(x.MedicareNumber),
                MedicareRefNumber = StringCipher.Decrypt(x.MedicareRefNo),
                MedicareExpire = x.Expiry,
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
                PharmacyName = StringCipher.Decrypt(x.PharmacyName),
                PharmacyAddress1 = StringCipher.Decrypt(x.PharmacyAddress1),
                PharmacyAddress2 = StringCipher.Decrypt(x.PharmacyAddress2),
                PharmacySuburb = StringCipher.Decrypt(x.PharmacySuburb),
                PharmacyCity = x.PharmacyCity,
                PharmacyCountryId = x.PharmacyCountryId,
                PharmacyFaxNumber = StringCipher.Decrypt(x.PharmacyFaxNumber),
                PharmacyMobileNumber = StringCipher.Decrypt(x.PharmacyMobileNumber),
                PharmacyPhoneNumber = StringCipher.Decrypt(x.PharmacyPhoneNumber),
                PharmacyPostCode = StringCipher.Decrypt(x.PharmacyPostCode),
                PharmacyState = x.PharmacyState,
                GernealNotes = StringCipher.Decrypt(x.GernealNotes),
                TreatingDoctors = StringCipher.Decrypt(x.TreatingDoctors),
                LoginId = x.LoginId,
                GenderId = x.Gender,
                MemberId = x.MemberId,
                DVAColorName = x.DavColorCard == (Int32)DVACardcolor.Gold ? "Gold" : x.DavColorCard == (Int32)DVACardcolor.Orange ? "Orange" : x.DavColorCard == (Int32)DVACardcolor.White ? "White" : ""

            }).ToList().FirstOrDefault();


            if (model.State != null)
            {
                model.StateName = StringCipher.Decrypt(_stateService.GetStateById(model.State).StateName);
            }
            if (model.UsualGpState != null)
            {
                model.usualGpStateName = StringCipher.Decrypt(_stateService.GetStateById(model.UsualGpState).StateName);
            }
            if (model.CountryId != null)
            {
                model.CountryName = StringCipher.Decrypt(_countryService.GetCountryById(model.CountryId).CountryName);
            }
            if (model.City != null)
            {
                model.CityName = StringCipher.Decrypt(_cityService.GetCityById(model.City).LookUpCityName);
            }
            if (model.OrgId != 0)
            {

                model.organizationName = StringCipher.Decrypt(_organiztionService.GetOrganizationById(model.OrgId).OrganizationName);

            }
            if (model.PharmacyState != null)
            {

                model.PharmacyStateName = StringCipher.Decrypt(_stateService.GetStateById(model.PharmacyState).StateName);
            }

            if (model.PharmacyCountryId != null)
            {

                model.PharmacyCountryName = StringCipher.Decrypt(_countryService.GetCountryById(model.PharmacyCountryId).CountryName);
            }
            if (model.PharmacyCity != null)
            {

                model.PharmacyCityName = StringCipher.Decrypt(_cityService.GetCityById(model.PharmacyCity).LookUpCityName);
            }
            if (model.ProfilePic != null)
            {
                model.ProfilePic = UserService.Find(model.LoginId).Avatar;
            }
            if (model.GenderId != null)
            {
                model.Gender = StringCipher.Decrypt(_genderService.GetGenderById(model.GenderId).GenderName);
            }
            if (model.UsualGpCountry != null)
            {
                model.UsualGpCountryName = StringCipher.Decrypt(_countryService.GetCountryById(model.UsualGpCountry).CountryName);
            }
            model.ProfilePic = UserService.Find(model.LoginId).Avatar;

            #endregion

            #region Myhistory
            var MemberId = 1;
            var LoginId = base.loginUserModel.LoginId;
            var Allergieslist = _patientHistoryService.GetPatientAllergiesbyPID(LoginId).ToList();
            for (int i = 0; i < Allergieslist.Count; i++)
            {
                Allergieslist[i].AllergyName = StringCipher.Decrypt(Allergieslist[i].AllergyName);
                Allergieslist[i].WhatHappens = StringCipher.Decrypt(Allergieslist[i].WhatHappens);
            }
            var MedicationHistorylist = _patientHistoryService.GetPatientMedicationbyPID(LoginId).ToList();
            for (int i = 0; i < MedicationHistorylist.Count; i++)
            {
                MedicationHistorylist[i].Name = StringCipher.Decrypt(MedicationHistorylist[i].Name);
            }
            var PastMedicallist = _patientHistoryService.GetPatientPastMedicalbyPID(LoginId).ToList();
            for (int i = 0; i < PastMedicallist.Count; i++)
            {
                PastMedicallist[i].ConditionName = StringCipher.Decrypt(PastMedicallist[i].ConditionName);
            }
            var Smokinglist = _patientHistoryService.GetPatientSmokingbyPID(LoginId).ToList();
            for (int i = 0; i < Smokinglist.Count; i++)
            {
                Smokinglist[i].HowMany = StringCipher.Decrypt(Smokinglist[i].HowMany);
            }
            var Alcohollist = _patientHistoryService.GetPatientAlcohalbyPID(LoginId).ToList();
            for (int i = 0; i < Alcohollist.Count; i++)
            {
                Alcohollist[i].InWeek = StringCipher.Decrypt(Alcohollist[i].InWeek);
                Alcohollist[i].InDay = StringCipher.Decrypt(Alcohollist[i].InDay);
            }
            var FamilyHistorylist = _patientHistoryService.GetPatientFamilybyPID(LoginId).ToList();
            for (int i = 0; i < FamilyHistorylist.Count; i++)
            {
                FamilyHistorylist[i].ConditionName = StringCipher.Decrypt(FamilyHistorylist[i].ConditionName);
                FamilyHistorylist[i].FamilyMember = StringCipher.Decrypt(FamilyHistorylist[i].FamilyMember);
            }
            var Employmentlist = _patientHistoryService.GetPatientEmploymentbyPID(LoginId).ToList();
            for (int i = 0; i < Employmentlist.Count; i++)
            {
                Employmentlist[i].Employment = StringCipher.Decrypt(Employmentlist[i].Employment);
            }
            var OtherMedicalRecords = _patientHistoryService.GetPatientMedicalRecordbyPID(LoginId).ToList();
            for (int i = 0; i < OtherMedicalRecords.Count; i++)
            {
                OtherMedicalRecords[i].FileName = StringCipher.Decrypt(OtherMedicalRecords[i].FileName);
                OtherMedicalRecords[i].Subject = StringCipher.Decrypt(OtherMedicalRecords[i].Subject);
            }
            //model.patientmodel pmode = new PatientHistoryViewModel();
            if (Allergieslist.Count == 0)
            {
                Allergieslist = new List<PatientAllergies>();
            }
            model.patientmodel = new PatientHistoryViewModel();
            model.patientmodel.MemberId = (int)id;
            model.patientmodel.AllergiesList = Allergieslist;
            model.patientmodel.MedicationHistoryList = MedicationHistorylist;
            model.patientmodel.PastMedicalList = PastMedicallist;
            model.patientmodel.SmokingList = Smokinglist;
            model.patientmodel.AlcoholList = Alcohollist;
            model.patientmodel.FamilyHistoryList = FamilyHistorylist;
            model.patientmodel.EmploymentList = Employmentlist;
            model.patientmodel.OtherMedicalRecordsList = OtherMedicalRecords;
            #endregion
            return View(model);
        }
        public ActionResult Delete(FormCollection collection)
        {
            var ids = collection["serviceIds"];
            UserService.Delete(ids);
            return RedirectToAction("Findpatient");
        }
        public ActionResult Acitvate(FormCollection collection)
        {
            var ids = collection["ActivateIds"];
            UserService.Activate(ids);
            return RedirectToAction("Findpatient");
        }
        public ActionResult AssignVoucher(int id)
        {
            return View();
        }
        public ActionResult GetProfessionSubTypes(int Id)
        {
            var ProfessionSubType = _professiontypeservice.GetProfessionSubTypesList().Where(t => t.Id == Id).ToList();
            return Json(ProfessionSubType, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PayWithCreditCard(int? id, int patientId)
        {
            CreditCardViewModel model = new CreditCardViewModel();
            model.AppointmentId = Convert.ToInt32(id);
            model.PatientId = patientId;
            return View("Payment", model);
        }
        [HttpPost]
        public ActionResult PayWithCreditCard(CreditCardViewModel model, FormCollection collection)
        {
            var ids = collection["ddlPaidBy"];

            if (!string.IsNullOrEmpty(ids))
            {
                if (ids == "1") //Voucher
                {
                    var cModel = new MyOnlineClinic.Entity.Payment();
                    cModel.Amount = 0;
                    cModel.Status = 1;
                    cModel.Response = model.VoucherNumber;
                    cModel.PaymentDate = DateTime.UtcNow;
                    cModel.PaymentSource = 4;
                    cModel.LoginId = userService.GetPatById(model.PatientId).LoginId;
                    cModel.AppointmentId = model.AppointmentId;
                    _servicePayment.Add(cModel);
                }
                else if (ids == "2")//CreditCard
                {
                    var response = _paymentService.PayWithCreditCard(model);

                    if (response != null && response.Status == "approved")
                    {
                        var cModel = new MyOnlineClinic.Entity.Payment();
                        cModel.Amount = 12;
                        cModel.Status = 1;
                        cModel.Response = response.ResponseString;
                        cModel.PaymentDate = DateTime.UtcNow;
                        cModel.PaymentSource = 2;
                        cModel.LoginId = userService.GetPatById(model.PatientId).LoginId;
                        cModel.AppointmentId = model.AppointmentId;
                        _servicePayment.Add(cModel);
                    }
                }
            }

            return RedirectToAction("BookAppointment");
        }

        [HttpPost]
        public JsonResult doesUserNameExist(string Email)
        {
            var user = UserService.CheckDuplicateMail(Email);

            return Json(user == false);

        }

        public JsonResult GetOrganizationList(string query = "")
        {
            int orgid;
            query = query.Trim();
            var org = string.IsNullOrEmpty(query) ? _organiztionService.GetOrganizationList().Select(x =>
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
                    ClinicList = _clinicService.GetClinicInOrganization(x.OrganizationId).ToList(),
                    IsActive = x.IsActive,
                    // OrganizationTypeName = _organizationTypeService.GetOrganizarionTypesById(x.OrganizationType).OrganizationTypeName,
                }).ToList()
            : _organiztionService.GetOrganizationList().Where(x => x.OrganizationName.Contains(query)).Select(x =>
                 new OrganizationViewModel
                {
                    OrganizationName = StringCipher.Decrypt(x.OrganizationName),
                    OrganizationId = x.OrganizationId,
                    OrganizationType = x.OrganizationType,
                    LoginId = x.LoginId,
                    Address1 = StringCipher.Decrypt(x.Address1),
                    FaxNumber = StringCipher.Decrypt(x.FaxNumber),
                    PostCode = StringCipher.Decrypt(x.PostCode),
                    Address2 = StringCipher.Decrypt(x.Address2),
                    PhoneNumber = StringCipher.Decrypt(x.PhoneNumber),
                    OrganizationLogo = x.OrganizationLogo,
                    Country = x.Country,
                    State = x.State,
                    City = x.City,
                    ModifiedDate = x.ModifiedDate,
                    //ClinicList = _clinicService.GetClinicInOrganization(x.OrganizationId).ToList(),
                    IsActive = x.IsActive,

                }).ToList();

            return Json(org, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTreatingDoctors(string query = "", string Clinic = "")
        {
            IEnumerable<SqlParameter> parameters = null;
            var dt = Common.ExecuteStoredProcedure(new EntitiesDB(), "[dbo].[GetDoctorSearch]", parameters).Tables[0];
            var list = Common.DataTableToList<SearchViewModel>(dt);
            string[] id = Clinic.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            var clinicId = id.Select(x => Convert.ToInt32(x));
            for (int i = 0; i < clinicId.Count(); i++)
            {
                list = list.Where(x => Clinic.Contains(x.Clinicid.ToString())).ToList();
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetClinicListByOrganization(string query = "", string OrgId = "")
        {
            query = query.Trim();
            var clinic = string.IsNullOrEmpty(query) ? _clinicService.GetClinicInByOrg(OrgId).Select(x =>
                new ClinicModel
                {
                    ClinicName = StringCipher.Decrypt(x.ClinicName),
                    ClinicId = x.ClinicId,
                    LoginId = x.LoginId,
                    Address1 = StringCipher.Decrypt(x.Address1),
                    FaxNumber = StringCipher.Decrypt(x.FaxNumber),
                    PostCode = StringCipher.Decrypt(x.PostCode),
                    Address2 = StringCipher.Decrypt(x.Address2),
                    PhoneNumber = StringCipher.Decrypt(x.PhoneNumber),
                    Country = x.Country,
                    State = x.State,
                    City = x.City,
                    IsActive = x.IsActive,
                    OrganizationId = x.organization.OrganizationId,

                }).ToList()
            : _clinicService.GetClinicInByOrg(OrgId).Where(x => x.ClinicName.Contains(query)).Select(x =>
                new ClinicModel
                {
                    ClinicName = StringCipher.Decrypt(x.ClinicName),
                    ClinicId = x.ClinicId,
                    LoginId = x.LoginId,
                    Address1 = StringCipher.Decrypt(x.Address1),
                    FaxNumber = StringCipher.Decrypt(x.FaxNumber),
                    PostCode = StringCipher.Decrypt(x.PostCode),
                    Address2 = StringCipher.Decrypt(x.Address2),
                    PhoneNumber = StringCipher.Decrypt(x.PhoneNumber),
                    Country = x.Country,
                    State = x.State,
                    City = x.City,
                    IsActive = x.IsActive,
                    OrganizationId = x.organization.OrganizationId,

                }).ToList();


            return Json(clinic, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult HomeVisit(int? id, string name)
        {
            if (!id.HasValue || string.IsNullOrEmpty(name) || Convert.ToInt32(name) != (int)AppointmentType.HomeVisit)
            {
                return RedirectToAction("FindPatient");
            }
            var clinicModel = new ClinicModel();
            //clinicModel.PatientId = Convert.ToInt32(id);
            var pModel = UserService.GetPatientById(Convert.ToInt32(id)).Select(x =>
                new HomeVisitModel
                {
                    FirstName = StringCipher.Decrypt(x.FirstName),
                    Surname = StringCipher.Decrypt(x.SurName),
                    Address1 = StringCipher.Decrypt(x.Address1),
                    Address2 = StringCipher.Decrypt(x.Address2),
                    Country = x.CountryId,
                    State = x.State,
                    City = x.City,
                    PostCode = StringCipher.Decrypt(x.PostCode),
                }).FirstOrDefault();
            if (pModel != null)
            {
                pModel.CountryName = StringCipher.Decrypt(_countryService.GetCountryById(pModel.Country).CountryName);
                pModel.StateList = _stateService.GetStateList().ToList();
                pModel.CityList = _cityService.GetCityList().ToList();
                var Citylist = _cityService.GetCityList();
                for (int i = 0; i < Citylist.Count; i++)
                {
                    Citylist[i].LookUpCityName = StringCipher.Decrypt(Citylist[i].LookUpCityName);
                }
                pModel.CityList = Citylist;
                var Statelist = _stateService.GetStateList();
                for (int i = 0; i < Statelist.Count; i++)
                {
                    Statelist[i].StateName = StringCipher.Decrypt(Statelist[i].StateName);
                }
                pModel.StateList = Statelist;
                pModel.PatientId = Convert.ToInt32(id);
            }
            return View(pModel);
        }

        [HttpPost]
        public ActionResult HomeVisit(FormCollection collection, HomeVisitModel model)
        {
            //HomeVisitAppointment nModel = new HomeVisitAppointment();
            Appointment cModel = new Appointment();
            string fromtime = collection.Get("FromHour") + ":" + collection.Get("FromMin") + " " + collection.Get("FromAMPM");
            //DateTime dtFromdate = DateTime.ParseExact(collection.Get("AppointmentDateLocal"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime dtFromdate = DateTime.ParseExact(collection.Get("AppointmentDateLocal"), "yyyy-dd-MM", CultureInfo.InvariantCulture);
            string fromtimeL = dtFromdate.ToString("dd/MM/yyyy").ToString() + " " + fromtime;
            var appointmentDateTime = DateTime.ParseExact(fromtimeL, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);
            var city = collection.Get("hidCityName");
            var state = collection.Get("hidStateName");
            var postcode = collection.Get("hidPostCode");
            var latitude = collection.Get("hidlatitude");
            var longitude = collection.Get("hidlongitude");
            var location = collection.Get("txtLocation");
            var AppointmentExisit = appointmentService.CheckAppointment(Convert.ToDateTime(fromtimeL));
            if (AppointmentExisit)
            {
                model.Confirm = "Exist";
                return View(model);
            }
            //cModel.DoctorLoginId = Guid.Parse("0");
            var patientResult = UserService.GetPatientById(model.PatientId);

            if (patientResult != null)
            {
                cModel.PatientLoginId = patientResult.FirstOrDefault().LoginId;
            }
            cModel.AppointmentDateLocal = appointmentDateTime;
            cModel.AppointmentStatus = (int)AppointmentStatus.New;
            cModel.AppointmentDateUtc = DateTime.Now;
            cModel.AppointmentType = (int)AppointmentType.HomeVisit;
            appointmentService.Add(cModel);

            HomeVisitAppointment nModel = new HomeVisitAppointment();
            nModel.AppointmentId = cModel.AppointmentId;
            nModel.AppointmentDateLocal = appointmentDateTime;
            nModel.AppointmentDateUtc = DateTime.Now;
            nModel.PatientLoginId = patientResult.FirstOrDefault().LoginId;
            //nModel.CityId = Convert.ToInt32(model.City);
            //nModel.StateId = Convert.ToInt32(model.State);
            nModel.location = location;
            nModel.CityId = _cityService.GetCityList().Where(x => StringCipher.Decrypt(x.LookUpCityName) == city).FirstOrDefault().LookUpCityId;
            nModel.StateId = _stateService.GetStateList().Where(x => StringCipher.Decrypt(x.StateName) == state).FirstOrDefault().Id;
            nModel.PostCode = postcode;
            nModel.latitude = latitude;
            nModel.longitude = longitude;
            nModel.SeverityOfProblem = collection.Get("ddlSeverity");
            nModel.TriageCategory = collection.Get("ddlTriageCategory");
            nModel.ReasonForContact = model.Problem;
            nModel.RegularGP = collection.Get("ddlRegularGP");
            nModel.AppointmentDateUtc = DateTime.Now;
            _homeVisitAppointmentService.Add(nModel);

            return RedirectToAction("Findpatient");
        }

        [HttpGet]
        public ActionResult ClinicVisit(int? id, string name)
        {
            if (!id.HasValue || string.IsNullOrEmpty(name) || Convert.ToInt32(name) != (int)AppointmentType.ClinicVisit)
            {
                return RedirectToAction("FindPatient");
            }
            var clinicModel = new ClinicModel();
            clinicModel.ProfessionTypesList = _professiontypeservice.GetProfessionTypesList();
            clinicModel.ProfessionSubList = new List<ProfessionSubType>();
            clinicModel.PatientId = Convert.ToInt32(id);
            clinicModel.ClinicId = base.loginUserModel.ClinicId;
            return View(clinicModel);
        }

        [HttpPost]
        public ActionResult ClinicVisit(ClinicModel clinicModel)
        {
            Appointment cModel = new Appointment();
            var appointmentDateTime = DateTime.ParseExact(clinicModel.AppointmentDateTime, "dd/MM/yyyy h:mm tt", CultureInfo.InvariantCulture);
            var AppointmentExisit = appointmentService.CheckAppointment(appointmentDateTime);

            if (AppointmentExisit)
            {
                ModelState.AddModelError("Error", "Appointment already taken for this datetime");
                return View(clinicModel);
            }

            if (clinicModel.DoctorId > 0)
            {
                var doctorResult = doctorService.GetDoctorById(clinicModel.DoctorId).FirstOrDefault();
                if (doctorResult != null)
                {
                    cModel.DoctorLoginId = doctorResult.LoginId;
                }

                var patientResult = UserService.GetPatientById(clinicModel.PatientId);

                if (patientResult != null)
                {
                    cModel.PatientLoginId = patientResult.FirstOrDefault().LoginId;
                }
                cModel.AppointmentDateLocal = appointmentDateTime;
                cModel.AppointmentStatus = (int)AppointmentStatus.Pending;
                cModel.AppointmentDateUtc = DateTime.Now;
                cModel.AppointmentType = (int)AppointmentType.ClinicVisit;
                appointmentService.Add(cModel);
            }
            return RedirectToAction("PayWithCreditCard", new
            {
                id = cModel.AppointmentId,
                patientId = clinicModel.PatientId
            });
        }

        [HttpGet]
        public JsonResult GetOrganizationListForAppointment(string query = "")
        {
            query = query.Trim();
            List<Organization> org = string.IsNullOrEmpty(query) ? _organiztionService.GetOrganizationList() : _organiztionService.GetOrganizationList().Where(x => x.OrganizationName.Contains(query)).ToList();
            for (int i = 0; i < org.Count; i++)
            {
                org[i].OrganizationName = StringCipher.Decrypt(org[i].OrganizationName);
            }
            return Json(org, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetClinicList(int? id, string query = "")
        {
            query = query.Trim();
            List<MyOnlineClinic.Entity.Clinic> clinicList = _clinicService.GetClinicsListbyOrgid(Convert.ToInt32(id), 0, query);
            for (int i = 0; i < clinicList.Count; i++)
            {
                clinicList[i].ClinicName = StringCipher.Decrypt(clinicList[i].ClinicName);
            }
            return Json(clinicList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDoctorList(int? clinicid)
        {
            List<Entity.Doctor> listDoctor = new List<Entity.Doctor>();
            var doctorIdList = _clinicService.GetDoctorListByClinicId(Convert.ToInt32(clinicid), (int)RoleType.Doctor);

            for (int i = 0; i < doctorIdList.Count; i++)
            {
                var doctor = doctorService.GetDoctorById(doctorIdList[i].UserId).FirstOrDefault();
                listDoctor.Add(doctor);
            }

            return Json(listDoctor, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAvabilityChart(string doctorIds, string searchDate)
        {
            var fromDate = string.Empty;
            var toDate = string.Empty;

            if (searchDate.Contains("¶"))
            {
                string[] arrDates = searchDate.Split('¶');
                fromDate = arrDates[0];
                toDate = arrDates[arrDates.Length - 1];
            }

            Dictionary<string, object> dicMain = new Dictionary<string, object>();
            List<List<Dictionary<string, object>>> listDoctor = new List<List<Dictionary<string, object>>>();
            List<Dictionary<string, object>> timeList = new List<Dictionary<string, object>>();
            var model = doctorService.GetDoctorAvabilityList(doctorIds, searchDate, fromDate, toDate);

            if (model != null)
            {
                var arrDoctorId = doctorIds.Split(',');

                for (int z = 0; z < arrDoctorId.Length; z++)
                {
                    var doctorList = model.Where(x => x.DoctorAvailabilitys.DoctorID == Convert.ToInt32(arrDoctorId[z])).ToList();

                    for (int i = 0; i < doctorList.Count; i++)
                    {
                        var startDate = doctorList[i].FromDateLocal;
                        var endDate = doctorList[i].ToDateLocal;

                        var dateDiffence = endDate.Subtract(startDate);
                        var endTime = startDate.AddHours(dateDiffence.Hours).AddMinutes(dateDiffence.Minutes);

                        List<string> time = new List<string>();
                        while (startDate <= endTime)
                        {
                            time.Add(startDate.ToString("h:mm tt"));
                            startDate = startDate.AddMinutes(15);
                        }

                        Dictionary<string, object> doctor = new Dictionary<string, object>();
                        doctor.Add("DoctorName", doctorList[i].DoctorAvailabilitys.Doctors.FirstName);
                        doctor.Add("DoctorId", doctorList[i].DoctorAvailabilitys.DoctorID);
                        doctor.Add("Date", doctorList[i].FromDateLocal.ToString("dd/MM/yyyy"));
                        doctor.Add("time", time);
                        timeList.Add(doctor);
                    }
                    listDoctor.Add(timeList);
                    timeList = new List<Dictionary<string, object>>();
                }
            }
            dicMain.Add("data", listDoctor);

            return Json(dicMain, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProfessionSubCategory(int? id)
        {
            return Json(_professiontypeservice.GetProfessionTypesListById(Convert.ToInt32(id)), JsonRequestBehavior.AllowGet);
        }

        public JsonResult FutureConsult(int id, Guid loginid)
        {
            var commonUtilities = new CommonUtilities();

            var FutureConsult = appointmentService.GetAppointmentList().Select(x => new AppointmentViewModel
            {
                AppointmentDateLocalText = x.AppointmentDateLocal.ToString("dd/MM/yyyy h:mm tt"),
                AppointmentId = x.AppointmentId,
                AppointmentTypeText = (x.AppointmentType == (int)AppointmentType.VideoConsult ?
                                      commonUtilities.GetDisplayName(AppointmentType.VideoConsult) :
                                      x.AppointmentType == (int)AppointmentType.ClinicVisit ?
                                      commonUtilities.GetDisplayName(AppointmentType.ClinicVisit) :
                                      commonUtilities.GetDisplayName(AppointmentType.HomeVisit)),
                PatientName = StringCipher.Decrypt(userService.Find(loginid).Member.FirstName) + " " + StringCipher.Decrypt(userService.Find(loginid).Member.SurName),
                AppointmentDateUtc = x.AppointmentDateUtc,
                AppointmentStatus = x.AppointmentStatus,
                Address1 = StringCipher.Decrypt(userService.Find(loginid).Member.Address1),
                Address2 = StringCipher.Decrypt(userService.Find(loginid).Member.Address2),
                Country = userService.Find(loginid).Member.CountryId,
                CountryName = StringCipher.Decrypt(_countryService.GetCountryById(userService.Find(loginid).Member.CountryId).CountryName),
                AppointmentStatusName = (x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.Pending ?
                                                          commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.Pending) :
                                                          x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.Approved ?
                                                          commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.Approved) :
                                                          commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.Completed)),
                MemberId = userService.Find(loginid).Member.MemberId,
                PatientLoginId = x.PatientLoginId,
                AppointmentType = x.AppointmentType


            }).Where(x => x.PatientLoginId == loginid && x.AppointmentStatus == (int)AppointmentStatus.Approved && x.AppointmentType == id).ToList();
            //Where(x => x.PatientLoginId == loginid && x.AppointmentStatus == (int)AppointmentStatus.Approved && x.AppointmentType == id).ToList();
            return Json(FutureConsult, JsonRequestBehavior.AllowGet);

        }

        public JsonResult PastConsult(int id, Guid loginid)
        {
            var commonUtilities = new CommonUtilities();

            var PastConsult = appointmentService.GetAppointmentList().Select(x => new AppointmentViewModel
            {
                AppointmentDateLocalText = x.AppointmentDateLocal.ToString("dd/MM/yyyy h:mm tt"),
                AppointmentId = x.AppointmentId,
                AppointmentTypeText = (x.AppointmentType == (int)AppointmentType.VideoConsult ?
                                      commonUtilities.GetDisplayName(AppointmentType.VideoConsult) :
                                      x.AppointmentType == (int)AppointmentType.ClinicVisit ?
                                      commonUtilities.GetDisplayName(AppointmentType.ClinicVisit) :
                                      commonUtilities.GetDisplayName(AppointmentType.HomeVisit)),
                PatientName = StringCipher.Decrypt(userService.Find(loginid).Member.FirstName) + " " + StringCipher.Decrypt(userService.Find(loginid).Member.SurName),
                AppointmentDateUtc = x.AppointmentDateUtc,
                AppointmentStatus = x.AppointmentStatus,
                Address1 = StringCipher.Decrypt(userService.Find(loginid).Member.Address1),
                Address2 = StringCipher.Decrypt(userService.Find(loginid).Member.Address2),
                Country = userService.Find(loginid).Member.CountryId,
                CountryName = StringCipher.Decrypt(_countryService.GetCountryById(userService.Find(loginid).Member.CountryId).CountryName),

                MemberId = userService.Find(loginid).Member.MemberId,
                PatientLoginId = x.PatientLoginId,
                AppointmentType = x.AppointmentType,
                AppointmentStatusName = x.AppointmentStatus == (int)AppointmentStatus.Pending ? commonUtilities.GetDisplayName(AppointmentStatus.Pending) :

          x.AppointmentStatus == (int)AppointmentStatus.Completed ?
          commonUtilities.GetDisplayName(AppointmentStatus.Completed) :

          x.AppointmentStatus == (int)AppointmentStatus.Approved ?
          commonUtilities.GetDisplayName(AppointmentStatus.Approved) :

          x.AppointmentStatus == (int)AppointmentStatus.CancelledByDoctor ?
          commonUtilities.GetDisplayName(AppointmentStatus.CancelledByDoctor) :

          x.AppointmentStatus == (int)AppointmentStatus.CancelledByPatient ?
          commonUtilities.GetDisplayName(AppointmentStatus.CancelledByPatient) :

          x.AppointmentStatus == (int)AppointmentStatus.Rejected ?
          commonUtilities.GetDisplayName(AppointmentStatus.Rejected) :

          x.AppointmentStatus == (int)AppointmentStatus.ApprovedNotCompleted ?
          commonUtilities.GetDisplayName(AppointmentStatus.ApprovedNotCompleted) :

          x.AppointmentStatus == (int)AppointmentStatus.CancelByAdmin ?
          commonUtilities.GetDisplayName(AppointmentStatus.CancelByAdmin) :

          x.AppointmentStatus == (int)AppointmentStatus.NotCompleted ?
          commonUtilities.GetDisplayName(AppointmentStatus.NotCompleted) :

          x.AppointmentStatus == (int)AppointmentStatus.NotRespondedByPatient ?
          commonUtilities.GetDisplayName(AppointmentStatus.NotRespondedByPatient) :

          x.AppointmentStatus == (int)AppointmentStatus.NotRespondedByDoctor ?
          commonUtilities.GetDisplayName(AppointmentStatus.NotRespondedByDoctor) :

          x.AppointmentStatus == (int)AppointmentStatus.New ?
          commonUtilities.GetDisplayName(AppointmentStatus.New) :

          x.AppointmentStatus == (int)AppointmentStatus.Assigned ?
          commonUtilities.GetDisplayName(AppointmentStatus.Assigned) :

          x.AppointmentStatus == (int)AppointmentStatus.InTransit ?
          commonUtilities.GetDisplayName(AppointmentStatus.InTransit) :

          x.AppointmentStatus == (int)AppointmentStatus.Seen ?
          commonUtilities.GetDisplayName(AppointmentStatus.Seen) :

          commonUtilities.GetDisplayName(AppointmentStatus.Waiting)



            }).Where(x => x.PatientLoginId == loginid && x.AppointmentStatus != (int)AppointmentStatus.Pending && x.AppointmentStatus != (int)AppointmentStatus.Approved && x.AppointmentType == id).ToList();
            //Where(x => x.PatientLoginId == loginid && x.AppointmentStatus == (int)AppointmentStatus.Approved && x.AppointmentType == id).ToList();
            return Json(PastConsult, JsonRequestBehavior.AllowGet);

        }

    }
}
