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

namespace MyOnlineClinic.Web.Areas.Clinic.Controllers
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


        public AppointmentController(IAppointmentService appointmentService, IUserService userService, IOrganizationService organizationService, IDoctorService doctorService,
            ICountryService countrySerive, IAdminUserService adminService, IHomeVisitAppointmentService homeVisitAppointmentService,
            ICityService cityService, IClinicUserService clinicUserService)
        {
            this.appointmentService = appointmentService;
            this.userService = userService;
            this._organizationService = organizationService;
            this._doctorService = doctorService;
            this._countrySerive = countrySerive;
            this._adminService = adminService;
            this._homeVisitAppointmentService = homeVisitAppointmentService;
            this._cityService = cityService;

        }

        [HttpGet]
        public ActionResult PendingAppointment()
        {
            //AppointmentViewModel model = new AppointmentViewModel();
            var commonUtilities = new CommonUtilities();

            var model = appointmentService.GetAppointmentList().Select(x => new AppointmentViewModel
                                  {
                                      AppointmentDateLocalText = x.AppointmentDateLocal.ToString("dd/MM/yyyy h:mm tt"),
                                      AppointmentId = x.AppointmentId,
                                      Address1 = StringCipher.Decrypt(userService.Find(x.PatientLoginId).Member.Address1),
                                      Address2 = StringCipher.Decrypt(userService.Find(x.PatientLoginId).Member.Address2),
                                      AppointmentType = x.AppointmentType,
                                      AppointmentTypeText = x.AppointmentType == (int)MyOnlineClinic.Entity.AppointmentType.VideoConsult ?
                                                            commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentType.VideoConsult) :
                                                            x.AppointmentType == (int)MyOnlineClinic.Entity.AppointmentType.ClinicVisit ?
                                                            commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentType.ClinicVisit) :
                                                            commonUtilities.GetDisplayName(MyOnlineClinic.Entity.AppointmentType.HomeVisit),
                                      DoctorFullName = x.DoctorLoginId.ToString() == "00000000-0000-0000-0000-000000000000" ? "" : StringCipher.Decrypt(userService.Find(x.DoctorLoginId).Member.FirstName),
                                      AvailabileDoctorList = x.AppointmentType != (int)MyOnlineClinic.Entity.AppointmentType.HomeVisit ? null : _doctorService.GetDoctorListofAvailabilityDate(x.AppointmentDateLocal.ToString("dd/MM/yyyy h:mm tt")).ToList(),
                                      HomeVisitDetail = x.AppointmentType != (int)MyOnlineClinic.Entity.AppointmentType.HomeVisit ? null : _homeVisitAppointmentService.GetDoctorListofHomeVisitAppointmentByAppointmentId(Convert.ToInt32(x.AppointmentId)).ToList(),


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
                                      IsActive = x.IsActive

                                  }).Where(x => x.IsActive == true).ToList();
            //  return PartialView("_ManageAppointment", appoinmentList);
            //  return PartialView("_ManageAppointment", appoinmentList);
            //if (model.DoctorLoginId != null)
            //{
            //    model.AvailabileDoctorList = _doctorService.GetDoctorListofAvailabilityDate(model.AppointmentDateLocal.ToString("dd/MM/yyyy h:mm tt")).ToList();
            //}


            var userClinic = userService.GetUsersList(base.loginUserModel.ClinicId, (int)RoleType.Doctor);
            string doctorids = string.Empty;

            if (userClinic != null && userClinic.ToList().Count > 0)
            {
                doctorids = String.Join(userClinic.Select(x => x.UserId).ToString(), ',');

                var doctors = _doctorService.GetDoctorList().Where(t => t.MemberId.ToString().Contains(doctorids)).ToList();
                string doctorLoginId = String.Join(doctors.Select(x => x.LoginId).ToString(), ',');

                model = model.ToList().Where(x => x.DoctorLoginId.ToString().Contains(doctorLoginId)).ToList();
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

            var userClinic = userService.GetUsersList(base.loginUserModel.ClinicId, (int)RoleType.Doctor);
            string doctorids = string.Empty;

            if (userClinic != null && userClinic.ToList().Count > 0)
            {
                doctorids = String.Join(userClinic.Select(x => x.UserId).ToString(), ',');

                var doctors = _doctorService.GetDoctorList().Where(t => t.MemberId.ToString().Contains(doctorids)).ToList();
                string doctorLoginId = String.Join(doctors.Select(x => x.LoginId).ToString(), ',');

               appoinmentList =   appoinmentList.ToList().Where(x => x.DoctorLoginId.ToString().Contains(doctorLoginId)).ToList();
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
                        PhoneNo = StringCipher.Decrypt(userService.Find(x.PatientLoginId).Member.PhoneNumber),
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
                    cityid = x.AvailabilityId,
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
    }
}