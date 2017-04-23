using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace OptitrackJourney.Models
{
    [DataContract]
    public class VisitorJourney
    {
        [DataMember]
        public int TotalVisits { get; set; }
        [DataMember]
        public int TotalPagesViewed { get; set; }
        [DataMember]
        public int UniquePagesViewed { get; set; }

        [DataMember]
        public List<Visit> Sessions { get; set; }

        public VisitorJourney()
        {
            Sessions = new List<Visit>();
        }
    }
}