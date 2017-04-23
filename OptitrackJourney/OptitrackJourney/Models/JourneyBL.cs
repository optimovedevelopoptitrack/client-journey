using OptitrackJourney.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Web.Script.Serialization;

namespace OptitrackJourney.Models
{
    public class JourneyBL : IJourneyBL
    {
        private JourneyDAL _dal;
       
        public JourneyBL()
        {
            _dal = new JourneyDAL(ConfigurationManager.ConnectionStrings["piwikConnectionString"].ConnectionString);
        }

        public VisitorJourney ExtractVisitorJourney(string visitorId)
        {
            int tenantId = Convert.ToInt32(ConfigurationManager.AppSettings["optimoveSiteId"].ToString());
            DataTable VisitTable = _dal.GetVisitorJourney(visitorId, tenantId);
            var sessions = VisitTable.AsEnumerable().OrderBy(row => row["EventId"])
                                                    .GroupBy(row => row["VisitId"])
                                                   .ToList();
            VisitorJourney visitJourney = new VisitorJourney();
            HashSet<string> pagesSet = new HashSet<string>();
            var totalVisits = 0;
            foreach (var session in sessions)
            {
                totalVisits = Math.Max(totalVisits,Convert.ToInt32(session.AsEnumerable().First()["VisitCounter"].ToString()));
                Visit visit = new Visit()
                {
                    Source = session.AsEnumerable().First()["Source"].ToString(),
                    SourceUrl = session.AsEnumerable().First()["SourceUrl"].ToString(),
                    Duration = (Convert.ToInt32(session.AsEnumerable().First()["SessionTotalTime"])*1.0) /60.0 ,
                    FirstActionTime = Convert.ToDateTime(session.AsEnumerable().First()["FirstActionTime"]),
                    CampaignName = session.AsEnumerable().First()["CampaignName"].ToString(),
                    CampaignSource = session.AsEnumerable().First()["CampaignSource"].ToString(),
                    CampaignMedium = session.AsEnumerable().First()["CampaignMedium"].ToString(),
                    CampaignTerm = session.AsEnumerable().First()["CampaignTerm"].ToString(),
                    CampaignContent = session.AsEnumerable().First()["CampaignContent"].ToString(),
                };
                int pageCounter = 1;
                foreach (DataRow pageVisit in session)
                {
                    var pageURL = pageVisit["UrlName"].ToString();
                    var pageRefURL = pageVisit["UrlRefName"].ToString();

                    if ( !Convert.IsDBNull(pageURL) && !pagesSet.Contains(pageURL))
                        pagesSet.Add(pageURL);
                    if ( !Convert.IsDBNull(pageRefURL) &&  !pagesSet.Contains(pageRefURL))
                        pagesSet.Add(pageRefURL);

                    if (pageCounter != 1)
                    {
                        PageView pv = new PageView();
                        pv.PageUrl = pageVisit["UrlRefName"].ToString();
                        pv.Duration = Convert.ToInt32(pageVisit["TimeSpentRefAction"]) * 1.0 / 60.0;
                        visit.PageViews.Add(pv);
                    }
                    

                    if (pageCounter == session.Count())//last page
                    {//last page of the session
                        PageView lastPageView = new PageView();
                        lastPageView.PageUrl = pageVisit["UrlName"].ToString();
                        lastPageView.Duration = -1; //no data regarding the duration of the last page
                        visit.PageViews.Add(lastPageView);
                    }

                    pageCounter++;
                }
                visitJourney.Sessions.Add(visit);
            }
            visitJourney.TotalVisits = totalVisits;
            visitJourney.UniquePagesViewed = pagesSet.Count;
            visitJourney.TotalPagesViewed = VisitTable.Rows.Count;
            return visitJourney;
            
        }

        public string PrepareHTML(VisitorJourney visitorJourney)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div>");
            sb.Append("<h1 style=line-height:0px;>VISIT HISTORY SUMMARY</h1>");
            sb.Append("<hr style=width:440px;float:left;>");
            sb.Append("<br><br>");
            sb.AppendFormat("Total visits: {0}", visitorJourney.TotalVisits);
            sb.Append("<br>");
            sb.AppendFormat("Total pages viewed: {0}", visitorJourney.TotalPagesViewed);
            sb.Append("<br>");
            sb.AppendFormat("Unique pages viewed: {0}", visitorJourney.UniquePagesViewed);
            sb.Append("<br>");
            sb.Append("<h2 style=line-height:0px;>Visit Log</h2>");
            sb.Append("<hr style=width:100px;float:left;>");
            sb.Append("<br>");
            sb.Append("<div style=margin-left:50px;>");
            int visitNum = 1;
            foreach (var session in visitorJourney.Sessions)
            {
                sb.AppendFormat("<h3 style=margin-top:0px;margin-bottom:0px;display:inline-block;>Visit {0}</h3><span> ({1}):</span>", visitNum, session.FirstActionTime);
                sb.Append("<div style=margin-left:60px;>");
                sb.AppendFormat("<div>Source: {0}</div>",session.Source);
                sb.AppendFormat("<div>Source URL: {0}</div>",session.SourceUrl);
                if (session.Duration == 0)
                    sb.AppendFormat("<div>Visit duration: N/A.</div>", session.Duration);
                else
                    sb.AppendFormat("<div>Visit duration: {0} Min.</div>", session.Duration);
                sb.Append("<div>Pages viewed:</div>");
                sb.Append("<ol style=padding-top:0px;margin-top: 0px;margin-bottom: 0px;margin-left:40px;>");
                foreach (var page in session.PageViews)
                {
                    if (page.Duration > 0)
                        sb.AppendFormat("<li>{0}({1} Mins.)</li>", page.PageUrl, page.Duration);
                    else
                        sb.AppendFormat("<li>{0}({1}.)</li>",page.PageUrl,"N/A");
                }
                sb.Append("</ol>");
                sb.Append("<br>");
                sb.Append("</div>");
                visitNum++;
            }
            sb.Append("</div>");
            sb.Append("</div>");
            return sb.ToString();
        }

        public string PrepareJson(VisitorJourney visitorJourney)
        {
            
            JavaScriptSerializer aaa = new JavaScriptSerializer();
            string ttt = aaa.Serialize(visitorJourney);
            return ttt;
            //MemoryStream stream1 = new MemoryStream();
            //DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(VisitorJourney));
            //ser.WriteObject(stream1, visitorJourney);
            //stream1.Position = 0;
            //StreamReader sr = new StreamReader(stream1);
            //Console.Write("JSON form of Person object: ");
            //var json = sr.ReadToEnd();
            
            
            //return Json(;
        }
    }
}