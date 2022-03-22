using parity_mvc_intro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace parity_mvc_intro.Filters
{
    public class RequiresUserValidation : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var ctx = context.HttpContext;

            // Return to log in because theres no active user
            if (ctx.Session["ActiveUserId"] == null)
                context.Result = new RedirectToRouteResult(new RouteValueDictionary{ 
                    { "controller", "Home"}, 
                    { "action", "Index" } 
                });
        }
    }
}