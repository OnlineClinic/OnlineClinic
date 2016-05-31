using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyOnlineClinic.Entity;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using MyOnlineClinic.Web.Helper;
namespace MyOnlineClinic.Web.Models
{
    public class UnregisteredOrganizationsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public int Country { get; set; }
        public int State { get; set; }
        public string ProfilePic { get; set; }
        public int City { get; set; }
        public string PostCode { get; set; }
        public string AddedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Timezone { get; set; }
        public string FaxNumber { get; set; }
        public string PhoneNumber { get; set; }
        public int OrganizationType { get; set; }
        public string OrganizationLogo { get; set; }
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }
        public string TimezoneId { get; set; }
        public int RoleType { get; set; }
        public List<OrganizationTypes> OrganizationTypes { get; set; }
        public List<LookUpCountry> Countries { get; set; }
        public List<LookUpCity> Cities { get; set; }
        public List<LookUpState> Statelist { get; set; }
        public List<OrganizationUserViewModel> OrganizationUserList { get; set; }
        public List<OrganizationTypes> OrganizationTypeList { get; set; }
        public List<TimeZones> TimeZonesesList { get; set; }
        public List<Clinic> ClinicList { get; set; }
        public List<LookUpTitle> TitleList { get; set; }
        public OrganizationUserViewModel OrganizationUser { get; set; }
        public UnregisteredOrganizationsViewModel() { }
        public UnregisteredOrganizationsViewModel(UnregisteredOrganizations model)
        {
            this.Name = StringCipher.Decrypt(model.Name);
            this.OrganizationType = model.OrganizationType;
            this.AddressLine1 = StringCipher.Decrypt(model.AddressLine1);
            this.AddressLine2 = StringCipher.Decrypt(model.AddressLine2);
            this.Country = (int)model.Country;
            this.State = (int)model.State;
            this.City = (int)model.City;
            this.PostCode = StringCipher.Decrypt(model.PostCode);
            this.FaxNumber = model.FaxNumber;
            this.PhoneNumber = model.PhoneNumber;        
            this.OrganizationLogo = model.OrganizationLogo;     
            this.IsActive = model.IsActive;
            this.TimezoneId = model.TimeZoneString;
            this.IsApproved = model.IsApproved;
        }
        public UnregisteredOrganizations GetEditModel(UnregisteredOrganizationsViewModel model)
        {
            UnregisteredOrganizations rModel = new UnregisteredOrganizations();
            rModel.Name = StringCipher.Encrypt(model.Name);
            rModel.OrganizationType = model.OrganizationType;
            rModel.AddressLine1 = StringCipher.Encrypt(model.AddressLine1);
            rModel.AddressLine2 = StringCipher.Encrypt(model.AddressLine2);
            rModel.Country = model.Country;
            rModel.State = model.State;
            rModel.City = model.City;
            rModel.PostCode = StringCipher.Encrypt(model.PostCode);
            rModel.FaxNumber = model.FaxNumber;
            rModel.PhoneNumber = model.PhoneNumber;
            rModel.Id = model.Id;
            rModel.OrganizationLogo = model.OrganizationLogo;
           // rModel.LoginId = model.LoginId;
            rModel.TimeZoneString = model.TimezoneId;
            rModel.IsApproved = model.IsApproved;
            return rModel;
        }
    }
    public class UnregisteredClinicViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public int Country { get; set; }
        public int State { get; set; }
        public int City { get; set; }
        public string PostCode { get; set; }
        public string FaxNumber { get; set; }
        public string PhoneNumber { get; set; }
        public int ClinicType { get; set; }
        public string ClinicLogo { get; set; }
        public bool IsActive { get; set; }
        public int OrganizationId { get; set; }
        public string TimeZone { get; set; }
        public string TimeZone1 { get; set; }
        public int? Titleid { get; set; }
        public int? Genderid { get; set; }
        public string OrganizationName { get; set; }
        public string CliniAdminName { get; set; }
        public string CityName { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string CountryName { get; set; }
        public string addedby { get; set; }
       
        public string PKIMedicareCertificateNo { get; set; }
        public ClinicUsers ClinicUserModel { get; set; }
        public List<TimeZones> TimeZoneList { get; set; }
        public List<LookUpTitle> TitleList { get; set; }
        public List<LookUpCountry> Countries { get; set; }
        public List<LookUpState> States { get; set; }
        public List<LookUpCity> Cities { get; set; }
        public ClinicUserViewModel ClinicUser { get; set; }
        public PaymentDetail Paymentdetail { get; set; }
        public List<ClinicUserViewModel> ClinicUserList { get; set; }
        public List<Organization> OrganizationList { get; set; }
        public List<ProfessionTypes> ProfessionTypesList { get; set; }
        public List<ProfessionSubType> ProfessionSubList { get; set; }
        public int ProfessionTypeId { get; set; }
        public int ProfessionSub { get; set; }
        public int RestrictedProfessionId { get; set; }
        public string AppointmentDateTime { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public UnregisteredClinicViewModel() { }
        public UnregisteredClinicViewModel(UnRegisteredClinic model)
        {

            this.Name = model.Name;
            //this.ClinicType = model.ClinicType;
            this.AddressLine1 = model.AddressLine1;
            this.AddressLine2 = model.AddressLine2;
            this.Country = (int)model.Country;
            this.State = (int)model.State;
            this.City = (int)model.City;
            this.PostCode = model.PostCode;
            this.FaxNumber = model.FaxNumber;
            this.PhoneNumber = model.PhoneNumber;
            this.Id = model.Id;
            this.ClinicLogo = model.ClinicLogo;
            //this.LoginId = model.LoginId;
            this.IsActive = model.IsActive;
            this.TimeZone = model.TimeZoneString;
            this.OrganizationId = model.OrgId;
            this.PKIMedicareCertificateNo = model.PKIMedicareCertificateNo;

        }
        public UnRegisteredClinic GetEditModel(UnregisteredClinicViewModel model)
        {
            UnRegisteredClinic rModel = new UnRegisteredClinic();
            rModel.Name = StringCipher.Encrypt(model.Name);
            rModel.ClinicType = model.ClinicType;
            rModel.AddressLine1 = StringCipher.Encrypt(model.AddressLine1);
            rModel.AddressLine2 = StringCipher.Encrypt(model.AddressLine2);
            rModel.Country = model.Country;
            rModel.State = model.State;
            rModel.City = model.City;
            rModel.PostCode = StringCipher.Encrypt(model.PostCode);
            rModel.FaxNumber = StringCipher.Encrypt(model.FaxNumber);
            rModel.PhoneNumber = StringCipher.Encrypt(model.PhoneNumber);
            rModel.Id = model.Id;
            rModel.ClinicLogo = model.ClinicLogo;
            //rModel.LoginId = model.LoginId;
            rModel.TimeZoneString = model.TimeZone;
            rModel.OrgId = model.OrganizationId;
            rModel.PKIMedicareCertificateNo = StringCipher.Encrypt(model.PKIMedicareCertificateNo);
            return rModel;
        }
    }

}