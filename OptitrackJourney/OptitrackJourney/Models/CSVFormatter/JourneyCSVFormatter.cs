using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace OptitrackJourney.Models.CSVFormatter
{
    public class JourneyCSVFormatter : BufferedMediaTypeFormatter
    {
        public JourneyCSVFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/csv"));

            SupportedEncodings.Add(new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
            SupportedEncodings.Add(Encoding.GetEncoding("iso-8859-1"));
        }


        public override bool CanWriteType(System.Type type)
        {
            if (type == typeof(VisitorJourney))
            {
                return true;
            }
            else
            {
                Type enumerableType = typeof(IEnumerable<VisitorJourney>);
                return enumerableType.IsAssignableFrom(type);
            }
        }

        public override bool CanReadType(Type type)
        {
            return false;
        }


        public override void WriteToStream(Type type, object value, Stream writeStream, HttpContent content)
        {
            using (var writer = new StreamWriter(writeStream))
            {
                var visitorJourney = value as VisitorJourney;
                var sessions = visitorJourney.Sessions;

                if (sessions != null)
                {
                    WriteItem(sessions, writer);
                }
                else
                {
                    var singleProduct = value as VisitorJourney;
                    if (singleProduct == null)
                    {
                        throw new InvalidOperationException("Cannot serialize type");
                    }
                   
                }
            }
        }

        private void WriteItem(List<Visit> session, StreamWriter writer)
        {

            foreach (var visit in session)
            {
                writer.WriteLine("{0},{1},{2},{3},{4}", Escape(visit.Source), Escape(visit.SourceUrl), Escape(visit.PageViews), Escape(visit.FirstActionTime), Escape(visit.Duration));
            }
            
        }


        private string Escape(object o)
        {
            if (o == null)
            {
                return "";
            }
            string field = o.ToString();
            if (field.IndexOfAny(_specialChars) != -1)
            {
                // Delimit the entire field with quotes and replace embedded quotes with "".
                return String.Format("\"{0}\"", field.Replace("\"", "\"\""));
            }
            else return field;
        }

        static char[] _specialChars = new char[] { ',', '\n', '\r', '"' };
    }
}