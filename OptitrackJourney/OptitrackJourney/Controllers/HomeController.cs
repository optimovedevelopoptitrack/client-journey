using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http.Cors;

namespace OptitrackJourney.Controllers
{
     [EnableCors(origins: " http://staging.optimove.com", headers: "*", methods: "*")]
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
          
            return View();
        }

    }
}
