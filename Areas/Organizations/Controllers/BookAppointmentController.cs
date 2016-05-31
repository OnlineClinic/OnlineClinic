using MyOnlineClinic.Entity;
using MyOnlineClinic.RepositoryService;
using MyOnlineClinic.Web.Controllers;
using MyOnlineClinic.Web.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Mvc;
using MyOnlineClinic.Web.Helper;
using System.Data.SqlClient;
using MyOnlineClinic.Migrator;
using System.Globalization;

namespace MyOnlineClinic.Web.Areas.Organizations.Controllers
{
    public class BookAppointmentController : BaseController
    {
        private readonly IAppointmentService appointmentService = null;
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
        private readonly IUregisteredOrganizationService _unregisteredOrganizationService;
        IProfessionTypeService _professiontypeservice;
        IPaymentHelper _paymentService;
        private readonly IPatientService _patientServices;
        private readonly IUnregisteredClinicService _unregisteredClinicService;
        private readonly IPaymentService _servicePayment;
        private readonly IVoucherService _voucherService;

        public BookAppointmentController(IAppointmentService appointmentService, IDoctorService doctorService, IUserService userService, IOrganizationService organiztionService, IClinicService clinicService,
        ICountryService countryService, ICityService cityService, ILookUpModuleService moduleService,
          IPermissionsService permissionService, ILookUpUserRolesService roleService,
          IPermissionInModuleService permissioninmoduleService, IAdminUserService adminuserService, ITimeZoneService timezoneService, ITitleService TitleServices, IGenderService genderService, IStateService stateService, ILoginHelper loginHelper, IUregisteredOrganizationService unregisteredOrganizationService

          , IProfessionTypeService professiontypeservice, IPaymentHelper paymentService, IPatientService patientServices, IPaymentService servicePayment, IUnregisteredClinicService unregisteredClinicService, IVoucherService voucherService)
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
            _voucherService = voucherService;
        }

