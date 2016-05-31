using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyOnlineClinic.Entity;
using MyOnlineClinic.RepositoryService;
using MyOnlineClinic.Web.Models;
using MyOnlineClinic.Web.Controllers;
using System.IO;
using System.Drawing;
using MyOnlineClinic.Web.Helper;
using MyOnlineClinic.Emailer;
using System.Configuration;
using System.Collections.ObjectModel;
namespace MyOnlineClinic.Web.Models
{
    public class SearchParametersViewModel
    {
        public string OrgName { get; set; }
        public string ClinicName { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public string DeviceName { get; set; }
        public string DeviceNumber { get; set; }
        public DateTime AssignDate { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public string PostCode { get; set; }
        public string SearchDate { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string AreaName { get; set; }
        public string ClinicAdminName { get; set; }
        public string OrganizationName { get; set; }
        public string RedirectToAction { get; set; }
        public int Orguser { get; set; }
        public string AdminName { get; set; }
        public string StaffName { get; set; }
        public int OrgType { get; set; }
        public int OrgAdmin { get; set; }
        public int ClinicAdmin { get; set; }
        public int Role { get; set; }
        public int? ActivationStatus { get; set; }
        public string Email { get; set; }
        public string PageTitle { get; set; }
        public List<LookUpCity> CityList { get; set; }
        public List<LookUpState> StateList { get; set; }
        public List<LookUpCountry> CountryList { get; set; }
        public List<OrganizationTypes> OrganizationTypeList { get; set; }
        public int AppointmentStatus { get; set; }
        public int AppointmentType { get; set; }
        // custom field for voucher
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Voucherstatus { get; set; }
        public int? UsedId { get; set; }
        public int? GeneratedBy { get; set; }
        public bool ShowClinicName { get; set; }
        public bool ShowOrganizationName { get; set; }
        public bool ShowEmail { get; set; }
        public bool ShowOrgType { get; set; }
        public bool ShowCountry { get; set; }
        public bool ShowState { get; set; }
        public bool ShowCity { get; set; }
        public bool ShowPostCode { get; set; }
        public bool ShowSearchDate { get; set; }
        public bool ShowOrgadmin { get; set; }
        public bool ShowClinicadmin { get; set; }
        public bool ShowDoctorName { get; set; }
        public bool ShowPatientName { get; set; }
        public bool ShowActiveStatus { get; set; }
        public bool ShowDeviceName { get; set; }
        public bool ShowDeviceNumber { get; set; }
        public bool ShowAssignDate { get; set; }
        public bool ShowStaffName { get; set; }
        public bool ShowRole { get; set; }
        public bool ShowOrguser { get; set; }

        public bool ShowFromDate { get; set; }
        public bool ShowToDate { get; set; }

        public bool ShowAppointmentStatus { get; set; }
        public bool ShowAppointmentType { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// 
        private readonly ICountryService _countryService;
        private readonly IStateService _stateService;
        private readonly ICityService _cityService;
        private readonly IOrganizationTypeService _organizationTypesService;
        public SearchParametersViewModel()
        {
            CountryId = 0;

            _cityService = new CityService();
            _countryService = new CountryService();
            _stateService = new StateService();
            _organizationTypesService = new OrganizationTypeService();

            //fill dropdownlist
            CityList = _cityService.GetCityList();
            for (int i = 0; i < CityList.Count;i++)
            {
                CityList[i].LookUpCityName = StringCipher.Decrypt(CityList[i].LookUpCityName);
            }
             StateList = _stateService.GetStateList();
             for (int i = 0; i < StateList.Count; i++)
             {
                 StateList[i].StateName = StringCipher.Decrypt(StateList[i].StateName);
             }
            CountryList = _countryService.GetCountryList();
            for (int i = 0; i < CountryList.Count; i++)
            {
                CountryList[i].CountryName = StringCipher.Decrypt(CountryList[i].CountryName);
            }
            OrganizationTypeList = _organizationTypesService.GetOrganizarionTypesList();
            for(int i=0;i<OrganizationTypeList.Count;i++)
            {
                OrganizationTypeList[i].OrganizationTypeName = StringCipher.Decrypt(OrganizationTypeList[i].OrganizationTypeName);
                }
           
        }

    }
}