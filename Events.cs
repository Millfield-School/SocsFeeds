using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Xml.Linq;
using SocsFeeds.helpers;
using SocsFeeds.objects;

namespace SocsFeeds
{
    public class Events 
    {
        public class AttendanceRoot
        {
            public List<EventAttendance> Attendances { get; set; } = new List<EventAttendance>();
        }

        public class DetailsRoot
        {
            public List<Event> Details { get; set; } = new List<Event>();
        }

        public class ActivityRoot
        {
            public List<ActivityAttendence> ActivityAttences { get; set; } = new List<ActivityAttendence>();
        }

        public static async Task<Response<AttendanceRoot>> GetEventAttendance(DateTime eventDatetime, DateTime endDateTime = default)
        {
            try
            {
                var extraParameters = new Dictionary<string, string>
                {
                    {"data", "registers"},
                    {"startdate", eventDatetime.ToString("dd-MM-yyyy")}
                };

                if (endDateTime != DateTime.MinValue)
                    extraParameters.Add("enddate", endDateTime.ToString("dd-MM-yyyy"));
                
                var response = await ApiClientProvider.GetApiResponseAsync("cocurricular", extraParameters);

                if (response.IsSuccessStatusCode)
                {
                    var responseXml = await response.Content.ReadAsStringAsync();
                    var attendance = ParseAttendanceFromXml(responseXml);
                    return Response<AttendanceRoot>.Success(new AttendanceRoot { Attendances = attendance });
                }

                return Response<AttendanceRoot>.Error(response.ReasonPhrase);
            }
            catch (Exception e)
            {
                return Response<AttendanceRoot>.Error($"Error retrieving Event Attendance data - {e.Message}");
            }
        }
        
        private static List<EventAttendance> ParseAttendanceFromXml(string xml)
        {
            var attendances = new List<EventAttendance>();
            var responseXml = XDocument.Parse(xml);

            foreach (var node in responseXml.Descendants("pupil"))
            {
                var temp = new EventAttendance
                {
                    EventID = int.TryParse(node.Element("eventid")?.Value, out int eventId) ? eventId : 0,
                    PupilID = node.Element("pupilid")?.Value,
                    Attendance = node.Element("attendance")?.Value,
                };
                attendances.Add(temp);
            }
            return attendances;
        }
       
        public static async Task<Response<DetailsRoot>> GetEventDetails(DateTime eventDatetime, DateTime endDateTime = default)
        {
            try
            {
                var extraParameters = new Dictionary<string, string>
                {
                    {"data", "events"},
                    {"staff", "1"},
                    {"startdate", eventDatetime.ToLongDateString()}
                };

                if (endDateTime != DateTime.MinValue)
                    extraParameters.Add("enddate", endDateTime.ToLongDateString());

                var response = await ApiClientProvider.GetApiResponseAsync("cocurricular", extraParameters);

                if (response.IsSuccessStatusCode)
                {
                    var responseXml = await response.Content.ReadAsStringAsync();
                    var details = ParseDetailsFromXml(responseXml);
                    return Response<DetailsRoot>.Success(new DetailsRoot { Details = details });
                }

                return Response<DetailsRoot>.Error(response.ReasonPhrase);
            }
            catch (Exception e)
            {
                return Response<DetailsRoot>.Error($"Error retrieving Event Details data - {e.Message}");
            }
        }

        private static List<Event> ParseDetailsFromXml(string xml)
        {
            var details = new List<Event>();
            var responseXml = XDocument.Parse(xml);

            foreach (var node in responseXml.Descendants("event"))
            {
                var temp = new Event
                {
                    clubid = int.TryParse(node.Element("clubid")?.Value, out int clubId) ? clubId : 0,
                    eventid = int.TryParse(node.Element("eventid")?.Value, out int eventId) ? eventId : 0,
                    EventTitle = node.Element("title")?.Value,
                    Location = node.Element("location")?.Value,
                    StartDate = DateTime.ParseExact(node.Element("startdate")?.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    StartTime = node.Element("starttime")?.Value,
                    EndTime = node.Element("endtime")?.Value,
                    AlldayEvent = bool.TryParse(node.Element("alldayevent")?.Value, out bool alldayEvent) && alldayEvent,
                    RecurringID = node.Element("recurringid")?.Value,
                    pupilID = ParserUtility.ParseCommaSeparatedValues<string>(node.Element("pupils")?.Value),
                    staffID = ParserUtility.ParseCommaSeparatedValues<string>(node.Element("staff")?.Value),
                };
                details.Add(temp);
            }
            return details;
        }

        public static async Task<Response<ActivityRoot>> GetActivityAttendences(string acadTerm, string fullAcadYear)
        {
            try
            {
                var extraParameters = new Dictionary<string, string>
                {
                    {"term", acadTerm},
                    {"academicYear", fullAcadYear}
                };

                var response = await ApiClientProvider.GetApiResponseAsync("proactivityabsencereport", extraParameters);

                if (response.IsSuccessStatusCode)
                {
                    var responseXml = await response.Content.ReadAsStringAsync();
                    var attendance = ParseActivityFromXml(responseXml);
                    return Response<ActivityRoot>.Success(new ActivityRoot { ActivityAttences = attendance });
                }

                return Response<ActivityRoot>.Error(response.ReasonPhrase);
            }
            catch (Exception e)
            {
                return Response<ActivityRoot>.Error($"Error retrieving Activity Attendance data - {e.Message}");
            }
        }

        private static List<ActivityAttendence> ParseActivityFromXml(string xml)
        {
            var attendances = new List<ActivityAttendence>();
            var responseXml = XDocument.Parse(xml);

            foreach (var node in responseXml.Descendants("pupil"))
            {
                var temp = new ActivityAttendence
                {
                    txtSchoolID = node.Element("PupilID")?.Value,
                    ActivityDateTime = DateTime.TryParse(node.Element("Date")?.Value, out DateTime dateTime) ? dateTime : default,
                    ActivityName = node.Element("Activity")?.Value,
                    AcademicYear = node.Element("Year")?.Value,
                    AcademicTerm = node.Element("Term")?.Value,
                    RecordedBy = node.Element("RecordedBy")?.Value,
                    tic = node.Element("MasterInCharge")?.Value,
                    excused = node.Element("Excused")?.Value switch
                    { 
                        "1" => true,
                        "0" => false,
                        _ => bool.TryParse(node.Element("Excused")?.Value, out bool excusedBool) ? excusedBool : (bool?)null,
                    },
                    excusedReason = node.Element("ExcusedReason")?.Value,
                    excusedby = node.Element("ExcusedBy")?.Value,
                    LastModDate = DateTime.TryParseExact(node.Element("ModifyDate")?.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime lastModDate) ? lastModDate : default,
                    ReportPath = node.Element("ReportURL")?.Value
                };
                attendances.Add(temp);
            }
            return attendances;
        }
      
    }
}
