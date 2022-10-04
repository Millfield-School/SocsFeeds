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
            xmlDocument.Load(SOCSURL + "registers&startdate=" + EventDate.ToLongDateString()+ "&enddate="+EventDate.ToLongDateString());
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
            xmlDocument.Load(SOCSURL + "events&startdate=" + EventDate.ToLongDateString() + "&enddate=" + EventDate.ToLongDateString() + "&staff=1");
            Console.WriteLine(SOCSURL + "events&startdate=" + EventDate.ToLongDateString() + "&enddate=" + EventDate.ToLongDateString() + "&staff=1");
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

                    var staff = p.SelectSingleNode("staff");
                    if (staff != null)
                    {
                        string staffID = p["staff"].InnerText;
                        List<string> s = staffID.Split(',').ToList();
                        e.staffID = s;
                    }
                    evn.Add(e);
                }

            }

            return evn;
        }

        public List<objects.Events> GetEventDetails(DateTime StartEventDate,DateTime EndEventDate, int SchoolID, string APIKey)
        {
            string SOCSURL = "https://www.socscms.com/socs/xml/cocurricular.ashx?ID=" + SchoolID + "&key=" + APIKey + "&data=";

            List<objects.Events> evn = new List<objects.Events>();
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(SOCSURL + "events&startdate=" + StartEventDate.ToLongDateString() + "&enddate=" + EndEventDate.ToLongDateString() + "&staff=1");
            Console.WriteLine(SOCSURL + "events&startdate=" + StartEventDate.ToLongDateString() + "&enddate=" + EndEventDate.ToLongDateString() + "&staff=1");
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
                    var staff = p.SelectSingleNode("staff");
                    if (staff != null)
                    {
                        string staffID = p["staff"].InnerText;
                        List<string> s = staffID.Split(',').ToList();
                        e.staffID = s;
                    }
                    evn.Add(e);
                }

            }

            return evn;
        }

        public List<objects.EventAttendance> GetEventAttendance(DateTime StartEventDate,DateTime EndEventDate, int SchoolID, string APIKey)
        {
            string SOCSURL = "https://www.socscms.com/socs/xml/cocurricular.ashx?ID=" + SchoolID + "&key=" + APIKey + "&data=";

            List<EventAttendance> att = new List<EventAttendance>();
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(SOCSURL + "registers&startdate=" + StartEventDate.ToLongDateString() + "&enddate=" + EndEventDate.ToLongDateString());
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

        public List<objects.ActivityAttendence> GetActivityAttendences(string AcademicTerm, string AcademicYear, string APIKey,int SchoolID, string Category)
        {
            string SOCSURL = $"https://www.socscms.com/socs/xml/proactivityabsencereport.ashx?ID={SchoolID}&key={APIKey}&Term={AcademicTerm}&AcademicYear={AcademicYear}" ;
            List<ActivityAttendence> att = new List<ActivityAttendence>();
            XmlDocument xmlDocument = new XmlDocument();
            if(!string.IsNullOrEmpty(Category))
                xmlDocument.Load(SOCSURL+ $"&Category=LIKE:{Category}");
            else
                xmlDocument.Load(SOCSURL);

            XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/PROactivityAbsenceReport");
            foreach (XmlNode reg in xmlNodeList)
            {
                foreach (XmlNode p in reg.ChildNodes)
                {
                    ActivityAttendence temp = new ActivityAttendence();
                    temp.txtSchoolID = p["PupilID"].InnerText;
                    temp.ActivityDateTime = Convert.ToDateTime(p["Date"].InnerText);
                    temp.ActivityName = p["Activity"].InnerText;
                    temp.AcademicYear = p["Year"].InnerText;
                    temp.AcademicTerm = p["Term"].InnerText;
                    var rb = p["RecordedBy"];
                    if (rb != null)
                        temp.RecordedBy = p["RecordedBy"].InnerText;

                    var tic = p["MasterInCharge"];
                    if (tic != null)
                        temp.tic = p["MasterInCharge"].InnerText;
                    var ex = p["Excused"];
                    if (ex != null)
                    {
                        string e = p["Excused"].InnerText;
                        if (e == "1")
                            temp.excused = true;
                        else if (e == "0")
                            temp.excused = false;
                    }
                    else
                    {
                        temp.excused = null;
                    }

                    var er = p["ExcusedReason"];
                    if (er != null)
                        temp.excusedReason = p["ExcusedReason"].InnerText;
                    var eb = p["ExcusedBy"];
                    if (eb != null)
                        temp.excusedby = p["ExcusedBy"].InnerText;
                    var lmd = p["ModifyDate"].InnerText;
                    if (lmd != null)
                    {
                        string[] newdatetime = lmd.Split('/');
                        string date = newdatetime[1] + "/" + newdatetime[0] + "/" + newdatetime[2];
                        temp.LastModDate = Convert.ToDateTime(date);
                    }

                    var rep = p["ReportURL"].InnerText;
                    if (rep != null)
                        temp.ReportPath = p["ReportURL"].InnerText;

                    att.Add(temp);
                }
            }

            return att;
        }

        public void Dispose()
        {
            
        }
    }
}
