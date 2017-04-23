using OptitrackJourney.Models.CSVFormatter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace OptitrackJourney
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            config.EnableCors();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{visitorId}",
                defaults: new { visitorId = -1 }
            );

            config.Routes.MapHttpRoute(
               name: "Default2Api",
               routeTemplate: "api/{controller}/{visitorId}/{format}",
               defaults: new { visitorId = -1, format = "json" }
           );


            var formatters = GlobalConfiguration.Configuration.Formatters;
            config.Formatters.Add(new JourneyCSVFormatter()); 

           // formatters.Remove(formatters.JsonFormatter);

            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();

            // To disable tracing in your application, please comment out or remove the following line of code
            // For more information, refer to: http://www.asp.net/web-api
            config.EnableSystemDiagnosticsTracing();
        }
    }
}
