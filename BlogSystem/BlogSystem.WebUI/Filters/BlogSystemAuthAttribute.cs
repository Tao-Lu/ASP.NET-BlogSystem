using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BlogSystem.WebUI.Filters
{
    public class BlogSystemAuthAttribute: AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            // sync data in cookie to session
            // do not need to check data is in cookie or session
            // get data from session all the time
            if(filterContext.HttpContext.Request.Cookies["loginEmail"] != null && filterContext.HttpContext.Session["loginEmail"] == null)
            {
                filterContext.HttpContext.Session["loginEmail"] = filterContext.HttpContext.Request.Cookies["loginEmail"].Value;
                filterContext.HttpContext.Session["loginUserId"] = filterContext.HttpContext.Request.Cookies["loginUserId"].Value;
            }
            
            
            // base.OnAuthorization(filterContext);
            if (!(filterContext.HttpContext.Session["loginEmail"] != null || filterContext.HttpContext.Request.Cookies["loginEmail"] != null))
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary()
                {
                    {"controller", "Home" },
                    {"action", "Login" }
                });
            }
        }
    }
}