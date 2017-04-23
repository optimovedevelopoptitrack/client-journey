using OptitrackJourney.Infrastructure;
using OptitrackJourney.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Http.Cors;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;


namespace OptitrackJourney.Controllers
{
    [EnableCors(origins: "http://staging.optimove.com, http://www.optimove.com", headers: "*", methods: "*")]
   
    public class JourneyController : ApiController
    {
        private IJourneyBL bl;
        
        public JourneyController()
        {
            bl = new JourneyBL();
        }


        // Usage: http://localhost:51184/api/Journey
        // Request Header: token: 7E4C3202-4574-4CA9-8C91-49ADD27E8F9E 
       [AuthenticationFilter]
        public ResponseAjax Post([FromBody] string visitorId)
        {
            var toRet = new ResponseAjax();
            try
            {
                VisitorJourney visitorJourney = bl.ExtractVisitorJourney(visitorId);
                toRet.Data = bl.PrepareHTML(visitorJourney);
                toRet.IsSuccess = true;
            }
            catch (Exception ex)
            {
                toRet.ErrorMsg = ex.Message;
                toRet.IsSuccess = false;
            }

            return toRet;
        }


       // Usage: http://localhost:51184/api/Journey/D8C0CAC395C32740
       // Request Header: token: 7E4C3202-4574-4CA9-8C91-49ADD27E8F9E 
       [AuthenticationFilter]
       public HttpResponseMessage Get([FromUri] string visitorId)
       {
         
           VisitorJourney visitorJourney = null;
           
           IContentNegotiator negotiator = this.Configuration.Services.GetContentNegotiator();
           ContentNegotiationResult result = negotiator.Negotiate(typeof(VisitorJourney), this.Request, this.Configuration.Formatters);
            if (result == null)
            {
                var response = new HttpResponseMessage(HttpStatusCode.NotAcceptable);
                throw new HttpResponseException(response);
            }


            try
            {             
          
               visitorJourney = bl.ExtractVisitorJourney(visitorId);

               var response = new HttpResponseMessage()
               {
                   Content = new ObjectContent<VisitorJourney>(
                       visitorJourney,		        // What we are serializing 
                       result.Formatter,           // The media formatter
                       result.MediaType.MediaType  // The MIME type
                   )
               };
               response.StatusCode = HttpStatusCode.OK;
               return response;
              

           
               
           }
           catch (Exception ex)
           {
             
                var responseErr =  new HttpResponseMessage()
               {
                   Content = new ObjectContent<VisitorJourney>(
                       null,		        // What we are serializing 
                       result.Formatter,           // The media formatter
                       result.MediaType.MediaType  // The MIME type                       
                   )

               };

                responseErr.StatusCode = HttpStatusCode.InternalServerError;
                return responseErr;
               
           }


           return null;
       }


       // Usage: http://localhost:51184/api/Journey/D8C0CAC395C32740/html
       // Request Header: token: 7E4C3202-4574-4CA9-8C91-49ADD27E8F9E
       [AuthenticationFilter]
       public ResponseAjax Get([FromUri] string visitorId, string format)
       {
           var toRet = new ResponseAjax();
           try
           {
               VisitorJourney visitorJourney = bl.ExtractVisitorJourney(visitorId);


               if (format.Equals("html") == true)
               {
                   toRet.Data = bl.PrepareHTML(visitorJourney);
               }
               else 
               {
                   toRet.Data = visitorJourney;
               }
              
               toRet.IsSuccess = true;              
           }
           catch (Exception ex)
           {
               toRet.ErrorMsg = ex.Message;
               toRet.IsSuccess = false;
           }

           return toRet;
       }


          
    }


}
