using MyOnlineClinic.Entity;
using MyOnlineClinic.Web.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MyOnlineClinic.Web.Models;
using MyOnlineClinic.Web.Helper;
namespace MyOnlineClinic.Web.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [System.Web.Mvc.CompareAttribute("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
    public class LoginViewModel
    {
        //[Required(ErrorMessage = "User Name is required !")]
        //[Display(Name = "Username")]
        //public string UserName { get; set; }


        [Required(ErrorMessage = "Inserir Utilizador")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Inserir Password !")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        public int UserType { get; set; }

    }


    //public class LoginViewModel
    //{
    //    [Required]
    //    [Display(Name = "User name")]
    //    public string UserName { get; set; }

    //    [Required]
    //    [DataType(DataType.Password)]
    //    [Display(Name = "Password")]
    //    public string Password { get; set; }

    //    [Display(Name = "Remember me?")]
    //    public bool RememberMe { get; set; }
    //}

    public class RegisterViewModel
    {
        public int MemberId { get; set; }
        public Guid LoginId { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string SurName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Suburb { get; set; }
        public string PostCode { get; set; }
        public int? CountryId { get; set; }
        public int? State { get; set; }
        public int? City { get; set; }
        public int? TitleId { get; set; }
        public int? GenderId { get; set; }
        public string ClinicName { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public string ProfessionName { get; set; }
        public string CityName { get; set; }
        public string PharmacyCountryName { get; set; }
        public string PharmacyStateName { get; set; }
        public string PharmacyCityName { get; set; }
        public string TitleName { get; set; }
        public string GenderName { get; set; }
        public string VoucherTime { get; set; }
        public string VideoProviderNo { get; set; }
        public string ClinicProviderNo { get; set; }
        public string VoucherNo { get; set; }
        public string HomeVisitProviderNo { get; set; }
        public int? Profession { get; set; }
        public int OrgId { get; set; }
        public string OrganizationAddress { get; set; }
     
        public string ClinicAddress { get; set; }
        public Guid Createdby { get; set; }
        public int ClinicID { get; set; }
        public string TimeZoneId { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yy}")]
        public DateTime? DOB { get; set; }
        public string Gender { get; set; }
        public string MobileNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string MedicareNumber { get; set; }
        [Range(typeof(int), "1", "20", ErrorMessage = "{0} can only be between {1} and {2}")]
        public string MedicareRefNumber { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yy}")]
        public DateTime? MedicareExpire { get; set; }
        public string DavNumber { get; set; }
        public string PrivateFund { get; set; }
        public string PrivateFundMembershipNo { get; set; }
        public string HCCPensionNo { get; set; }
        public string HCCPensionNoExpiry { get; set; }
        public string UsualGPName { get; set; }
        public string UsualGPAddress { get; set; }
        public int? DavCardColor { get; set; }
        public string DavDisablities { get; set; }
        public string UsualGPClinicName { get; set; }
        public string GPAddress2 { get; set; }
        public int? UsualGpCity { get; set; }
        public int? UsualGpCountry { get; set; }
        public int? UsualGpState { get; set; }
        public string UsualGpPostCode { get; set; }
        public string usualGpStateName { get; set; }
        public string FullName { get; set; }
        public string ProfilePic { get; set; }
        public string Timezone { get; set; }
        public string organizationName { get; set; }
        public string GernealNotes { get; set; }
        //added by arvind 16/3/2016 for remaining doctor field
        public int PhoneCode { get; set; }
        public string LanguageSpoken { get; set; }
        // public string Suburb { get; set; }
        public int? ProfessionCategory { get; set; }
        public string IntrestArea { get; set; }
        public string qualification { get; set; }
        public string AHPRANo { get; set; }
        public string ProviderNo { get; set; }
        public string PrescriberNo { get; set; }
        public string DoctorProfile { get; set; }
        public string SignaturePic { get; set; }
        public List<TimeZones> TimeZoneList { get; set; }
        public List<Organization> OrganizationList { get; set; }
        public List<Patient> GetPatientList { get; set; }
        public List<DoctorConsultFee> TelemedicineFeeList { get; set; }
        public DoctorConsultFee Prescriptionmodel { get; set; }
        public List<DoctorConsultFee> ClinicVisitFeeList { get; set; }
        public IEnumerable<AppointmentViewModel> Appointmentmodel { get; set; }
        public IEnumerable<AppointmentViewModel> PastConsultModel { get; set; }
        public List<TreatingDoctorViewModel> ListoftreatingDoctors { get; set; }
        //Model for patienthistory//
        public IEnumerable<PatientAllergies> AllergiesList { get; set; }
        public IEnumerable<PatientPastMedical> PastMedicallist { get; set; }
        public IEnumerable<PatientMedication> MedicationHistoryList { get; set; }
        public IEnumerable<PatientSmoking> SmokingList { get; set; }
        public IEnumerable<PatientFamily> FamilyHistoryList { get; set; }
        public IEnumerable<PatientEmployment> EmploymentList { get; set; }
        public IEnumerable<PatientMedicalRecord> OtherMedicalRecordsList { get; set; }
        public IEnumerable<PatientAlcohal> AlcoholList { get; set; }
        public PatientHistoryViewModel patientmodel { get; set; }
        public string PharmacyName { get; set; }
        public string PharmacyAddress1 { get; set; }
        public string PharmacyAddress2 { get; set; }
        public string PharmacySuburb { get; set; }
        public string PharmacyPostCode { get; set; }
        public int? PharmacyCountryId { get; set; }
        public int? PharmacyState { get; set; }
        public int? PharmacyCity { get; set; }
        public string PharmacyMobileNumber { get; set; }
        public string PharmacyPhoneNumber { get; set; }
        public string PharmacyFaxNumber { get; set; }
        public string TreatingDoctors { get; set; }
        public int? VoucherId { get; set; }
        //[Required(ErrorMessage = "Inserir Email ")]
        //[EmailAddress(ErrorMessage = "Invalid Email Address")]
        //[Remote("ValidateEmail", "Account", " ", ErrorMessage = "Endereço de email já registrado")]
        [Remote("doesUserNameExist", "Patient", HttpMethod = "POST", ErrorMessage = "Email is already exists")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        //[Required(ErrorMessage = "Inserir Uitlizador")]
        [Display(Name = "User name")]
        //[Remote("ValidateUser", "Account", ErrorMessage = "nome de usuário já utilizado!!")]
        public string UserName { get; set; }
        //[Required(ErrorMessage = "Inserir Password")]
        //[StringLength(100, ErrorMessage = "A {0} deve ter pelo menos {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        //[Required(ErrorMessage = "Repetir Password")]
        //[DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        //[System.Web.Mvc.CompareAttribute("Password", ErrorMessage = "A senha de senha e confirmação não coincidem.")]
        public string ConfirmPassword { get; set; }
        //[Required(ErrorMessage = "Por favor escolha o perfil")]
        [Display(Name = "Profile")]
        public RoleType RoleId { get; set; }
        public bool Newsletter { get; set; }
        // public Guid LoginId { get; set; }
        public List<LookUpRole> RoleList { get; set; }
        public List<LookUpCountry> CountryList { get; set; }
        public List<LookUpCity> CityList { get; set; }
        public List<ProfessionTypes> ProfessionTypes { get; set; }
        public List<LookUpTitle> TitleList { get; set; }
        public List<LookUpGender> GenderList { get; set; }
        public bool IsActive { get; set; }
        public DateTime? ModiNfiedDate { get; set; }
        public DateTime ModyfiedDate { get; set; }
        public List<SelectListItem> dvacolor { get; set; }

        public List<LookUpState> StateList { get; set; }
        public List<Clinic> ClinicList { get; set; }
        public List<DoctorClinicModel> clinicmodellist { get; set; }
        public List<DoctorClinicModel> clinicProviderlist { get; set; }
        public List<RegisterViewModel> RecentlyDoctors { get; set; }
        public List<RegisterViewModel> ActivateDoctors { get; set; }
        public OrganizationViewModel Orgmodel { get; set; }
        public string UserOrganization { get; set; }

        public string OrganizationAddress1 { get; set; }

        public string OrganizationAddress2 { get; set; }

        public int? OrganizationCountryId { get; set; }

        public int? OrganizationStateId { get; set; }

        public int? OrganizationCityId { get; set; }

        public string OrgCountryName { get; set; }
        public string OrgStateName { get; set; }
        public string OrgCityName { get; set; }

        public string GPCountryName { get; set; }
        public string GPStateName { get; set; }
        public string GPCityName { get; set; }
        public string OrganizationPostalCode { get; set; }

        public string UserClinic { get; set; }

        public string ClinicAddress1 { get; set; }

        public string ClinicAddress2 { get; set; }

        public int ClinicCountryId { get; set; }

        public int ClinicStateId { get; set; }

        public int ClinicCityId { get; set; }

        public string ClinicPostCodeCode { get; set; }

        public string DVAColorName { get; set; }

        public int OrganizationId { get; set; }
        public List<LookUpCountry> OrganizationCountryList { get; set; }

        public List<LookUpState> OrganizationStateList { get; set; }
        public List<ProfessionSubType> ProfessionSubType { get; set; }
        public List<LookUpCity> OrganizationCityList { get; set; }

        public List<LookUpCountry> ClinicCountryList { get; set; }

        public List<LookUpState> ClinicStateList { get; set; }

        public List<LookUpCity> ClinicCityList { get; set; }

        public string UserClinicName { get; set; }
        public Guid DoctorLoginId { get; set; }
        public string UserClinicAddress1 { get; set; }

        public string UserClinicAddress2 { get; set; }
        public string UsualGpCountryName { get; set; }
        public int UserClinicId { get; set; }

        public int? UserClinicCountryId { get; set; }

        public int? UserClinicStateId { get; set; }

        public int? UserClinicCityId { get; set; }

        public int PatientOrgId { get; set; }
        public int PatientClinicId { get; set; }

        public string UserClinicPostalCode { get; set; }

        public List<LookUpCountry> UserClinicCountryList { get; set; }

        public List<LookUpState> UserClinicStateList { get; set; }
        public List<RadiologyType> RadioTypeList { get; set; }
        public List<Reason> RegionListbyId { get; set; }
        public List<LookUpCity> UserClinicCityList { get; set; }
        public List<RegisterViewModel> OrgClinicIdsList { get; set; }
        public List<TreatingDoctors> TreatingDocotrsIdsList { get; set; }

        public string OrgClinicDoctorsIds { get; set; }
        public string OrgClinicIds { get; set; }
        public string UpdateOrgClinicIds { get; set; }
        public int UserorgId { get; set; }
        public string ContactUserName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactMessage { get; set; }
        public string TreatingDoctorsIds { get; set; }


        public List<NewConsultation> NewConsultationList { get; set; }

        public List<FutureAppointmentTypeViewModel> VideoAppointmentsList { get; set; }
        public List<FutureAppointmentTypeViewModel> HomevisitAppointmentsList { get; set; }
        public List<FutureAppointmentTypeViewModel> ClinicvisitAppointmentsList { get; set; }

        public int? AppointmentType { get; set; }
        public decimal? TMAStatdardFee { get; set; }
        public decimal? TMALongFee { get; set; }
        public decimal? TMAVeryLongFee { get; set; }
        public int TMACurrencyType { get; set; }

        public decimal? ClinicLongFee { get; set; }
        public decimal? ClinicVeryLongFee { get; set; }
        public List<RegisterViewModel> feedetails { get; set; }
        public decimal? tmaRepeatPrescription { get; set; }
        public string Bsb { get; set; }
        public List<PatientHistoryViewModel> patientHistory { get; set; }
        public string BankName { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string PaypelEmailId { get; set; }
        public DoctorMembership membership { get; set; }
        public DoctorAccountDetails DoctorAccountdetail { get; set; }
        public string paymentType { get; set; }
        public List<RegisterViewModel> Mypatientlist { get; set; }
        public List<TreatingDoctorViewModel> TreatingDoctorss { get; set; }
        public OrganizationViewModel OrganizationDetails { get; set; }



        public RegisterViewModel()
        {

        }
        public RegisterViewModel(RegisterViewModel m)
        {
            this.FirstName = m.FirstName;
            this.MiddleName = m.MiddleName;
            this.SurName = m.SurName;
            this.MemberId = MemberId;
            this.LoginId = LoginId;
            this.Title = Title;
            this.PostCode = PostCode;
            this.CountryId = CountryId;
            this.City = City;
            this.State = State;
            this.Gender = Gender;
            this.MobileNumber = MobileNumber;
            this.PhoneNumber = PhoneNumber;
            this.Email = Email;
            this.ProfilePic = ProfilePic;
            this.organizationName = organizationName;
            this.ClinicID = ClinicID;
            this.FaxNumber = FaxNumber;
            this.DOB = Convert.ToDateTime(DOB.ToString());
            this.Timezone = Timezone;
            this.TimeZoneId = TimeZoneId;
            this.ModyfiedDate = ModyfiedDate;
            this.IsActive = IsActive;
            this.TitleId = TitleId;
            this.GenderId = GenderId;
            this.DVAColorName = DVAColorName;
        }
        //public Login GetEditLogin(RegisterViewModel m)
        //{

        //    this.FirstName = m.FirstName;
        //    this.MiddleName = m.MiddleName;
        //    this.SurName = m.SurName;
        //    this.MemberId = MemberId;
        //    this.LoginId = LoginId;
        //    this.Title = Title;
        //    this.PostCode = PostCode;
        //    this.CountryId = CountryId;
        //    this.City = City;
        //    this.State = State;
        //    this.Gender = Gender;
        //    this.MobileNumber = MobileNumber;
        //    this.PhoneNumber = PhoneNumber;
        //    this.Email = Email;
        //    this.ProfilePic = ProfilePic;
        //    this.organizationName = organizationName;
        //    this.ClinicID = ClinicID;
        //    this.FaxNumber = FaxNumber;
        //    this.DOB = DOB;
        //    this.Timezone = Timezone;
        //    this.TimeZoneId = TimeZoneId;
        //    this.ModyfiedDate = ModyfiedDate;
        //    this.IsActive = IsActive;           
        //    this.FirstName = m.FirstName;
        //    this.MiddleName = m.MiddleName;
        //    this.SurName = m.SurName;
        //    this.MemberId = MemberId;
        //    this.LoginId = LoginId;
        //    this.Title = Title;
        //    this.PostCode = PostCode;
        //    this.CountryId = CountryId;
        //    this.City = City;
        //    this.State = State;
        //    this.Gender = Gender;
        //    this.MobileNumber = MobileNumber;
        //    this.PhoneNumber = PhoneNumber;
        //    this.Email = Email;
        //    this.ProfilePic = ProfilePic;
        //    this.organizationName = organizationName;
        //    this.ClinicID = ClinicID;
        //    this.FaxNumber = FaxNumber;
        //    this.DOB = DOB;
        //    this.Timezone = Timezone;
        //    this.TimeZoneId = TimeZoneId;
        //    this.ModyfiedDate = ModyfiedDate;
        //    this.IsActive = IsActive;
        //    this.LoginId = LoginId;

        //}
        public Login GetLogin(BaseMember member, string Email)
        {
            Login login = new Login
            {
                //LoginName=UserName,
                LoginName = StringCipher.Encrypt(Email),

                Avatar = ProfilePic,
                //Add role accordingly
                //Add role accordingly
                LookUpRoleId = (Int32)RoleId,
                Email = StringCipher.Encrypt(Email),
                Newsletter = Newsletter,
            };
            var SelectTimeZone = member.TimeZoneString;
            member.LoginId = login.LoginId;
            login.Member = member;
            login.Member.SetCreated(login.LoginId);
            login.SetCreated(login.LoginId);
            return login;
        }
        public BaseMember GetDoctor()
        {
            BaseMember profile = new Doctor
            {
                MemberId = MemberId,
                Title = TitleId,
                FirstName = FirstName,
                MiddleName = MiddleName,
                SurName = SurName,
                Address1 = Address1,
                Address2 = Address2,
                Suburb = Suburb,
                PostCode = PostCode,
                CountryId = CountryId,
                State = State,
                City = City,
                OrganizationAddress = OrganizationAddress,
                TimeZoneString = TimeZoneId,
                DOB = DateTime.Now,
                Gender = GenderId,
                MobileNumber = MobileNumber,
                PhoneNumber = PhoneNumber,
                FaxNumber = FaxNumber,
                Profession = Profession,
                ClinicID = ClinicID,
                OrgId = 1,
                ProfessionCategory = ProfessionCategory ?? 1


            };
            profile = SetMember(profile);
            return profile;
        }
        public Doctor SetDoctor()
        {
            Doctor profile = new Doctor
            {

                MemberId = MemberId,
                Title = TitleId,
                FirstName = FirstName,
                MiddleName = MiddleName,
                SurName = SurName,
                Address1 = Address1,
                Address2 = Address2,
                Suburb = Suburb,
                PostCode = PostCode,
                CountryId = CountryId,
                State = State,
                City = City,
                OrganizationAddress = OrganizationAddress,
                TimeZoneString = TimeZoneId,
                //  DOB = DOB,
                //DOB = DOB,
                Gender = GenderId,
                MobileNumber = MobileNumber,
                PhoneNumber = PhoneNumber,
                FaxNumber = FaxNumber,
                DOB = Convert.ToDateTime(DateTime.Now.ToString()),
                Profession = Profession,
                ClinicID = ClinicID,
                OrgId = OrgId
            };
            profile = SetMemberAdmin(profile);

            return profile;
        }
        public Doctor AdminEditDoctor(RegisterViewModel model)
        {
            var cmodel = new Doctor
            {
                MemberId = model.MemberId,
                Title = model.TitleId,
                FirstName = StringCipher.Encrypt(model.FirstName),
                Email = StringCipher.Encrypt(model.Email),
                SurName = StringCipher.Encrypt(model.SurName),
                CountryId = model.CountryId,
                City = model.City,
                Address1 = StringCipher.Encrypt(model.Address1),
                Address2 = StringCipher.Encrypt(model.Address2),
                Suburb = model.Suburb,
                PostCode = StringCipher.Encrypt(model.PostCode),
                DOB = Convert.ToDateTime(model.DOB),
                State = model.State,
                TimeZoneString = model.TimeZoneId,
                Gender = model.GenderId,
                MobileNumber = StringCipher.Encrypt(model.MobileNumber),
                PhoneNumber = StringCipher.Encrypt(model.PhoneNumber),
                FaxNumber = StringCipher.Encrypt(model.FaxNumber),
                Profession = model.Profession,
                ClinicID = ClinicID,
                MiddleName = StringCipher.Encrypt(model.MiddleName),
                OrgId = model.OrgId,
                LoginId = model.LoginId,
                ModifiedDate = DateTime.Now,
                //ModifiedBy=model.LoginId,
                // ModifiedBy=base.loginUserModel.LoginId,
                CreatedBy = model.Createdby,
                PhoneCode = PhoneCode,
                LanguageSpoken = StringCipher.Encrypt(LanguageSpoken),
                ProfessionCategory = ProfessionCategory ?? 1,
                IntrestArea = StringCipher.Encrypt(IntrestArea),
                qualification = StringCipher.Encrypt(qualification),
                AHPRANo = StringCipher.Encrypt(AHPRANo),
                VideoProviderNo = StringCipher.Encrypt(VideoProviderNo),
                ClinicProviderNo = StringCipher.Encrypt(ClinicProviderNo),
                HomeVisitProviderNo = StringCipher.Encrypt(HomeVisitProviderNo),
                PrescriberNo = StringCipher.Encrypt(PrescriberNo),
                DoctorProfile = StringCipher.Encrypt(DoctorProfile),
                SignaturePic = SignaturePic,
            };
            return cmodel;
        }
        public BaseMember GetDoctorAdmin()
        {
            Doctor profile = new Doctor
            {
                MemberId = MemberId,
                Title = TitleId,
                FirstName = StringCipher.Encrypt(FirstName),
                MiddleName = StringCipher.Encrypt(MiddleName),
                SurName = StringCipher.Encrypt(SurName),
                Address1 = StringCipher.Encrypt(Address1),
                Address2 = StringCipher.Encrypt(Address2),
                Suburb = Suburb,
                PostCode = StringCipher.Encrypt(PostCode),
                CountryId = CountryId,
                State = State,
                City = City,
                OrganizationAddress = StringCipher.Encrypt(OrganizationAddress),
                TimeZoneString = TimeZoneId,
                //  DOB = DOB,
                //DOB = DOB,
                Gender = GenderId,
                MobileNumber = StringCipher.Encrypt(MobileNumber),
                // PhoneNumber = PhoneNumber,
                FaxNumber = StringCipher.Encrypt(FaxNumber),
                DOB = Convert.ToDateTime(DateTime.Now.ToString()),
                Profession = Profession,
                ClinicID = ClinicID,
                OrgId = OrgId,
                // PhoneNumber = PhoneCode + "-" + PhoneNumber,
                PhoneNumber = StringCipher.Encrypt(PhoneNumber),
                LanguageSpoken = StringCipher.Encrypt(LanguageSpoken),
                ProfessionCategory = ProfessionCategory ?? 1,
                IntrestArea = StringCipher.Encrypt(IntrestArea),
                qualification = StringCipher.Encrypt(qualification),
                AHPRANo = StringCipher.Encrypt(AHPRANo),
                VideoProviderNo = StringCipher.Encrypt(VideoProviderNo),
                HomeVisitProviderNo = StringCipher.Encrypt(HomeVisitProviderNo),
                ClinicProviderNo = StringCipher.Encrypt(ClinicProviderNo),
                PrescriberNo = StringCipher.Encrypt(PrescriberNo),
                DoctorProfile = StringCipher.Encrypt(DoctorProfile),
                SignaturePic = StringCipher.Encrypt(SignaturePic),
            };
            profile = SetMemberAdmin(profile);
            return profile;
        }

        public BaseMember GetAdminUser()
        {
            BaseMember profile = new AdminUser();
            profile = SetMember(profile);
            return profile;
        }
        public BaseMember GetPatient()
        {
            BaseMember profile = new MyOnlineClinic.Entity.Patient
            {
            };
            profile = SetMember(profile);
            return profile;
        }
        public BaseMember SetMember(BaseMember member)
        {

            member.FirstName = FirstName;
            member.Email = Email;
            member.SurName = SurName;
            member.CountryId = CountryId;
            member.City = City;
            member.Address1 = Address1;
            member.Address2 = Address2;
            member.Suburb = Suburb;
            member.PostCode = PostCode;
            member.CountryId = CountryId;
            member.State = State;

            member.DOB = DateTime.Now;

            member.Gender = GenderId;
            member.MobileNumber = MobileNumber;
            member.PhoneNumber = PhoneNumber;
            member.FaxNumber = FaxNumber;
            return member;
        }
        public Doctor SetMemberAdmin(Doctor member)
        {
            member.FirstName = StringCipher.Encrypt(FirstName);
            member.Email = StringCipher.Encrypt(Email);
            member.SurName = StringCipher.Encrypt(SurName);
            member.CountryId = CountryId;
            member.City = City;
            member.Address1 = StringCipher.Encrypt(Address1);
            member.Address2 = StringCipher.Encrypt(Address2);
            member.Suburb = Suburb;
            member.PostCode = StringCipher.Encrypt(PostCode);
            member.CountryId = CountryId;
            member.State = State;
            member.Gender = GenderId;
            member.Title = TitleId;
            //  member.DOB = DOB;
            member.DOB = Convert.ToDateTime(DateTime.Now.ToString());
            member.ModifiedDate = DateTime.Now;


            return member;
        }
        //for patient registration in admn..by dheeraj
        public Patient GetAdminPatient(int orgid)
        {
            Patient profile = new Patient();
            profile = AdminSetMember(profile, orgid);
            return profile;
        }
        public Patient AdminSetMember(Patient patient, int orgid)
        {
            patient.MemberId = MemberId;
            patient.Title = TitleId;
            patient.FirstName = StringCipher.Encrypt(FirstName);
            patient.Email = StringCipher.Encrypt(Email);
            patient.MiddleName = StringCipher.Encrypt(MiddleName);
            patient.SurName = StringCipher.Encrypt(SurName);
            patient.CountryId = CountryId;
            patient.City = City;
            patient.Address1 = StringCipher.Encrypt(Address1);
            patient.Address2 = StringCipher.Encrypt(Address2);
            patient.Suburb = Suburb;
            patient.PostCode = StringCipher.Encrypt(PostCode);
            patient.CountryId = CountryId;
            patient.State = State;
            patient.TimeZoneString = TimeZoneId;
            patient.DOB = Convert.ToDateTime(DOB.ToString());
            patient.Gender = GenderId;
            patient.MobileNumber = StringCipher.Encrypt(MobileNumber);
            patient.PhoneNumber = StringCipher.Encrypt(PhoneNumber);
            patient.FaxNumber = StringCipher.Encrypt(FaxNumber);
            patient.MedicareNumber = StringCipher.Encrypt(MedicareNumber);
            patient.MedicareRefNo = StringCipher.Encrypt(MedicareRefNumber);
            patient.Expiry = MedicareExpire;
            patient.DVANumber = StringCipher.Encrypt(DavNumber);
            patient.PrivateFund = StringCipher.Encrypt(PrivateFund);
            patient.PrivateFundMembershipNo = StringCipher.Encrypt(PrivateFundMembershipNo);
            patient.HCCPensionNo = StringCipher.Encrypt(HCCPensionNo);
            patient.HCCPensionNoExpiry = StringCipher.Encrypt(HCCPensionNoExpiry);
            patient.UsualGPName = StringCipher.Encrypt(UsualGPName);
            patient.UsualGPAddress = StringCipher.Encrypt(UsualGPAddress);
            patient.DavColorCard = DavCardColor;
            patient.DavDisablities = StringCipher.Encrypt(DavDisablities);
            patient.UsualGPClinicName = StringCipher.Encrypt(UsualGPClinicName);
            patient.UsualGpCountry = UsualGpCountry;
            patient.UsualGpCity = UsualGpCity;
            patient.UsualGpState = UsualGpState;
            patient.UsualGpPostCode = StringCipher.Encrypt(UsualGpPostCode);
            patient.GPAddress2 = StringCipher.Encrypt(GPAddress2);
            patient.OrgId = orgid;
            patient.PharmacyName = StringCipher.Encrypt(PharmacyName);
            patient.PharmacyAddress1 = StringCipher.Encrypt(PharmacyAddress1);
            patient.PharmacyAddress2 = StringCipher.Encrypt(PharmacyAddress2);
            patient.PharmacySuburb = StringCipher.Encrypt(PharmacySuburb);
            patient.PharmacyCity = PharmacyCity;
            patient.PharmacyCountryId = PharmacyCountryId;
            patient.PharmacyFaxNumber = StringCipher.Encrypt(PharmacyFaxNumber);
            patient.PharmacyMobileNumber = StringCipher.Encrypt(PharmacyMobileNumber);
            patient.PharmacyPhoneNumber = StringCipher.Encrypt(PharmacyPhoneNumber);
            patient.PharmacyPostCode = StringCipher.Encrypt(PharmacyPostCode);
            patient.PharmacyState = PharmacyState;
            patient.GernealNotes = StringCipher.Encrypt(GernealNotes);
            patient.TreatingDoctors = StringCipher.Encrypt(TreatingDoctors);
            return patient;
        }
        public Patient AdminEditMember(RegisterViewModel model)
        {
            var rModel = new Patient
          {
              MemberId = model.MemberId,
              Title = model.TitleId,
              FirstName = StringCipher.Encrypt(model.FirstName),
              Email = StringCipher.Encrypt(model.Email),
              MiddleName = StringCipher.Encrypt(model.MiddleName),
              SurName = StringCipher.Encrypt(model.SurName),
              CountryId = model.CountryId,
              City = model.City,
              Address1 = StringCipher.Encrypt(model.Address1),
              Address2 = StringCipher.Encrypt(model.Address2),
              Suburb = model.Suburb,
              PostCode = StringCipher.Encrypt(model.PostCode),
              DOB = Convert.ToDateTime(DOB.ToString()),
              State = model.State,
              TimeZoneString = StringCipher.Encrypt(model.TimeZoneId),
              Gender = model.GenderId,
              MobileNumber = StringCipher.Encrypt(model.MobileNumber),
              PhoneNumber = StringCipher.Encrypt(model.PhoneNumber),
              FaxNumber = StringCipher.Encrypt(model.FaxNumber),
              MedicareNumber = StringCipher.Encrypt(model.MedicareNumber),
              MedicareRefNo = StringCipher.Encrypt(model.MedicareRefNumber),
              DVANumber = StringCipher.Encrypt(model.DavNumber),
              PrivateFund = StringCipher.Encrypt(model.PrivateFund),
              PrivateFundMembershipNo = StringCipher.Encrypt(model.PrivateFundMembershipNo),
              HCCPensionNo = StringCipher.Encrypt(model.HCCPensionNo),
              HCCPensionNoExpiry = StringCipher.Encrypt(model.HCCPensionNoExpiry),
              UsualGPName = StringCipher.Encrypt(model.UsualGPName),
              UsualGPAddress = StringCipher.Encrypt(model.UsualGPAddress),
              DavColorCard = model.DavCardColor,
              DavDisablities = StringCipher.Encrypt(model.DavDisablities),
              UsualGPClinicName = StringCipher.Encrypt(model.UsualGPClinicName),
              UsualGpCountry = model.UsualGpCountry,
              UsualGpCity = model.UsualGpCity,
              UsualGpState = model.UsualGpState,
              UsualGpPostCode = StringCipher.Encrypt(model.UsualGpPostCode),
              GPAddress2 = StringCipher.Encrypt(model.GPAddress2),
              OrgId = model.OrgId,
              LoginId = model.LoginId,
              PharmacyName = StringCipher.Encrypt(model.PharmacyName),
              PharmacyAddress1 = StringCipher.Encrypt(model.PharmacyAddress1),
              PharmacyAddress2 = StringCipher.Encrypt(model.PharmacyAddress2),
              PharmacySuburb = StringCipher.Encrypt(model.PharmacySuburb),
              PharmacyCity = model.PharmacyCity,
              PharmacyCountryId = model.PharmacyCountryId,
              PharmacyFaxNumber = StringCipher.Encrypt(model.PharmacyFaxNumber),
              PharmacyMobileNumber = StringCipher.Encrypt(model.PharmacyMobileNumber),
              PharmacyPhoneNumber = StringCipher.Encrypt(model.PharmacyPhoneNumber),
              PharmacyPostCode = StringCipher.Encrypt(model.PharmacyPostCode),
              PharmacyState = model.PharmacyState,
              GernealNotes = StringCipher.Encrypt(model.GernealNotes),
              TreatingDoctors = StringCipher.Encrypt(model.TreatingDoctors),
              Expiry = model.MedicareExpire
              //pro=model.ProfilePic

          };
            return rModel;
        }
        public RegisterViewModel(Patient patient)
        {
            this.MemberId = MemberId;
            this.TitleId = TitleId;
            this.FirstName = FirstName;
            this.Email = Email;
            this.SurName = SurName;
            this.CountryId = CountryId;
            this.City = City;
            this.Address1 = Address1;
            this.Address2 = Address2;
            this.Suburb = Suburb;
            this.PostCode = PostCode;
            this.CountryId = CountryId;
            this.State = State;
            this.Timezone = Timezone;
            this.DOB = DOB;
            this.GenderId = GenderId;
            this.MobileNumber = MobileNumber;
            this.PhoneNumber = PhoneNumber;
            this.FaxNumber = FaxNumber;
            this.MedicareNumber = MedicareNumber;
            this.MedicareRefNumber = MedicareRefNumber;
            this.MedicareExpire = MedicareExpire;
            this.DavNumber = DavNumber;
            this.PrivateFund = PrivateFund;
            this.PrivateFundMembershipNo = PrivateFundMembershipNo;
            this.HCCPensionNo = HCCPensionNo;
            this.HCCPensionNoExpiry = HCCPensionNoExpiry;
            this.UsualGPName = UsualGPName;
            this.UsualGPAddress = UsualGPAddress;
            this.DavCardColor = DavCardColor;
            this.DavDisablities = DavDisablities;
            this.UsualGPClinicName = UsualGPClinicName;
            this.UsualGpCountry = UsualGpCountry;
            this.UsualGpCity = UsualGpCity;
            this.UsualGpState = UsualGpState;
            this.UsualGpPostCode = UsualGpPostCode;
            this.GPAddress2 = GPAddress2;
            this.OrgId = OrgId;

        }
        public Login GetLoginAdmin(Patient member, string Email)
        {
            Login login = new Login
            {
                //LoginName=UserName,
                LoginName = StringCipher.Encrypt(Email),

                //Add role accordingly
                LookUpRoleId = (Int32)RoleId,
                Email = StringCipher.Encrypt(Email),
                Newsletter = Newsletter,
                Avatar = ProfilePic,
            };
            var SelectTimeZone = member.TimeZoneString;
            member.LoginId = login.LoginId;
            login.Member = member;
            login.Member.SetCreated(login.LoginId, SelectTimeZone, false);
            login.SetCreated(login.LoginId, SelectTimeZone, false);
            return login;
        }


    }
    public class AdminRegisterViewModel
    {
        public int MemberId { get; set; }
        public Guid LoginId { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string SurName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Suburb { get; set; }
        public string PostCode { get; set; }
        public int? CountryId { get; set; }
        public int? State { get; set; }
        public int? City { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
        public string MobileNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string Oreganizationname { get; set; }
        public int? titleid { get; set; }
        public int? genderid { get; set; }

        [Required(ErrorMessage = "Inserir Email ")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Remote("ValidateEmail", "Account", " ", ErrorMessage = "Endereço de email já registrado")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Inserir Uitlizador")]
        [Display(Name = "User name")]
        [Remote("ValidateUser", "Account", ErrorMessage = "nome de usuário já utilizado!!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Inserir Password")]
        [StringLength(100, ErrorMessage = "A {0} deve ter pelo menos {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Repetir Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.Web.Mvc.CompareAttribute("Password", ErrorMessage = "A senha de senha e confirmação não coincidem.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Por favor escolha o perfil")]
        [Display(Name = "Profile")]
        public RoleType RoleId { get; set; }

        public bool Newsletter { get; set; }

        // public Guid LoginId { get; set; }

        public List<LookUpRole> RoleList { get; set; }
        public List<LookUpCountry> CountryList { get; set; }

        public List<LookUpCity> CityList { get; set; }

        //public List<DoctorConsultFee> StandardConsultFeeList { get; set; }  
        public Login GetLogin(BaseMember member)
        {
            Login login = new Login
            {
                LoginName = UserName,
                LoginPassword = Password,
                LookUpRoleId = (Int32)RoleId,
                Email = Email,
                Newsletter = Newsletter,

            };
            member.LoginId = login.LoginId;
            login.Member = member;
            login.Member.SetCreated(login.LoginId, string.Empty, false);
            login.SetCreated(login.LoginId, string.Empty, false);
            return login;

        }

        public BaseMember GetDoctor()
        {
            BaseMember profile = new Doctor
            {
                MemberId = MemberId,
                Title = titleid,
                FirstName = FirstName,
                MiddleName = MiddleName,
                SurName = SurName,
                Address1 = Address1,
                Address2 = Address2,
                Suburb = Suburb,
                PostCode = PostCode,
                CountryId = CountryId,
                State = State,
                City = City,
                DOB = DateTime.Now,
                Gender = genderid,
                MobileNumber = MobileNumber,
                PhoneNumber = PhoneNumber,
                FaxNumber = FaxNumber,

            };
            profile = SetMember(profile);
            return profile;
        }
        public BaseMember GetAdminUser()
        {
            BaseMember profile = new AdminUser();
            profile = SetMember(profile);
            return profile;
        }

        public BaseMember GetPatient()
        {
            BaseMember profile = new Patient
            {
            };
            profile = SetMember(profile);
            return profile;
        }

        public BaseMember SetMember(BaseMember member)
        {
            member.FirstName = FirstName;
            member.Email = Email;
            member.SurName = SurName;
            member.CountryId = CountryId;
            member.City = City;
            member.Address1 = Address1;
            member.Address2 = Address2;
            member.Suburb = Suburb;
            member.PostCode = PostCode;
            member.CountryId = CountryId;
            member.State = State;
            member.DOB = DateTime.Now;
            member.Gender = genderid;
            member.MobileNumber = MobileNumber;
            member.PhoneNumber = PhoneNumber;
            member.FaxNumber = FaxNumber;

            return member;
        }
    }

    public class TreatingDoctorViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int DoctorId { get; set; }
        public string TitleName { get; set; }
        public bool IsDelete { get; set; }
        public bool IsActive { get; set; }
    }
    public class FutureAppointmentTypeViewModel
    {
        public int AppointmentId { get; set; }

        public int AppointmentType { get; set; }

        public Guid PatientLoginId { get; set; }

        public Guid DoctorLoginId { get; set; }

        public string DoctorPic { get; set; }

        public string AppointmentDate { get; set; }

        public string AppointmentTime { get; set; }

        public DateTime AppointmentDateLocal { get; set; }

        public int AppointmentStatus { get; set; }

        public string AppointmentStatusText { get; set; }

        public string DoctorFullName { get; set; }

        public string DoctorTitle { get; set; }

    }
}
