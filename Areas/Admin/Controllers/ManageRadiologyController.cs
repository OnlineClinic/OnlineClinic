using MyOnlineClinic.RepositoryService;
using MyOnlineClinic.Web.Controllers;
using MyOnlineClinic.Web.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using MyOnlineClinic.Web.Helper;

namespace MyOnlineClinic.Web.Areas.Admin.Controllers
{
    public class ManageRadiologyController : BaseController
    {
        private readonly IManagePrescriptionService _managePrescriptionService;
        private readonly IUserService _userService;
        private readonly IDoctorService _doctorService;
        private readonly IAppointmentService _appointmentService;
        private readonly IOrganizationService _organizationService;
        private readonly IClinicalService _clinicalService;
        private readonly IGenderService _genderService;
        public ManageRadiologyController(IManagePrescriptionService managePrescriptionService, IUserService userService, IDoctorService doctorService, IAppointmentService appointmentService, IOrganizationService organizationService, IClinicalService clinicalService, IGenderService genderService)
        {
            _managePrescriptionService = managePrescriptionService;
            _userService = userService;
            _doctorService = doctorService;
            _appointmentService = appointmentService;
            _organizationService = organizationService;
            _clinicalService = clinicalService;
            _genderService = genderService;
        }
        //
        // GET: /Admin/ManageRadiology/
        public ActionResult Index()
        {
            var radiology = _clinicalService.GetAllRadiology().Select(x => new RadiologyViewModel
            {

                RadiologyId = x.RadiologyId,
                AppointmentId = x.AppointmentId,
                PatientId = x.PatientUserId,
                DoctorId = x.DoctorUserId,
                RequestPrint = StringCipher.Decrypt(x.RequestPrint),
                ClinicalNotes = StringCipher.Decrypt(x.ClinicalNotes),
                DoctorName = StringCipher.Decrypt(_doctorService.GetDoctorByLoginId(x.DoctorUserId).FirstName) + " " + StringCipher.Decrypt(_doctorService.GetDoctorByLoginId(x.DoctorUserId).SurName),
                PatientName = StringCipher.Decrypt(_userService.GetPatientByLoginId(x.PatientUserId).FirstName) + " " + StringCipher.Decrypt(_userService.GetPatientByLoginId(x.PatientUserId).SurName),
                OrganizationName = StringCipher.Decrypt(_organizationService.GetOrganizationById(_userService.GetPatientByLoginId(x.PatientUserId).OrgId).OrganizationName),
                RequestNo =x.RequestNo,
                CopyResult = x.CopyResult,
               
               // res = StringCipher.Decrypt(x.InstructionForPatient),
                //FastingNonFastingValue = x.FastingNonFastingValue
            }).ToList();

            return View(radiology);
        }
        public JsonResult GetPatientDetail(Guid PatientId, Guid doctorId)
        {
            var id = _userService.Find(PatientId).Member.MemberId;
            var model = _userService.GetPatientById(Convert.ToInt32(id)).Select(x => new RegisterViewModel
            {
                FullName = StringCipher.Decrypt(x.FirstName) + " " + StringCipher.Decrypt(x.SurName),
                Address1 = StringCipher.Decrypt(x.Address1),
                Address2 = StringCipher.Decrypt(x.Address2),
                PhoneNumber = StringCipher.Decrypt(x.PhoneNumber),
                Email = StringCipher.Decrypt(x.Email),
                MedicareNumber = StringCipher.Decrypt(x.MedicareNumber),
                MedicareExpire = x.Expiry,
                DOB = x.DOB,
                GenderName = StringCipher.Decrypt(_genderService.GetGenderById(x.Gender).GenderName),
                DoctorName = _doctorService.GetDoctorByLoginId(doctorId).MiddleName != null ? StringCipher.Decrypt(_doctorService.GetDoctorByLoginId(doctorId).FirstName) + " " + _doctorService.GetDoctorByLoginId(doctorId).MiddleName + " " + StringCipher.Decrypt(_doctorService.GetDoctorByLoginId(doctorId).SurName) : StringCipher.Decrypt(_doctorService.GetDoctorByLoginId(doctorId).FirstName) + " " + StringCipher.Decrypt(_doctorService.GetDoctorByLoginId(doctorId).SurName),
                VideoProviderNo = StringCipher.Decrypt(_doctorService.GetDoctorByLoginId(doctorId).VideoProviderNo),
                SignaturePic = "/UploadedFiles/OrganizationLogo/" + _doctorService.GetDoctorByLoginId(doctorId).SignaturePic
            }).ToList().FirstOrDefault();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

    }
}