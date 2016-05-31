using MyOnlineClinic.Entity;
using MyOnlineClinic.RepositoryService;
using MyOnlineClinic.Web.Controllers;
using MyOnlineClinic.Web.Helper;
using MyOnlineClinic.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;

namespace MyOnlineClinic.Web.Areas.Organizations.Controllers
{
    public class AppointmentController : BaseController
    {
        private readonly IAppointmentService appointmentService;
        private readonly IUserService userService;
        private readonly IOrganizationService _organizationService;
        private readonly IDoctorService _doctorService;
        private readonly ICountryService _countrySerive;
        private readonly IAdminUserService _adminService;
        private readonly IHomeVisitAppointmentService _homeVisitAppointmentService;
        private readonly ICityService _cityService;
        private readonly IStateService _StateService;
        private readonly IClinicService _ClinicService;
        private readonly IGenderService _genderService;
        public AppointmentController(IAppointmentService appointmentService, IUserService userService, IOrganizationService organizationService, IDoctorService doctorService,
            ICountryService countrySerive, IAdminUserService adminService, IHomeVisitAppointmentService homeVisitAppointmentService, ICityService cityService, IStateService StateService, IGenderService genderService, IClinicService clinicService)
        {
            this.appointmentService = appointmentService;
            this.userService = userService;
            this._organizationService = organizationService;
            this._doctorService = doctorService;
            this._countrySerive = countrySerive;
            this._adminService = adminService;
            this._homeVisitAppointmentService = homeVisitAppointmentService;
            this._cityService = cityService;
            this._StateService = StateService;
            this._genderService = genderService;
            this._ClinicService = clinicService;
        }
        [HttpGet]
        public ActionResult PendingAppointment()
        {
            //var organization = _organizationService.GetOrganizationById(base.loginUserModel.OrganizationId);
            //AppointmentViewModel model = new AppointmentViewModel();
            var commonUtilities = new CommonUtilities();

            var model = appointmentService.GetAppointmentList().Select(x => new AppointmentViewModel
            {
                AppointmentDateLocalText = x.AppointmentDateLocal.ToString("dd/MM/yyyy hh:mm tt"),
                AppointmentId = x.AppointmentId,
                Address1 = StringCipher.Decrypt(userService.Find(x.PatientLoginId).Member.Address1),
                Address2 = StringCipher.Decrypt(userService.Find(x.PatientLoginId).Member.Address2),
                AppointmentType = x.AppointmentType,
                AppointmentTypeText = x.AppointmentType == (int)MyOnlineClinic.Entity.AppointmentType.VideoConsult ?
                                      commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentType.VideoConsult) :
                                      x.AppointmentType == (int)MyOnlineClinic.Entity.AppointmentType.ClinicVisit ?
                                      commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentType.ClinicVisit) :
                                      commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentType.HomeVisit),
                DoctorFullName = x.DoctorLoginId.ToString() == Guid.Empty.ToString() ? "" : StringCipher.Decrypt(userService.Find(x.DoctorLoginId).Member.FirstName) + " " + StringCipher.Decrypt(userService.Find(x.DoctorLoginId).Member.SurName),
                AvailabileDoctorList = x.AppointmentType != (int)MyOnlineClinic.Entity.AppointmentType.HomeVisit ? null : _doctorService.GetDoctorListofAvailabilityDate(x.AppointmentDateLocal.ToString("dd/MM/yyyy hh:mm tt")).ToList(),
                HomeVisitDetail = x.AppointmentType != (int)MyOnlineClinic.Entity.AppointmentType.HomeVisit ? null : _homeVisitAppointmentService.GetDoctorListofHomeVisitAppointmentByAppointmentId(Convert.ToInt32(x.AppointmentId)).ToList(),
                PatientName = StringCipher.Decrypt(userService.Find(x.PatientLoginId).Member.FirstName) + " " + StringCipher.Decrypt(userService.Find(x.PatientLoginId).Member.SurName),
                DoctorLoginId = x.DoctorLoginId,
                PatientLoginId = x.PatientLoginId,
                //ADminTimezone = userService.Find(base.loginUserModel.LoginId).ToString() != "00000000-0000-0000-0000-000000000000" ? "" : Common.UtcToLocal(Convert.ToDateTime(userService.Find(base.loginUserModel.LoginId).ModifiedDate), userService.Find(base.loginUserModel.LoginId).TimeZoneString).ToString("dd/MM/yyyy h:mm tt"),
                //  DoctorTimezone = userService.Find(x.DoctorLoginId).ToString() == "00000000-0000-0000-0000-000000000000" ? "" : Common.UtcToLocal(Convert.ToDateTime(userService.Find(x.PatientLoginId).ModifiedDate), userService.Find(x.DoctorLoginId).TimeZoneString).ToString("dd/MM/yyyy h:mm tt"),
                ////  PatientTimezone = x.PatientLoginId.ToString()!= "00000000-0000-0000-0000-000000000000"? userService.Find(x.PatientLoginId).TimeZoneString:string.Empty,
                PatientTimezone = userService.Find(x.PatientLoginId).ToString() == "00000000-0000-0000-0000-000000000000" ? "" : Common.UtcToLocal(Convert.ToDateTime(userService.Find(x.PatientLoginId).ModifiedDate), userService.Find(x.PatientLoginId).TimeZoneString).ToString("dd/MM/yyyy h:mm tt"),
                DoctorTimezone = userService.Find(x.DoctorLoginId).ToString() == "00000000-0000-0000-0000-000000000000" ? "" : Common.UtcToLocal(Convert.ToDateTime(userService.Find(x.DoctorLoginId).ModifiedDate), userService.Find(x.DoctorLoginId).TimeZoneString).ToString("dd/MM/yyyy h:mm tt"),
                PTimeZoneDisplayName = Common.GetTimeZoneStandardIdAndDisplayName(userService.Find(x.PatientLoginId).TimeZoneString, true),
                DTimeZoneDisplayName = Common.GetTimeZoneStandardIdAndDisplayName(userService.Find(x.DoctorLoginId).TimeZoneString, true),
                ATimeZoneDisplayName = Common.GetTimeZoneStandardIdAndDisplayName(userService.Find(base.loginUserModel.LoginId).TimeZoneString, true),
                AppointmentStatusName = x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.Pending ?
                          commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.Pending) :

