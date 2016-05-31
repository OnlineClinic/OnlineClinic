using MyOnlineClinic.RepositoryService;
using MyOnlineClinic.Web.Controllers;
using MyOnlineClinic.Web.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using MyOnlineClinic.Web.Helper;

namespace MyOnlineClinic.Web.Areas.Admin.Controllers
{
    public class ManagePathologyController : BaseController
    {
        private readonly IManagePrescriptionService _managePrescriptionService;
        private readonly IUserService _userService;
        private readonly IDoctorService _doctorService;
        private readonly IAppointmentService _appointmentService;
        private readonly IOrganizationService _organizationService;
        private readonly IClinicalService _clinicalService;
        private readonly IStateService _stateService;
        private readonly IGenderService _genderService;
        public ManagePathologyController(IManagePrescriptionService managePrescriptionService, IUserService userService, IDoctorService doctorService, IAppointmentService appointmentService, IOrganizationService organizationService, IClinicalService clinicalService, IStateService stateService, IGenderService genderService)
        {
            _managePrescriptionService = managePrescriptionService;
            _userService = userService;
            _doctorService = doctorService;
            _appointmentService = appointmentService;
            _organizationService = organizationService;
            _clinicalService = clinicalService;
            _stateService = stateService;
            _genderService = genderService;
        }

        // GET: /Admin/ManagePathology/
        public ActionResult Index()
        {
            var model = _clinicalService.GetAllPathology().Select(x => new PathologyViewModel
            {
                PathologyId = x.PathologyId,
                TestName = StringCipher.Decrypt(x.TestName),
                PatientLoginId=x.PatientLoginId,
                ClinicalNotes = StringCipher.Decrypt(x.ClinicalNotes),
                DoctorLoginId = x.DoctorLoginId,
                DoctorName = StringCipher.Decrypt(_doctorService.GetDoctorByLoginId(x.DoctorLoginId).FirstName) + " " + StringCipher.Decrypt(_doctorService.GetDoctorByLoginId(x.DoctorLoginId).SurName),
                AppointmentId = x.AppointmentId,
                CreatedDate = Convert.ToDateTime(x.CreatedDate).ToString("dd/MM/yyyy"),
                PatientName = StringCipher.Decrypt(_userService.GetPatientByLoginId(x.PatientLoginId).FirstName) + " " + StringCipher.Decrypt(_userService.GetPatientByLoginId(x.PatientLoginId).SurName),
                OrganizationName = StringCipher.Decrypt(_organizationService.GetOrganizationById(_userService.GetPatientByLoginId(x.PatientLoginId).OrgId).OrganizationName),
                RequestNo=x.RequestNo,
              CopyResultTo= StringCipher.Decrypt(x.CopyResultTo),
              InstructionForPatient=StringCipher.Decrypt(x.InstructionForPatient),
              FastingNonFastingValue=x.FastingNonFastingValue
            }).ToList();
            return View(model);
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
                MedicareNumber=StringCipher.Decrypt(x.MedicareNumber),
                MedicareExpire=x.Expiry,
                DOB = x.DOB,
                GenderName= StringCipher.Decrypt(_genderService.GetGenderById(x.Gender).GenderName),
                DoctorName = _doctorService.GetDoctorByLoginId(doctorId).MiddleName != null ? StringCipher.Decrypt(_doctorService.GetDoctorByLoginId(doctorId).FirstName) + " " + _doctorService.GetDoctorByLoginId(doctorId).MiddleName + " " + StringCipher.Decrypt(_doctorService.GetDoctorByLoginId(doctorId).SurName) : StringCipher.Decrypt(_doctorService.GetDoctorByLoginId(doctorId).FirstName) + " " + StringCipher.Decrypt(_doctorService.GetDoctorByLoginId(doctorId).SurName),
                VideoProviderNo = StringCipher.Decrypt(_doctorService.GetDoctorByLoginId(doctorId).VideoProviderNo),
                SignaturePic = "/UploadedFiles/OrganizationLogo/" + _doctorService.GetDoctorByLoginId(doctorId).SignaturePic
            }).ToList().FirstOrDefault();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}