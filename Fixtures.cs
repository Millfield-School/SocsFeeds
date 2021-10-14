using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using SocsFeeds.objects;

namespace SocsFeeds
{
    public class Fixtures :IDisposable
    {

       
        public List<objects.Fixtures> GetFixtureDetails(DateTime StartDate, DateTime FinishDate, int SchoolID, string APIKey)
        {
            
            
            string SOCSURL = "https://www.schoolssports.com/school/xml/fixturecalendar.ashx?ID=" + SchoolID + "&key=" + APIKey + "&TS=1";
            List<objects.Fixtures> sc = new List<objects.Fixtures>();
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(SOCSURL + "&startdate=" + StartDate.ToLongDateString() + "&enddate=" + FinishDate.ToLongDateString());

            XmlNodeList clubnode = xmlDocument.SelectNodes("fixtures");
            foreach (XmlNode i in clubnode)
            {
                foreach (XmlNode ii in i.ChildNodes)
                {
                    objects.Fixtures c = new objects.Fixtures();
                    c.EventID = Convert.ToInt32(ii["eventid"].InnerText);
                    c.Sport = ii["sport"].InnerText;
                    c.FixtureDate = Convert.ToDateTime(ii["date"].InnerText);
                    if (ii["meettime"] != null)
                        c.MeetTime = ii["meettime"].InnerText;
                    c.FixtureTime = ii["time"].InnerText;
                    if (ii["returntime"] != null)
                        c.ReturnTime = ii["returntime"].InnerText;
                    c.TeamName = ii["team"].InnerText;
                    c.Opposition = ii["opposition"].InnerText;
                    c.Oppositionteam = ii["oppositionteam"].InnerText;
                    if (ii["location"] != null)
                        c.Location = ii["location"].InnerText;
                    if (ii["transport"] != null)
                        c.Transport = ii["transport"].InnerText;
                    if (ii["details"] != null)
                        c.Details = ii["details"].InnerText;
                    if (ii["url"] != null)
                        c.URL = ii["url"].InnerText;
                    c.StartDateTimeFull = Convert.ToDateTime(ii["startdatefull"].InnerText);
                    if (!string.IsNullOrEmpty(ii["enddatefull"].InnerText))
                        c.EndDateTimeFull = Convert.ToDateTime(ii["enddatefull"].InnerText);
                    if (ii["pupils"] != null)
                    {
                        List<string> pupilid = ii["pupils"].InnerText.Split(',').ToList<string>();
                        c.PupilsList = pupilid;
                    }

                    if (ii["staff"] != null)
                    {
                        List<string> staffid = ii["staff"].InnerText.Split(',').ToList<string>();
                        c.StaffList = staffid;
                    }


                    sc.Add(c);
                }

            }

            return sc;
        }

        public void Dispose()
        {
            
        }
    }
}
