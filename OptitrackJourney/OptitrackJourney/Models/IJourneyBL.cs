using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptitrackJourney.Models
{
    interface IJourneyBL
    {
        VisitorJourney ExtractVisitorJourney(string visitorId);
        string PrepareHTML(VisitorJourney visitorJourney);
        string PrepareJson(VisitorJourney visitorJourney);
    }
}
