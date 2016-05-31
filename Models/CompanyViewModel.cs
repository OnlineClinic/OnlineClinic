using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyOnlineClinic.Web.Models
{
    public class CompanyViewModel
    {
        public int LookUpCompanyId { get; set; }
        public string LookUpCompanyName { get; set; }
        public string Morada { get; set; }
        public string CountryName { get; set; }
        public string DistritoName { get; set; }
        public string ConcelhoName { get; set; }
        public string FregusiaName { get; set; }
        public string Capital { get; set; }
        public string CodPostal { get; set; }
        public string Telefone { get; set; }
        public string Fax { get; set; }
        public string Descricao { get; set; }
        public string Denominacao { get; set; }
        public string CompanyLogo { get; set; }
        public string Email { get; set; }
        public string WebSite { get; set; }
        public string NIPC { get; set; }
        public string Localidade { get; set; }
        
        public int TotalRows { get; set; }
        public bool IsActive { get; set; }
        public bool IsFeatured { get; set; }
    }
}