        public ActionResult Index(int id)
        {
            var Loginid = userService.GetPatientById(id).FirstOrDefault().LoginId;

            ReadOnlyCollection<TimeZoneInfo> tz;
            tz = TimeZoneInfo.GetSystemTimeZones();
            var timeZoneList = tz.Select(p => new SelectListItem { Value = p.Id, Text = p.DisplayName }).ToList();
            ViewBag.Timezone = timeZoneList;
            SearchViewModel model = new SearchViewModel();
            var Cmodel = UserService.GetLoginPatientById(Loginid).Select(x => new SearchViewModel
            {
                PatientId = x.LoginId,
                org = x.OrgId,
                Timezone = x.TimeZoneString,
                MemberId = x.MemberId,
                PatientLoginId = x.LoginId

            }).FirstOrDefault();
            var Omodel = _organiztionService.GetOrganizationById(Cmodel.org);
            var ClinicDecrypt = _clinicService.GetClinicsListbyOrgid(Cmodel.org, Omodel.OrganizationType).ToList();
            for (int i = 0; i < ClinicDecrypt.Count; i++)
            {
                ClinicDecrypt[i].ClinicName = StringCipher.Decrypt(ClinicDecrypt[i].ClinicName);
            }
            model.ClinicList = ClinicDecrypt;
            model.PatientLoginId = Cmodel.PatientLoginId;
            model.MemberId = Cmodel.MemberId;
            BindType(model);
            return View(model);
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

            return model;

        }
        public ActionResult GetProfessionSubTypes(int Id)
        {
            var ProfessionSubType = _professiontypeservice.GetProfessionSubTypesList().Where(t => t.Id == Id).ToList();
            return Json(ProfessionSubType, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Search(SearchViewModel model)
        {

            IEnumerable<SqlParameter> parameters = null;
            var dt = Common.ExecuteStoredProcedure(new EntitiesDB(), "[dbo].[GetDoctorSearch]", parameters).Tables[0];
            var list = Common.DataTableToList<SearchViewModel>(dt);
            if (model.AppointmentSelectionType == 4)
            {
                list = list.Where(x => x.Doctorclinicid == model.Clinicid).ToList();
            }
            //if (model.AppointmentDateLocal != null)
            //{
            //    list = list.Where(x => x.AvaliFromTime > model.AppointmentDateLocal && x.AvaliTotime < model.AppointmentDateLocal).ToList();
            //    list = list.Where(model.AppointmentDateLocal> x=>x.).ToList();

            //}
            if (model.DoctorFullName != null)
            {
                list = list.Where(x => x.firstname.Contains(model.firstname)).ToList();
            }
            if (model.countryid != null)
            {
                list = list.Where(x => x.countryid == model.countryid).ToList();
            }
            if (model.stateid != null)
            {
                list = list.Where(x => x.stateid == model.stateid).ToList();
            }
            if (model.cityid != null)
            {
                list = list.Where(x => x.cityid == model.cityid).ToList();
            }
            if (model.postcode != null)
            {
                list = list.Where(x => x.postcode.Contains(model.postcode)).ToList();
            }
            if (model.language != null)
            {
                list = list.Where(x => x.language.Contains(model.language)).ToList();
            }
            if (model.AppointmentSelectionType != 4)
            {
                list = list.Where(x => x.ProfessionType == model.AppointmentSelectionType).ToList();
            }
            //if (model.ProfessionType != null)
            //{
            //    list = list.Where(x => x.ProfessionType == model.ProfessionType).ToList();
            //}
            if (model.ProfessionSub > 0)
            {
                list = list.Where(x => x.ProfessionSub == model.ProfessionSub).ToList();
            }
            if (list != null)
            {
                var SearchDoctor = list.Select(x => new SearchViewModel
                {
                    firstname = StringCipher.Decrypt(x.firstname),
                    surname = StringCipher.Decrypt(x.surname),
                    ClinicName = StringCipher.Decrypt(x.ClinicName),
                    OrganizationName = StringCipher.Decrypt(x.OrganizationName),
                    DoctorProfile = StringCipher.Decrypt(x.DoctorProfile),
                    language = StringCipher.Decrypt(x.language),
                    DoctorLogin = x.DoctorLogin,
                    Avtar = userService.Find(x.DoctorLogin).Avatar == null ? " " : userService.Find(x.DoctorLogin).Avatar,
                    OrgId = x.OrgId,
                    Doctorclinicid = x.Doctorclinicid,
                    MemberId = doctorService.GetDoctorByLoginId(x.DoctorLogin).MemberId,

                }).ToList();

                return Json(SearchDoctor, JsonRequestBehavior.AllowGet);
            }

            return Json("", JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public ActionResult BookAppointment()
        {
            SearchViewModel model = new SearchViewModel();
            return View(model);
        }

        [HttpGet]
        public ActionResult Book(SearchViewModel model, Guid PatientIdLogin)
        {
            var PatientId = PatientIdLogin;
            var patient = userService.GetPatientByLoginId(PatientId);
            var datetime = string.Empty;
            if (model.hdnDocIdwhenAval > 0)
            {
                var doctorResult = doctorService.GetDoctorById(Convert.ToInt32(model.hdnDocIdwhenAval)).FirstOrDefault();
                Appointment cModel = new Appointment();
                datetime = model.hdnAppointmentDateForAval + " " + model.hdnAppointmentTimeForAval;
                var AppointmentExisit = appointmentService.CheckAppointment(Convert.ToDateTime(datetime));
                if (AppointmentExisit)
                {
                    model.Confirm = "Exist";
                    return View(model);
                }
                else if (datetime != null)
                {
                    cModel.PatientLoginId = PatientId;
                    cModel.DoctorLoginId = doctorResult.LoginId;
                    cModel.AppointmentDateLocal = Convert.ToDateTime(datetime);
                    cModel.AppointmentStatus = (int)AppointmentStatus.Pending;
                    TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(model.TimeZoneValue);
                    TimeSpan offset = tzi.GetUtcOffset(Convert.ToDateTime(datetime));
                    cModel.TimeZoneString = GetTimeZoneOffset(offset);
                    cModel.AppointmentDateUtc = LocalToUtc(Convert.ToDateTime(datetime), cModel.TimeZoneString);
                    cModel.AppointmentEndDateUtc = (LocalToUtc(Convert.ToDateTime(datetime), cModel.TimeZoneString)).AddMinutes(model.ConsultType);
                    cModel.AppointmentType = (int)AppointmentType.VideoConsult;
                    appointmentService.Add(cModel);
                    model.Confirm = "Book";
                    return Json(cModel, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var doctorResult = doctorService.GetDoctorById(Convert.ToInt32(model.HdnDoctorId)).FirstOrDefault();
                Appointment cModel = new Appointment();
                datetime = model.AppointmentDateLocalText + " " + model.FromTime + ":" + model.ToTime + "" + model.FromAMPM;
                var AppointmentExisit = appointmentService.CheckAppointment(Convert.ToDateTime(datetime));
                if (AppointmentExisit)
                {
                    model.Confirm = "Exist";
                    return View(model);
                }
                else if (datetime != null)
                {
                    cModel.PatientLoginId = PatientId;
                    cModel.DoctorLoginId = doctorResult.LoginId;
                    cModel.AppointmentDateLocal = Convert.ToDateTime(datetime);
                    cModel.AppointmentStatus = (int)AppointmentStatus.Pending;
                    TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(model.TimeZoneValue);
                    TimeSpan offset = tzi.GetUtcOffset(Convert.ToDateTime(datetime));
                    cModel.TimeZoneString = GetTimeZoneOffset(offset);
                    cModel.AppointmentDateUtc = LocalToUtc(Convert.ToDateTime(datetime), cModel.TimeZoneString);
                    cModel.AppointmentEndDateUtc = (LocalToUtc(Convert.ToDateTime(datetime), cModel.TimeZoneString)).AddMinutes(model.ConsultType);
                    cModel.ConsultTime = model.ConsultType;
                    cModel.AppointmentType = (int)AppointmentType.VideoConsult;
                    appointmentService.Add(cModel);
                    model.Confirm = "Book";
                    return Json(cModel, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CheckAvailability(int Id)
        {
            var test = doctorService.GetDoctorAvabilityById(Id);

            Dictionary<string, object> dicMain = new Dictionary<string, object>();
            List<List<Dictionary<string, object>>> listDoctor = new List<List<Dictionary<string, object>>>();
            List<Dictionary<string, object>> timeList = new List<Dictionary<string, object>>();

            if (test != null)
            {
                var doctorList = test.Where(x => x.DoctorAvailabilitys.DoctorID == Convert.ToInt32(Id)).ToList();

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
                    doctor.Add("Date", doctorList[i].FromDateLocal.ToString("dd-MM-yyyy"));
                    doctor.Add("ToDate", doctorList[i].ToDateLocal.ToString("dd/MM/yyyy"));
                    doctor.Add("time", time);
                    timeList.Add(doctor);
                }
                listDoctor.Add(timeList);
                timeList = new List<Dictionary<string, object>>();
            }

            dicMain.Add("data", listDoctor);

            return Json(dicMain, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PayWithCreditCard(int? id, int patientid)
        {
            var loginid = userService.GetPatById(patientid).LoginId;
            CreditCardViewModel model = new CreditCardViewModel();
            model.AppointmentId = Convert.ToInt32(id);
            model.PatientId = patientid;
            model.LoginId = loginid;
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
                    var Decryptvocucher = StringCipher.Encrypt(model.VoucherNumber);
                    var vocuehr = _voucherService.GetVoucherByNumber(Decryptvocucher);
                    if (vocuehr != null)
                    {
                        var assigen = _voucherService.GetVoucherListbyUserid().Where(x => x.VoucherID == vocuehr.VoucherID && x.UserID == model.LoginId && x.RoleType == (int)RoleType.Patient).Count();
                        if (assigen > 0)
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
                            return RedirectToAction("BookAppointment");
                        }
                        else
                        {
                            ModelState.AddModelError("Voucher", "Voucher Numbere Does not Exsist");
                            return View("Payment", model);
                        }

                    }
                    ModelState.AddModelError("Voucher", "Voucher Numbere Does not Exsist");
                    return View("Payment", model);
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
                        return RedirectToAction("BookAppointment");
                    }
                }
            }

            return View();
        }

        public string GetTimeZoneOffset(TimeSpan offset)
        {
            string Time = "";
            string dt = "";
            if (offset != null)
            {
                dt = offset.ToString();
                if (dt.Substring(0, 1) == "-")
                {
                    Time = dt.Substring(0, 5);
                }
                else
                {
                    Time = dt.Substring(0, 5);
                    Time = "+" + Time;
                }
            }
            return Time;
        }

        public DateTime LocalToUtc(DateTime dts, string timeZoneString)
        {
            string Time = "";
            string dt = "";
            DateTime utcLocal = new DateTime();
            if (timeZoneString != "")
            {
                if (timeZoneString.Substring(0, 1) == "+")
                {
                    Time = timeZoneString.Substring(1, 5);
                    IFormatProvider culture = new CultureInfo("en-US", true);
                    DateTime dateVal = DateTime.ParseExact(Time, "HH:mm", culture);
                    utcLocal = dts;
                    utcLocal = utcLocal.AddHours(-dateVal.Hour);
                    utcLocal = utcLocal.AddMinutes(-dateVal.Minute);

                }
                if (timeZoneString.Substring(0, 1) == "-")
                {
                    Time = timeZoneString.Substring(1, 6);
                    IFormatProvider culture = new CultureInfo("en-US", true);
                    DateTime dateVal = DateTime.ParseExact(Time, "HH:mm", culture);
                    utcLocal = dts;
                    utcLocal = utcLocal.AddHours(dateVal.Hour);
                    utcLocal = utcLocal.AddMinutes(dateVal.Minute);
                }
            }
            return utcLocal;
        }
    }
}