using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using SocsFeeds.objects;

namespace SocsFeeds
{
    public class Events :IDisposable
    {
       
        public  List<objects.EventAttendance> GetEventAttendance(DateTime EventDate, int SchoolID, string APIKey)
        {
            string SOCSURL = "https://www.socscms.com/socs/xml/cocurricular.ashx?ID=" + SchoolID + "&key=" + APIKey + "&data=";
           
            List<EventAttendance> att = new List<EventAttendance>();
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(SOCSURL + "registers&startdate=" + EventDate.ToLongDateString());
            XmlNodeList xmlNodeList = xmlDocument.SelectNodes("registers");
            foreach (XmlNode reg in xmlNodeList)
            {
                foreach (XmlNode p in reg.ChildNodes)
                {
                    EventAttendance a = new EventAttendance();
                    a.eventid = Convert.ToInt32(p["eventid"].InnerText);
                    a.pupilid = p["pupilid"].InnerText;
                    var at = p.SelectSingleNode("attendance");
                    if (at != null)
                        a.attendance = p["attendance"].InnerText;
                    att.Add(a);
                }

            }

            return att;
        }

        public List<objects.Events> GetEventDetails(DateTime EventDate, int SchoolID, string APIKey)
        {
            string SOCSURL = "https://www.socscms.com/socs/xml/cocurricular.ashx?ID=" + SchoolID + "&key=" + APIKey + "&data=";
           
            List<objects.Events> evn = new List<objects.Events>();
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(SOCSURL + "events&startdate=" + EventDate.ToLongDateString());
            Console.WriteLine(SOCSURL + "events&startdate=" + EventDate.ToLongDateString());
            XmlNodeList xmlNodeList = xmlDocument.SelectNodes("events");
            foreach (XmlNode evt in xmlNodeList)
            {
                foreach (XmlNode p in evt.ChildNodes)
                {
                    objects.Events e = new objects.Events();
                    e.clubid = Convert.ToInt32(p["clubid"].InnerText);
                    e.eventid = Convert.ToInt32(p["eventid"].InnerText);
                    e.EventTitle = p["title"].InnerText;
                    e.Location = p["location"].InnerText;
                    e.StartDate = Convert.ToDateTime(p["startdate"].InnerText);
                    e.StartTime = p["starttime"].InnerText;
                    e.EndTime = p["endtime"].InnerText;
                    e.AlldayEvent = Convert.ToBoolean(p["alldayevent"].InnerText);
                    e.RecurringID = p["recurringid"].InnerText;
                    var pupils = p.SelectSingleNode("pupils");
                    if (pupils != null)
                    {
                        string txtSchoolID = p["pupils"].InnerText;
                        //convert string to list
                        List<string> s = txtSchoolID.Split(',').ToList();
                        e.pupilID = s;
                    }
                    evn.Add(e);
                }

            }

            return evn;
        }

        public void Dispose()
        {
            
        }
    }
}
