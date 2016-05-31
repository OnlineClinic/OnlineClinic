using System.Web.Mvc;

namespace MyOnlineClinic.Web.Areas.Organizations
{
    public class OrganizationsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Organizations";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Organizations_default",
                "Organizations/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}