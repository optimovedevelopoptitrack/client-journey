using OptitrackJourney.Infrastructure;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace OptitrackJourney
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterHttpFilters(System.Web.Http.Filters.HttpFilterCollection filters)
        {
            filters.Add(new AuthenticationFilter());
        }
    }
}