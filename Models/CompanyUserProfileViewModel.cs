using MyOnlineClinic.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;

namespace MyOnlineClinic.Web.Models
{
    public class CompanyUserProfileViewModel
    {
        public Guid LoginId { get; set; }
        
        public string LoginName { get; set; }

        public string NIPC { get; set; }

        public int IndustryId { get; set; }

        public int SectorId { get; set; }

        public int? LookUpStateId { get; set; }

        public string MemberName { get; set; }

        public int MemberNumber { get; set; }

        public string CompanyName { get; set; }

        public string Address { get; set; }

        public string PostalCode { get; set; }

        public string PostalLocal { get; set; }

        public int? CountryId { get; set; }

        public short? DistritosId { get; set; }

        public int? ConcelhosId { get; set; }

        public string EmailId { get; set; }

        public string website { get; set; }

        public string telephone1 { get; set; }

        public string telephone2 { get; set; }

        public string Fax { get; set; }

        public RoleType RoleId { get; set; }

        public string Description { get; set; }

        public string Avatar { get; set; }
        public string Denominacao { get; set; }

        public string Morada { get; set; }

        public string Localidade { get; set; }

        public string Actividade { get; set; }

        public string Email { get; set; }

        public string Email2 { get; set; }

        public string Url { get; set; }

        public int? DId { get; set; }

        public int? CId { get; set; }

        public int? FId { get; set; }

        public string Cae { get; set; }

        public int? EmpresaActId { get; set; }

        public string FormaJuridica { get; set; }

        public decimal? Capital { get; set; }

        public string DataConst { get; set; }

        public string PrincipalGestor { get; set; }

        public string Funcao { get; set; }

        public string Descricao { get; set; }

        public string CompanyLogo { get; set; }

        public int LookUpCompanyId { get; set; }

       

        public List<LookUpCity> CityList { get; set; }



        public CompanyUserProfileViewModel()
        {

        }

      
    }


    public class CompanyEditViewModel
    {
       
        public int LookUpCompanyId { get; set; }
        public Guid LoginId { get; set; }
        public string NIPC { get; set; }
        public bool IsFeatured { get; set; }
        public int UserNumber { get; set; }

        [DisplayName("Empresa")]
        public string LookUpCompanyName { get; set; }

        public string SlugTitle { get; set; }

        public string Denominacao { get; set; }

        public string Morada { get; set; }

        public string Localidade { get; set; }

        public string CodPostal { get; set; }

        public string Telefone { get; set; }

        public string Telefone2 { get; set; }

        public string Fax { get; set; }

        public string Email { get; set; }

        public string Email2 { get; set; }

        public string Url { get; set; }

        public int? DId { get; set; }

        public int? CId { get; set; }

        public int? FId { get; set; }

        public string Cae { get; set; }

        public int? EmpresaActId { get; set; }

        public string Actividade { get; set; }

        public short? SectorId { get; set; }

        public string FormaJuridica { get; set; }

        public decimal Capital { get; set; }

        public string DataConst { get; set; }

        public string PrincipalGestor { get; set; }

        public string Funcao { get; set; }

        public string Descricao { get; set; }

        public string CompanyLogo { get; set; }

        public int? LookUpStateId { get; set; }

        public string UserName { get; set; }

       

        public string Avatar { get; set; }
        public bool Active { get; set; }

        public int? CountryId { get; set; }

        public CompanyEditViewModel()
        { }

        
    }
}