                          x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.Completed ?
                          commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.Completed) :

                          x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.Approved ?
                          commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.Approved) :

                          x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.CancelledByDoctor ?
                          commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.CancelledByDoctor) :

                          x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.CancelledByPatient ?
                          commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.CancelledByPatient) :

                          x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.Rejected ?
                          commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.Rejected) :

                          x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.ApprovedNotCompleted ?
                          commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.ApprovedNotCompleted) :

                          x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.CancelByAdmin ?
                          commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.CancelByAdmin) :

                          x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.NotCompleted ?
                          commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.NotCompleted) :

                          x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.NotRespondedByPatient ?
                          commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.NotRespondedByPatient) :

                          x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.NotRespondedByDoctor ?
                          commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.NotRespondedByDoctor) :

                          x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.New ?
                          commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.New) :

                          x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.Assigned ?
                          commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.Assigned) :

                          x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.InTransit ?
                          commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.InTransit) :

                          x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.Seen ?
                          commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.Seen) :

                          commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.Waiting),
                IsActive = x.IsActive

            }).Where(x => x.IsActive == true).ToList();

            for (int i = 0; i < model.Count; i++)
            {
                if (model[i].DoctorLoginId.ToString() != "00000000-0000-0000-0000-000000000000" && model[i].PatientLoginId.ToString() != "00000000-0000-0000-0000-000000000000")
                {
                    // model[i].DoctorId = userService.Find(model[i].DoctorLoginId).Member.MemberId);
                    //model[i].PatientId = userService.Find( model[i].PatientLoginId).Member.MemberId;
                    model[i].DoctorOrgname = StringCipher.Decrypt(_organizationService.GetOrganizationById(userService.Find(model[i].DoctorLoginId).Member.OrgId).OrganizationName);
                    model[i].OrganizationName = StringCipher.Decrypt(_organizationService.GetOrganizationById(userService.Find(model[i].PatientLoginId).Member.OrgId).OrganizationName);
                }
            }

          
            return View(model);
        }

        //[HttpPost]
        //public ActionResult PendingAppointment(SearchParametersViewModel searchModel)
        //{

        //    var ObjFilter = new FilterParameters
        //    {
        //        OrganizationName = searchModel.OrgName,
        //        CountryId = searchModel.CountryId,
        //        StateId = searchModel.StateId,
        //        PatientName=searchModel.PatientName,
        //        DoctorName=searchModel.DoctorName,
        //        AppointmentStatus=searchModel.AppointmentStatus,
        //        AppointmentType=searchModel.AppointmentType,
        //        FromDate=searchModel.FromDate,
        //        ToDate=searchModel.ToDate

        //    };

        //}

        [HttpGet]
        public ActionResult AppointmentRecord(int id)
        {
            AppointmentViewModel model = new AppointmentViewModel();
            if (id != 0)
            {
                var Appointment = appointmentService.GetAppointmentId(id).Select(x => new AppointmentViewModel
                {
                    AppointmentId = x.AppointmentId,
                    PatientLoginId = x.PatientLoginId,
                    DoctorLoginId = x.DoctorLoginId,
                    //DoctorName = StringCipher.Decrypt(_doctorService.GetDoctorByLoginId(x.DoctorLoginId).FirstName) + " " + StringCipher.Decrypt(_doctorService.GetDoctorByLoginId(x.DoctorLoginId).SurName),
                    //DoctorFullName = StringCipher.Decrypt(userService.Find(x.DoctorLoginId).Member.FirstName),
                    DoctorName = x.DoctorLoginId.ToString() == "00000000-0000-0000-0000-000000000000" ? "" : StringCipher.Decrypt(_doctorService.GetDoctorByLoginId(x.DoctorLoginId).FirstName) + " " + StringCipher.Decrypt(_doctorService.GetDoctorByLoginId(x.DoctorLoginId).SurName),
                    DoctorTimezone = _doctorService.GetDoctorByLoginId(x.DoctorLoginId).TimeZoneString == null ? "" : StringCipher.Decrypt(_doctorService.GetDoctorByLoginId(x.DoctorLoginId).TimeZoneString),
                    DoctorAddress = StringCipher.Decrypt(_doctorService.GetDoctorByLoginId(x.DoctorLoginId).Address1),
                    DoctorCountry = _doctorService.GetDoctorByLoginId(x.DoctorLoginId).CountryId,
                    PatientName = StringCipher.Decrypt(userService.GetPatientByLoginId(x.PatientLoginId).FirstName) + " " + StringCipher.Decrypt(userService.GetPatientByLoginId(x.PatientLoginId).SurName),
                    DoctorCountryname = StringCipher.Decrypt(_countrySerive.GetCountryById(_doctorService.GetDoctorByLoginId(x.DoctorLoginId).CountryId).CountryName),
                    PatientTimezone = userService.GetPatientByLoginId(x.PatientLoginId).TimeZoneString == null ? "" : StringCipher.Decrypt(userService.GetPatientByLoginId(x.PatientLoginId).TimeZoneString),
                    PatientAddress = StringCipher.Decrypt(userService.GetPatientByLoginId(x.PatientLoginId).Address1),
                    PatientCountry = userService.GetPatientByLoginId(x.PatientLoginId).CountryId,
                    PatientCountryname = StringCipher.Decrypt(_countrySerive.GetCountryById(userService.GetPatientByLoginId(x.PatientLoginId).CountryId).CountryName),
                    PatientProfilePic = userService.Find(x.PatientLoginId).Avatar == null ? "" : userService.Find(x.PatientLoginId).Avatar,
                    DoctorProfilePic = userService.Find(x.DoctorLoginId).Avatar == null ? "" : userService.Find(x.DoctorLoginId).Avatar,
                    OrganizationName = StringCipher.Decrypt(_organizationService.GetOrganizationById(userService.Find(x.PatientLoginId).Member.OrgId).OrganizationName),
                    ADminTimezone = StringCipher.Decrypt(_adminService.GetAdminUserLoginId(base.loginUserModel.LoginId).TimeZoneString)
                }).FirstOrDefault();
                return View(Appointment);
            }
            else
            {
                return View(model);
            }

        }

        public ActionResult Search(AppointmentViewModel model)
        {
            var commonUtilities = new CommonUtilities();

            var appoinmentList = appointmentService.GetAppointmentList().Select(x => new AppointmentViewModel
            {
                AppointmentDateLocalText = StringCipher.Decrypt(x.AppointmentDateLocal.ToString("dd/MM/yyyy h:mm tt")),
                AppointmentId = x.AppointmentId,
                AppointmentType = x.AppointmentType,
                AppointmentTypeText = StringCipher.Decrypt((x.AppointmentType == (int)MyOnlineClinic.Entity.AppointmentType.VideoConsult ?
                                      commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentType.VideoConsult) :
                                      x.AppointmentType == (int)MyOnlineClinic.Entity.AppointmentType.ClinicVisit ?
                                      commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentType.ClinicVisit) :
                                      commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentType.HomeVisit))),
                //DoctorFullName = StringCipher.Decrypt(userService.Find(x.DoctorLoginId).Member.FirstName),
                DoctorFullName = x.DoctorLoginId.ToString() == "00000000-0000-0000-0000-000000000000" ? "" : StringCipher.Decrypt(userService.Find(x.DoctorLoginId).Member.FirstName),
                PatientName = StringCipher.Decrypt(userService.Find(x.PatientLoginId).Member.FirstName),
                OrganizationName = StringCipher.Decrypt(_organizationService.GetOrganizationById(userService.Find(x.PatientLoginId).Member.OrgId).OrganizationName),
                AppointmentStatusName = x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.Pending ?
                                      commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.Pending) :

                                      x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.Completed ?
                                      commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.Completed) :

                                      x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.Approved ?
                                      commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.Approved) :

                                      x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.CancelledByDoctor ?
                                      commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.CancelledByDoctor) :

                                      x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.CancelledByPatient ?
                                      commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.CancelledByPatient) :

                                      x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.Rejected ?
                                      commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.Rejected) :

                                      x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.ApprovedNotCompleted ?
                                      commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.ApprovedNotCompleted) :

                                      x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.CancelByAdmin ?
                                      commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.CancelByAdmin) :

                                      x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.NotCompleted ?
                                      commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.NotCompleted) :

                                      x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.NotRespondedByPatient ?
                                      commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.NotRespondedByPatient) :

                                      x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.NotRespondedByDoctor ?
                                      commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.NotRespondedByDoctor) :

                                      x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.New ?
                                      commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.New) :

                                      x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.Assigned ?
                                      commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.Assigned) :

                                      x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.InTransit ?
                                      commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.InTransit) :

                                      x.AppointmentStatus == (int)MyOnlineClinic.Entity.AppointmentStatus.Seen ?
                                      commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.Seen) :

                                      commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentStatus.Waiting),
                AppointmentStatus = x.AppointmentStatus,
                IsActive = x.IsActive

            }).Where(x => x.IsActive == true).ToList();
            if (model.OrganizationName != null)
            {
                appoinmentList = appoinmentList.Where(x => x.OrganizationName.Contains(model.OrganizationName)).ToList();
            }
            if (model.PatientName != null)
            {
                appoinmentList = appoinmentList.Where(x => x.PatientName.Contains(model.PatientName)).ToList();
            }
            if (model.DoctorName != null)
            {
                appoinmentList = appoinmentList.Where(x => x.DoctorFullName.Contains(model.DoctorName)).ToList();
            }
            if (model.AppointmentStatus > 0)
            {
                appoinmentList = appoinmentList.Where(x => x.AppointmentStatus == model.AppointmentStatus).ToList();
            }
            if (model.AppointmentType > 0)
            {
                appoinmentList = appoinmentList.Where(x => x.AppointmentType == model.AppointmentType).ToList();
            }
            if (model.FroMDate != null)
            {
                appoinmentList = appoinmentList.Where(x => x.AppointmentDateLocal == model.FroMDate).ToList();
            }
            if (model.TOdate != null)
            {
                appoinmentList = appoinmentList.Where(x => x.AppointmentDateLocal == model.TOdate).ToList();
            }
            return PartialView("_ManageAppointment", appoinmentList);
            // return Json(appoinmentList, JsonRequestBehavior.AllowGet);


        }

        public ActionResult AssignDoctorForHomeVisitAppointment(int AppointmentId, int DoctorId)
        {
            var DoctorLoginId = _doctorService.GetDoctorById(DoctorId).ToList().FirstOrDefault().LoginId;
            var appid = appointmentService.GetAppointmentById(AppointmentId);
            appid.AppointmentId = AppointmentId;
            appid.DoctorLoginId = DoctorLoginId;
            appointmentService.updateDoctorForHomeVisit(appid);
            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDoctorAndPatientOnMap()
        {
            Dictionary<string, object> dicMain = new Dictionary<string, object>();
            List<Dictionary<string, object>> dicpatient = new List<Dictionary<string, object>>();
            var patientList = appointmentService.GetAllPatientOfHomeVisitAppointment().Select(x =>
                    new AppointmentViewModel
                    {
                        AppointmentDateLocalText = x.AppointmentDateLocal.ToString("dd/MM/yyyy h:mm tt"),
                        AppointmentId = x.AppointmentId,
                        PatientName = StringCipher.Decrypt(userService.Find(x.PatientLoginId).Member.FirstName) + " " + StringCipher.Decrypt(userService.Find(x.PatientLoginId).Member.SurName),
                        Address1 = StringCipher.Decrypt(userService.Find(x.PatientLoginId).Member.Address1),
                        PhoneNo= StringCipher.Decrypt(userService.Find(x.PatientLoginId).Member.PhoneNumber),
                        Address2 = StringCipher.Decrypt(userService.Find(x.PatientLoginId).Member.Address2),
                        latitude = _homeVisitAppointmentService.GetHomeVisitAppointmentByAppId(x.AppointmentId).latitude,
                        longitude = _homeVisitAppointmentService.GetHomeVisitAppointmentByAppId(x.AppointmentId).longitude,
                        Location = _homeVisitAppointmentService.GetHomeVisitAppointmentByAppId(x.AppointmentId).location,
                        AvailabileDoctorList = x.AppointmentType != (int)MyOnlineClinic.Entity.AppointmentType.HomeVisit ? null : _doctorService.GetDoctorListofAvailabilityDate(x.AppointmentDateLocal.ToString("dd/MM/yyyy h:mm tt")).ToList(),
                        HomeVisitDetail = x.AppointmentType != (int)MyOnlineClinic.Entity.AppointmentType.HomeVisit ? null : _homeVisitAppointmentService.GetDoctorListofHomeVisitAppointmentByAppointmentId(Convert.ToInt32(x.AppointmentId)).ToList()
                    }).ToList();
            for (int i = 0; i < patientList.Count; i++)
            {
                Dictionary<string, object> PatientDetail = new Dictionary<string, object>();
                PatientDetail.Add("PatientName", patientList[i].PatientName);
                PatientDetail.Add("AppointmentDateLocalText", patientList[i].AppointmentDateLocalText);
                PatientDetail.Add("PhoneNumber", patientList[i].PhoneNo);
                PatientDetail.Add("Address1", patientList[i].Address1);
                PatientDetail.Add("Address2", patientList[i].Address2);
                PatientDetail.Add("latitude", patientList[i].latitude);
                PatientDetail.Add("longitude", patientList[i].longitude);
                PatientDetail.Add("Location", patientList[i].Location);
                PatientDetail.Add("HomeVisitDetail", patientList[i].HomeVisitDetail);
                dicpatient.Add(PatientDetail);
            }
            dicMain.Add("PatientDetail", dicpatient);
            
            // here doctor details
            var curdate = DateTime.Now.ToString("dd/MM/yyyy h:mm tt");
            //string d = DateTime.ParseExact(curdate, "dd/MM/yyyy h:mm tt", CultureInfo.InvariantCulture);
            List<Dictionary<string, object>> dicDoctor = new List<Dictionary<string, object>>();
            var doctorList = _doctorService.GetDoctorListofAvailabilityDate(curdate).Select(x =>
                new AppointmentViewModel
                {
                    DoctorName = StringCipher.Decrypt(userService.Find(x.DoctorAvailabilitys.Doctors.LoginId).Member.FirstName) + " " + StringCipher.Decrypt(userService.Find(x.DoctorAvailabilitys.Doctors.LoginId).Member.SurName),
                    Address1 = StringCipher.Decrypt(userService.Find(x.DoctorAvailabilitys.Doctors.LoginId).Member.Address1),
                    PhoneNo = StringCipher.Decrypt(userService.Find(x.DoctorAvailabilitys.Doctors.LoginId).Member.PhoneNumber),
                    Address2 = StringCipher.Decrypt(userService.Find(x.DoctorAvailabilitys.Doctors.LoginId).Member.Address2),
                    latitude = _doctorService.GetDoctorAvailabilityList().Where(y => x.AvailabilityId == y.AvailabilityId && y.AvailabilityType == (int)MyOnlineClinic.Entity.AppointmentType.HomeVisit).FirstOrDefault().latitude,
                    longitude = _doctorService.GetDoctorAvailabilityList().Where(y => x.AvailabilityId == y.AvailabilityId && y.AvailabilityType == (int)MyOnlineClinic.Entity.AppointmentType.HomeVisit).FirstOrDefault().longitude,
                    cityid=x.AvailabilityId,
                    //CityName = StringCipher.Decrypt(_cityService.GetCityById(x.DoctorAvailabilitys.CityId).LookUpCityName)
                    //CityName="Sydney"
                }).ToList();
            for (int i = 0; i < doctorList.Count; i++)
            {
                Dictionary<string, object> DoctorDetail = new Dictionary<string, object>();
                DoctorDetail.Add("DoctorName", doctorList[i].DoctorName);
                //DoctorDetail.Add("CityName", doctorList[i].CityName);
                //DoctorDetail.Add("AppointmentDateLocalText", doctorList[i].AppointmentDateLocalText);
                DoctorDetail.Add("PhoneNumber", doctorList[i].PhoneNo);
                DoctorDetail.Add("Address1", doctorList[i].Address1);
                DoctorDetail.Add("Address2", doctorList[i].Address2);
                DoctorDetail.Add("latitude", doctorList[i].latitude);
                DoctorDetail.Add("longitude", doctorList[i].longitude);
                //DoctorDetail.Add("Location", doctorList[i].Location);
                //DoctorDetail.Add("HomeVisitDetail", doctorList[i].HomeVisitDetail);
                DoctorDetail.Add("ava Id", doctorList[i].cityid);
                dicDoctor.Add(DoctorDetail);
            }
            dicMain.Add("DoctorDetail", dicDoctor);
            return Json(dicMain, JsonRequestBehavior.AllowGet);
            //return Json(patientList, JsonRequestBehavior.AllowGet);
            //return Json("", JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPatientDetail(Guid UserId)
        {
            var organization = _organizationService.GetOrganizationById(base.loginUserModel.OrganizationId);
            var id = userService.Find(UserId).Member.MemberId;
            //.Where(t => organization != null && t.OrgId == organization.OrganizationId)
            var model = userService.GetPatientById(Convert.ToInt32(id)).Select(x => new RegisterViewModel
            {
                FullName = StringCipher.Decrypt(x.FirstName) + " " + StringCipher.Decrypt(x.SurName),
                // FirstName = StringCipher.Decrypt(x.FirstName),
                //   SurName = StringCipher.Decrypt(x.SurName),
                // Title = StringCipher.Decrypt(_titleService.GetTitleById(x.Title).TitleName),
                State = x.State,
                CountryId = x.CountryId,
                City = x.CountryId,
                Address1 = StringCipher.Decrypt(x.Address1),
                //  MiddleName = StringCipher.Decrypt(x.MiddleName),
                Address2 = StringCipher.Decrypt(x.Address2),
                PhoneNumber = StringCipher.Decrypt(x.PhoneNumber),
                MobileNumber = StringCipher.Decrypt(x.MobileNumber),
                FaxNumber = StringCipher.Decrypt(x.FaxNumber),
                Email = StringCipher.Decrypt(x.Email),
                DOB = x.DOB,
                PostCode = StringCipher.Decrypt(x.PostCode),
                OrgId = x.OrgId,

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
                //LoginId = x.LoginId,
                //   GenderId = x.Gender,
                // MemberId = x.MemberId,


            }).ToList().FirstOrDefault();
            if (model.OrgId != 0)
            {
                try
                {
                    model.organizationName = StringCipher.Decrypt(_organizationService.GetOrganizationById(model.OrgId).OrganizationName);
                }
                catch { }
            }

            if (model.State != null)
            {
                model.StateName = StringCipher.Decrypt(_StateService.GetStateById(model.State).StateName);
            }


            if (model.CountryId != null)
            {
                model.CountryName = StringCipher.Decrypt(_countrySerive.GetCountryById(model.CountryId).CountryName);
            }
            if (model.City != null)
            {
                model.CityName = StringCipher.Decrypt(_cityService.GetCityById(model.City).LookUpCityName);
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDoctorDetail(Guid UserId)
        {
            var organization = _organizationService.GetOrganizationById(base.loginUserModel.OrganizationId);
            var id = userService.Find(UserId).Member.MemberId;
            //.Where(t => organization != null && t.OrgId == organization.OrganizationId)
            var model = _doctorService.GetDoctorById(Convert.ToInt32(id)).Select(x => new RegisterViewModel
            {
                FullName = x.MiddleName != null ? StringCipher.Decrypt(x.FirstName) + " " + StringCipher.Decrypt(x.MiddleName) + " " + StringCipher.Decrypt(x.SurName) : StringCipher.Decrypt(x.FirstName) + " " + StringCipher.Decrypt(x.SurName),
                Email = StringCipher.Decrypt(x.Email),
                DOB = x.DOB,
                GenderId = Convert.ToInt32(x.Gender),
                MobileNumber = x.MobileNumber == null ? " " : StringCipher.Decrypt(x.MobileNumber),
                FaxNumber = x.FaxNumber == null ? " " : StringCipher.Decrypt(x.FaxNumber),
                City = Convert.ToInt32(x.City),
                State = Convert.ToInt32(x.State),
                CountryId = Convert.ToInt32(x.CountryId),
                PostCode = x.PostCode == null ? " " : StringCipher.Decrypt(x.PostCode),
                Timezone = x.TimeZoneString == null ? " " : StringCipher.Decrypt(x.TimeZoneString),
                PhoneNumber = x.PhoneNumber == null ? " " : StringCipher.Decrypt(x.PhoneNumber),
                Address1 = StringCipher.Decrypt(x.Address1 == null ? " " : x.Address1),
                Address2 = StringCipher.Decrypt(x.Address2 == null ? " " : x.Address2),
                OrgId = Convert.ToInt32(x.OrgId),
                LoginId = x.LoginId,
                IsActive = x.IsActive,
                SignaturePic = x.SignaturePic,
                MemberId = x.MemberId,
                UserorgId = x.OrgId,
                DoctorLoginId = x.LoginId,
                ModyfiedDate = (DateTime)x.ModifiedDate,
                //GenderName =x.Gender==null?"": _genderService.GetGenderById(x.Gender).GenderName
            }).ToList().FirstOrDefault();

            if (model.Gender != null)
            {
                model.GenderName = _genderService.GetGenderList().Where(x => x.GenderId == Convert.ToInt32(model.Gender) && x.Type == Convert.ToInt32(RoleType.Doctor)).FirstOrDefault().GenderName;
            }
            if (model.OrgId != 0)
            {
                try
                {
                    model.organizationName = StringCipher.Decrypt(_organizationService.GetOrganizationById(model.OrgId).OrganizationName);
                    model.OrganizationAddress1 = StringCipher.Decrypt(_organizationService.GetOrganizationById(model.OrgId).Address1);
                    model.OrganizationPostalCode = StringCipher.Decrypt(_organizationService.GetOrganizationById(model.OrgId).PostCode);
                    model.OrganizationAddress2 = StringCipher.Decrypt(_organizationService.GetOrganizationById(model.OrgId).Address1);
                    //model.Address2 = StringCipher.Decrypt(_organizationservice.GetOrganizationById(model.OrgId).Address2);
                    var orgCountry = _organizationService.GetOrganizationById(model.OrgId).Country;
                    model.OrgCountryName = StringCipher.Decrypt(_countrySerive.GetCountryById(orgCountry).CountryName);
                    var ClinicState = _organizationService.GetOrganizationById(model.OrgId).State;
                    model.OrgStateName = StringCipher.Decrypt(_StateService.GetStateById(orgCountry).StateName);
                    var ClinicCity = _organizationService.GetOrganizationById(model.OrgId).City;
                    model.OrgCityName = StringCipher.Decrypt(_cityService.GetCityById(ClinicCity).LookUpCityName);
                }
                catch { }
            }
            //if (model.MiddleName != null)
            //{
            //    model.FullName = StringCipher.Decrypt(model.FirstName) + " " + StringCipher.Decrypt(model.MiddleName) + " " + StringCipher.Decrypt(model.SurName);
            //}
            //else
            //{
            //    model.FullName = StringCipher.Decrypt(model.FirstName) + " " + StringCipher.Decrypt(model.SurName);
            //}
            if (model.GenderId != 0)
            {
                model.GenderName = StringCipher.Decrypt(_genderService.GetGenderById(model.GenderId).GenderName);
            }
            if (model.CountryId != 0)
            {
                model.CountryName = StringCipher.Decrypt(_countrySerive.GetCountryById(model.CountryId).CountryName);
            }
            if (model.State != 0)
            {
                model.StateName = StringCipher.Decrypt(_StateService.GetStateById(model.State).StateName);
            }
            if (model.City != 0)
            {
                model.CityName = StringCipher.Decrypt(_cityService.GetCityById(model.City).LookUpCityName);
            }
            try
            {
                model.ProfilePic = userService.Find(model.LoginId).Avatar;
            }
            catch { }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CancelAppointment(int appointmentId)
        {
            var appointment = appointmentService.GetAppointmentById(Convert.ToInt32(appointmentId));
            appointment.AppointmentStatus = 8;
            appointmentService.Update(appointment);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ChangeDate(string selectedDate, string mainHour, string mainMin, string MainAmPm, int appointmentId)
        {
            var appointment = appointmentService.GetAppointmentById(Convert.ToInt32(appointmentId));
            //appointment.AppointmentDateUtc = "";
            var stdate = selectedDate + " " + mainHour + ":" + mainMin + " " + MainAmPm;
            var enddate = Convert.ToDateTime(stdate).AddMinutes(Convert.ToDouble(appointment.ConsultTime));
            //  var enddate = Convert.ToDateTime(stdate).AddHours(Convert.ToDouble(mainHour)).AddMinutes(Convert.ToDouble(mainMin));
            appointment.AppointmentDateUtc = Convert.ToDateTime(stdate);
            appointment.AppointmentEndDateUtc = Convert.ToDateTime(enddate);

            //get + -
            var splidtzone = appointment.TimeZoneString.Split('+');
            //get hour minute
            var timezonestring = splidtzone[1].Split(':');
            double Hour = Convert.ToDouble(timezonestring[0]);
            double minute = Convert.ToDouble(timezonestring[1]);
            if (appointment.TimeZoneString.Contains('+'))
            {
                appointment.AppointmentDateLocal = Convert.ToDateTime(stdate).AddHours(Hour).AddMinutes(minute);
            }
            else if (appointment.TimeZoneString.Contains('-'))
            {
                appointment.AppointmentDateLocal = Convert.ToDateTime(stdate).AddHours(-Hour).AddMinutes(-minute);
            }
            //appointment.
            appointmentService.Update(appointment);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAppointmentdate(int appointmentId)
        {
            var appointment = appointmentService.GetAppointmentById(Convert.ToInt32(appointmentId));
            string[] strarr = new string[4];
            strarr[0] = appointment.AppointmentDateUtc.ToString("tt", CultureInfo.InvariantCulture);
            strarr[1] = appointment.AppointmentDateUtc.ToString("hh", CultureInfo.InvariantCulture);
            strarr[2] = appointment.AppointmentDateUtc.ToString("mm", CultureInfo.InvariantCulture);
            strarr[3] = appointment.AppointmentDateUtc.Date.ToString("dd/MM/yyyy");
            return Json(strarr, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPharmacyDetail(Guid UserId)
        {
            var organization = _organizationService.GetOrganizationById(base.loginUserModel.OrganizationId);
            var id = userService.Find(UserId).Member.MemberId;
            //.Where(t => organization != null && t.OrgId == organization.OrganizationId)
            var model = userService.GetPatientById(Convert.ToInt32(id)).Select(x => new RegisterViewModel
            {
                //FullName = StringCipher.Decrypt(x.FirstName) + " " + StringCipher.Decrypt(x.SurName),
                // FirstName = StringCipher.Decrypt(x.FirstName),
                //   SurName = StringCipher.Decrypt(x.SurName),
                // Title = StringCipher.Decrypt(_titleService.GetTitleById(x.Title).TitleName),
                // State = x.State,
                // CountryId = x.CountryId,
                // City = x.CountryId,
                // Address1 = StringCipher.Decrypt(x.Address1),
                //  MiddleName = StringCipher.Decrypt(x.MiddleName),
                // Address2 = StringCipher.Decrypt(x.Address2),
                //PhoneNumber = StringCipher.Decrypt(x.PhoneNumber),
                // MobileNumber = StringCipher.Decrypt(x.MobileNumber),
                // FaxNumber = StringCipher.Decrypt(x.FaxNumber),
                // Email = StringCipher.Decrypt(x.Email),
                //  DOB = x.DOB,
                //   PostCode = StringCipher.Decrypt(x.PostCode),
                OrgId = x.OrgId,

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
                // GernealNotes = StringCipher.Decrypt(x.GernealNotes),
                // TreatingDoctors = StringCipher.Decrypt(x.TreatingDoctors),
                //LoginId = x.LoginId,
                //   GenderId = x.Gender,
                // MemberId = x.MemberId,


            }).ToList().FirstOrDefault();
            if (model.PharmacyCountryId != null)
            {
                model.CountryName = StringCipher.Decrypt(_countrySerive.GetCountryById(model.PharmacyCountryId).CountryName);
            }
            if (model.PharmacyState != null)
            {
                model.StateName = StringCipher.Decrypt(_StateService.GetStateById(model.PharmacyState).StateName);
            }
            if (model.PharmacyCity != null)
            {
                model.CityName = StringCipher.Decrypt(_cityService.GetCityById(model.PharmacyCity).LookUpCityName);
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}