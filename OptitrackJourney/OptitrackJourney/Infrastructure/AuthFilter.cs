using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.Results;
using WebApi.AuthenticationFilter;

namespace OptitrackJourney.Infrastructure
{
    public class AuthenticationFilter : AuthenticationFilterAttribute
    {
        public override void OnAuthentication(HttpAuthenticationContext context)
        {
            if (!Authenticate(context))
                context.ErrorResult = new StatusCodeResult(HttpStatusCode.Unauthorized, context.Request);
        }
        

        private bool Authenticate(HttpAuthenticationContext context)
        {
            if (!context.Request.Headers.Contains("token"))
                return false;
            var headersToken = context.Request.Headers.GetValues("token").FirstOrDefault();
            var token = ConfigurationManager.AppSettings["token"].ToString();
            var authentication = token == headersToken;
            return authentication;
        }
    }
}

