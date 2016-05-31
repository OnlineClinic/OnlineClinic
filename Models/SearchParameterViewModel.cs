using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyOnlineClinic.Entity;
using MyOnlineClinic.RepositoryService;

namespace MyOnlineClinic.Web.Models
{
    public class SearchParameterViewModel
    {
        private readonly ICountryService _countryService;
        private readonly IStateService _stateService;
        private readonly ICityService _cityService;
        private readonly IOrganizationTypeService _organizationTypeService;
        public string OrganizationName { get; set; }
        public string ClinicName { get; set; }
        public string PostCode { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public int OrgType { get; set; }
        public int Country { get; set; }
        public int State { get; set; }
        public int City { get; set; }
        public int ProfessionType { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string AreaName { get; set; }

        public bool ShowOrganizationName { get; set; }
        public bool ShowCliniName { get; set; }
        public bool ShowEmail { get; set; }
        public bool ShowPostCode { get; set; }
        public bool ShowUserName { get; set; }
        public bool ShowCountry { get; set; }
        public bool ShowState { get; set; }
        public bool ShowCity { get; set; }
        public bool ShowOrgType { get; set; }
        public bool ShowProfessionType { get; set; }

        public List<LookUpCountry> CountryList { get; set; }
        public List<LookUpState> StateList { get; set; }
        public List<LookUpCity> CityList { get; set; }
        public List<OrganizationTypes> OrganizationTypeses { get; set; }

        public SearchParameterViewModel(ICountryService countryService, 
            IStateService stateService, ICityService cityService,
            IOrganizationTypeService organizationTypeService)
        {
            _countryService = countryService;
            _stateService = stateService;
            _cityService = cityService;
            _organizationTypeService = organizationTypeService;
            this.CountryList = _countryService.GetCountryList();
            this.StateList = _stateService.GetStateList();
            this.CityList = _cityService.GetCityList();
            this.OrganizationTypeses = _organizationTypeService.GetOrganizarionTypesList();
        }
        public SearchParameterViewModel()
        {
        }
    }
}