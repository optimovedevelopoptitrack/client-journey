using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace OptitrackJourney.Models
{
     [DataContract]
    public class Visit
    {
          [DataMember]
        public DateTime FirstActionTime { get; set; }

          [DataMember]
        public string Source { get; set; }

          [DataMember]
        public string SourceUrl { get; set; }

          [DataMember]
        public double Duration { get; set; }

          [DataMember]
        public string CampaignName { get; set; }

        [DataMember]
        public string CampaignSource { get; set; }

          [DataMember]
        public string CampaignMedium { get; set; }

          [DataMember]
        public string CampaignTerm { get; set; }

          [DataMember]
        public string CampaignContent { get; set; }

          [DataMember]
        public List<PageView> PageViews { get; set; }

        public Visit()
        {
            PageViews = new List<PageView>();
        }
    }
}