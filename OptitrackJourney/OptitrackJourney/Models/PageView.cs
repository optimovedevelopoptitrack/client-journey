using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;


namespace OptitrackJourney.Models
{
     [DataContract]
    public class PageView
    {
         [DataMember]
        public string PageUrl { get; set; }
         [DataMember]
        public double Duration { get; set; }
    }
}