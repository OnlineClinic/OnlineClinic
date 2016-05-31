using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyOnlineClinic.Web.Helper
{
    public static class SecurityCheck
    {
        public static bool ActionIsAuthorized(string actionName, string controllerName)
        {
            IControllerFactory factory = ControllerBuilder.Current.GetControllerFactory();
            ControllerBase controller = factory.CreateController(HttpContext.Current.Request.RequestContext, controllerName) as ControllerBase;
            var controllerContext = new ControllerContext(HttpContext.Current.Request.RequestContext, controller);
            var controllerDescriptor = new ReflectedControllerDescriptor(controller.GetType());
            var actionDescriptor = controllerDescriptor.FindAction(controllerContext, actionName);
            AuthorizationContext authContext = new AuthorizationContext(controllerContext, actionDescriptor);
            foreach (var authAttribute in actionDescriptor.GetFilterAttributes(true).Where(a => a is AuthorizeAttribute).Select(a => a as AuthorizeAttribute))
            {
                authAttribute.OnAuthorization(authContext);
                if (authContext.Result != null)
                    return false;
            }
            return true;
        }
    }
}