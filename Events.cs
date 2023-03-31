using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using SocsFeeds.objects;

namespace SocsFeeds
{
    public class Events 
    {
       
        public static async Task<(List<EventAttendance>,string)> GetEventAttendance(DateTime eventDate, int schoolID, string apiKey)
        {
            string socsUrl = $"https://www.socscms.com/socs/xml/cocurricular.ashx?ID={schoolID}&key={apiKey}&data=registers&startdate={eventDate.ToLongDateString()}&enddate={eventDate.ToLongDateString()}";
            var client = new HttpClient();
            var response = await client.GetAsync(socsUrl);
            
            var attendanceList = new List<EventAttendance>();

            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = $"Error retrieving Event Attendance. Status code: {response.StatusCode}";
                return (null, errorMessage);
            }

            
                var xml = await response.Content.ReadAsStringAsync();

                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);

                var attendanceNodes = xmlDoc.SelectNodes("//register/pupil");

                

                foreach (XmlNode attendanceNode in attendanceNodes)
                {
                    var attendance = new EventAttendance
                    {
                        EventID = int.TryParse(attendanceNode.SelectSingleNode("eventid")?.InnerText, out int eventId) ? eventId : 0,
                        PupilID = attendanceNode.SelectSingleNode("pupilid")?.InnerText,
                        Attendance = attendanceNode.SelectSingleNode("attendance")?.InnerText,
                    };
                    attendanceList.Add(attendance);
                }
            
