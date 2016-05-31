using MyOnlineClinic.Migrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace MyOnlineClinic.Web
{
    public class AuthorizeRole : AuthorizeAttribute
    {
        EntitiesDB context = new EntitiesDB(); // my entity  
        private readonly string[] allowedroles;
        public AuthorizeRole(params string[] roles)
        {
            this.allowedroles = roles;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            foreach (var role in allowedroles)
            {
                HttpCookie authCookie = httpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
                FormsAuthenticationTicket ticket = authCookie == null ? null : FormsAuthentication.Decrypt(authCookie.Value);
                if (ticket != null)
                {
                    string[] userNameAndRole = ticket.UserData.Split(':');
                    string[] userRoles = userNameAndRole[1].Split(',');
                    foreach (var item in userRoles)
                    {
                        if (this.allowedroles.Contains(item))
                        {
                            authorize = true; /* return true if Entity has current user(active) with specific role */
                            return authorize;
                        }
                    }                    
                }
            }
            return authorize;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //// Returns HTTP 401 - see comment in HttpUnauthorizedResult.cs.
            filterContext.Result = new RedirectToRouteResult(
                                       new RouteValueDictionary 
                                   {
                                       { "action", "Unauthorized" },
                                       { "controller", "Home" },
                                       {"area",null}
                                   });
        }
    }
}