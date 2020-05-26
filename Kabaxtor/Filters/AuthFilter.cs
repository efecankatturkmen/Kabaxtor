using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kabaxtor.Filters
{
    public class AuthFilter : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Session["customer"] == null)
            {
                filterContext.Result = new RedirectResult("/Home/Index");
            }
            //if ()
            //{

            //}

        }
    }
}