            return (attendanceList,null);
        }
        
        public static async Task<(List<EventAttendance>,string)> GetEventAttendance(DateTime startEventDate, DateTime endEventDate, int schoolID, string apiKey)
        {
            string socsUrl = $"https://www.socscms.com/socs/xml/cocurricular.ashx?ID={schoolID}&key={apiKey}&data=registers&startdate={startEventDate.ToLongDateString()}&enddate={endEventDate.ToLongDateString()}";
            Console.WriteLine(socsUrl);
            var client = new HttpClient();
            var response = await client.GetAsync(socsUrl);
            var attendanceList = new List<EventAttendance>();
            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = $"Error retrieving Event Attendance. Status code: {response.StatusCode}";
                return (null, errorMessage);
            }
            var eventAttendance = new List<EventAttendance>();
            {
                var xml = await response.Content.ReadAsStringAsync();
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);
                var attendanceNodes = xmlDoc.SelectNodes("//registers/pupil");
                foreach (XmlNode attendanceNode in attendanceNodes)
                {
                    var attendance = new EventAttendance
                    {
                        EventID = int.TryParse(attendanceNode.SelectSingleNode("eventid")?.InnerText, out int eventId) ? eventId : 0,
                        PupilID = attendanceNode.SelectSingleNode("pupilid")?.InnerText,
                        Attendance = attendanceNode.SelectSingleNode("attendance")?.InnerText
                    };
                    eventAttendance.Add(attendance);
                }
            }
            return (eventAttendance,null);
        }

        public static async Task<(List<Event>, string)> GetEventDetails(DateTime eventDate, int schoolID, string apiKey)
        {
            try
            {
                string socsUrl = $"https://www.socscms.com/socs/xml/cocurricular.ashx?ID={schoolID}&key={apiKey}&data=events&staff=1";
                List<Event> events = new List<Event>();

                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync($"{socsUrl}&startdate={eventDate.ToLongDateString()}&enddate={eventDate.ToLongDateString()}");

                    if (!response.IsSuccessStatusCode)
                    {
                        string errorMessage = $"Error retrieving event details. Status code: {response.StatusCode}";
                        return (null, errorMessage);
                    }

                    var xml = await response.Content.ReadAsStringAsync();
                    var xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(xml);

                    var eventNodes = xmlDoc.SelectNodes("//event");

                    foreach (XmlNode eventNode in eventNodes)
                    {
                        var evt = new Event
                        {
                            clubid = int.TryParse(eventNode.SelectSingleNode("clubid")?.InnerText, out int clubId) ? clubId : 0,
                            eventid = int.TryParse(eventNode.SelectSingleNode("eventid")?.InnerText, out int eventId) ? eventId : 0,
                            EventTitle = eventNode.SelectSingleNode("title")?.InnerText,
                            Location = eventNode.SelectSingleNode("location")?.InnerText,
                            StartDate = DateTime.ParseExact(eventNode.SelectSingleNode("startdate")?.InnerText, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            StartTime = eventNode.SelectSingleNode("starttime")?.InnerText,
                            EndTime = eventNode.SelectSingleNode("endtime")?.InnerText,
                            AlldayEvent = bool.TryParse(eventNode.SelectSingleNode("alldayevent")?.InnerText, out bool alldayEvent) && alldayEvent,
                            RecurringID = eventNode.SelectSingleNode("recurringid")?.InnerText,
                            pupilID = eventNode.SelectSingleNode("pupils")?.InnerText.Split(',').ToList(),
                            staffID = eventNode.SelectSingleNode("staff")?.SelectNodes("staffid").Cast<XmlNode>().Select(x => x.InnerText).ToList(),
                        };
                        events.Add(evt);
                    }

                }
                return (events, null);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An error occurred while retrieving event details: {ex.Message}";
                return (null, errorMessage);
            }
        }


        public static async Task<(List<Event>, string)> GetEventDetails(DateTime startEventDate, DateTime endEventDate, int schoolID, string apiKey)
        {
            try
            {
                string socsUrl = $"https://www.socscms.com/socs/xml/cocurricular.ashx?ID={schoolID}&key={apiKey}&data=events&staff=1";
                List<Event> events = new List<Event>();

                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync($"{socsUrl}&startdate={startEventDate.ToLongDateString()}&enddate={endEventDate.ToLongDateString()}");

                    if (!response.IsSuccessStatusCode)
                    {
                        string errorMessage = $"Error retrieving event details. Status code: {response.StatusCode}";
                        return (null, errorMessage);
                    }

                    var xml = await response.Content.ReadAsStringAsync();
                    var xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(xml);

                    var eventNodes = xmlDoc.SelectNodes("//event");

                    foreach (XmlNode eventNode in eventNodes)
                    {
                        var evt = new Event
                        {
                            clubid = int.TryParse(eventNode.SelectSingleNode("clubid")?.InnerText, out int clubId) ? clubId : 0,
                            eventid = int.TryParse(eventNode.SelectSingleNode("eventid")?.InnerText, out int eventId) ? eventId : 0,
                            EventTitle = eventNode.SelectSingleNode("title")?.InnerText,
                            Location = eventNode.SelectSingleNode("location")?.InnerText,
                            StartDate = DateTime.ParseExact(eventNode.SelectSingleNode("startdate")?.InnerText, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            StartTime = eventNode.SelectSingleNode("starttime")?.InnerText,
                            EndTime = eventNode.SelectSingleNode("endtime")?.InnerText,
                            AlldayEvent = bool.TryParse(eventNode.SelectSingleNode("alldayevent")?.InnerText, out bool alldayEvent) && alldayEvent,
                            RecurringID = eventNode.SelectSingleNode("recurringid")?.InnerText,
                            pupilID = eventNode.SelectSingleNode("pupils")?.InnerText.Split(',').ToList(),
                            staffID = eventNode.SelectSingleNode("staff")?.SelectNodes("staffid").Cast<XmlNode>().Select(x => x.InnerText).ToList(),
                        };
                        events.Add(evt);
                    }

                }
                return (events, null);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An error occurred while retrieving event details: {ex.Message}";
                return (null, errorMessage);
            }
        }

        public static async Task<(List<ActivityAttendence>, string)> GetActivityAttendences(string academicTerm, string academicYear, string apiKey, int schoolID, string category)
        {
            try
            {
                string socsUrl = $"https://www.socscms.com/socs/xml/proactivityabsencereport.ashx?ID={schoolID}&key={apiKey}&Term={academicTerm}&AcademicYear={academicYear}";

                if (!string.IsNullOrEmpty(category))
                {
                    socsUrl += $"&Category=LIKE:{category}";
                }

                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(socsUrl);

                    if (!response.IsSuccessStatusCode)
                    {
                        string errorMessage = $"Error retrieving activity attendances. Status code: {response.StatusCode}";
                        return (null, errorMessage);
                    }

                    var xml = await response.Content.ReadAsStringAsync();
                    var xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(xml);

                    var activityNodes = xmlDoc.SelectNodes("//PROactivityAbsenceReport/pupil");
                    var activityAttendences = new List<ActivityAttendence>();

                    foreach (XmlNode activityNode in activityNodes)
                    {
                        var activityAttendence = new ActivityAttendence
                        {
                            txtSchoolID = activityNode.SelectSingleNode("PupilID")?.InnerText,
                            ActivityDateTime = DateTime.TryParse(activityNode.SelectSingleNode("Date")?.InnerText, out DateTime dateTime) ? dateTime : default,
                            ActivityName = activityNode.SelectSingleNode("Activity")?.InnerText,
                            AcademicYear = activityNode.SelectSingleNode("Year")?.InnerText,
                            AcademicTerm = activityNode.SelectSingleNode("Term")?.InnerText,
                            RecordedBy = activityNode.SelectSingleNode("RecordedBy")?.InnerText,
                            tic = activityNode.SelectSingleNode("MasterInCharge")?.InnerText,
                            excused = bool.TryParse(activityNode.SelectSingleNode("Excused")?.InnerText, out bool excusedBool) ? excusedBool : (bool?)null,
                            excusedReason = activityNode.SelectSingleNode("ExcusedReason")?.InnerText,
                            excusedby = activityNode.SelectSingleNode("ExcusedBy")?.InnerText,
                            LastModDate = DateTime.TryParseExact(activityNode.SelectSingleNode("ModifyDate")?.InnerText, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime lastModDate) ? lastModDate : default,
                            ReportPath = activityNode.SelectSingleNode("ReportURL")?.InnerText,
                        };

                        activityAttendences.Add(activityAttendence);
                    }

                    return (activityAttendences, null);
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"An error occurred while retrieving activity attendances: {ex.Message}";
                return (null, errorMessage);
            }
        }


    }
}
