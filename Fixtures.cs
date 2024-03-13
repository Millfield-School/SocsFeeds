using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using SocsFeeds.helpers;
using SocsFeeds.objects;

namespace SocsFeeds
{
    public class Fixtures
    {
        public class Root
        {
            public List<Fixture> Fixtures { get; set; } = new List<Fixture>();
        }

        public class AttendanceRoot
        {
            public List<FixtureAttendence> Fixtures { get; set; } = new List<FixtureAttendence>();
        }
        
        public static async Task<Response<Root>> GetFixtureDetails(DateTime startDate, DateTime finishDateTime)
        {
            try
            {
                var extraParameters = new Dictionary<string, string>
                {
                    {"startdate", startDate.ToLongDateString()},
                    {"enddate", finishDateTime.ToLongDateString()},
                    {"ts", "1"}
                };
                
                var response = await ApiClientProvider.GetApiResponseAsync("fixturecalendar", extraParameters);

                if (response.IsSuccessStatusCode)
                {
                    var responseXml = await response.Content.ReadAsStringAsync();
                    var fixtures = ParseDetailsFromXml(responseXml);
                    return Response<Root>.Success(new Root { Fixtures = fixtures });
                }

                return Response<Root>.Error(response.ReasonPhrase);
            }
            catch (Exception e)
            {
                return Response<Root>.Error($"Error retrieving fixture details data - {e.Message}");
            }
        }

        private static List<Fixture> ParseDetailsFromXml(string xml)
        {
            var tuitions = new List<Fixture>();
            var responseXml = XDocument.Parse(xml);

            foreach (var node in responseXml.Descendants("fixture"))
            {
                var temp = new Fixture
                {
                    EventID = Convert.ToInt32(node.Element("eventid")?.Value),
                    Sport = node.Element("sport")?.Value,
                    FixtureDate = Convert.ToDateTime(node.Element("date")?.Value),
                    MeetTime = node.Element("meettime")?.Value,
                    FixtureTime = node.Element("time")?.Value,
                    ReturnTime = node.Element("returntime")?.Value,
                    TeamName = node.Element("team")?.Value,
                    Opposition = node.Element("opposition")?.Value,
                    Oppositionteam = node.Element("oppositionteam")?.Value,
                    Location = node.Element("location")?.Value,
                    Transport = node.Element("transport")?.Value,
                    Details = node.Element("details")?.Value,
                    URL = node.Element("url")?.Value,
                    StartDateTimeFull = DateTime.TryParse(node.Element("startdatefull")?.Value, out DateTime startDateTimeFull) ? startDateTimeFull : default,
                    EndDateTimeFull = DateTime.TryParse(node.Element("enddatefull")?.Value, out DateTime endDateTimeFull) ? endDateTimeFull : default,
                    PupilsList = ParserUtility.ParseCommaSeparatedValues<string>(node.Element("pupils")?.Value),
                    StaffList = ParserUtility.ParseCommaSeparatedValues<string>(node.Element("staff")?.Value)
                };
                tuitions.Add(temp);
            }
            return tuitions;
        }
        
        public static async Task<Response<AttendanceRoot>> GetFixtureAttendance(DateTime startDate)
        {
            try
            {
                var extraParameters = new Dictionary<string, string>
                {
                    {"startdate", startDate.ToLongDateString()},
                    {"data", "registers"}
                };

                var response = await ApiClientProvider.GetApiResponseAsync("mso-sport", extraParameters);

                if (response.IsSuccessStatusCode)
                {
                    var responseXml = await response.Content.ReadAsStringAsync();
                    var fixtures = ParseAttendanceFromXml(responseXml);
                    return Response<AttendanceRoot>.Success(new AttendanceRoot { Fixtures = fixtures });
                }

                return Response<AttendanceRoot>.Error(response.ReasonPhrase);
            }
            catch (Exception e)
            {
                return Response<AttendanceRoot>.Error($"Error retrieving fixture attendance data - {e.Message}");
            }
        }

        private static List<FixtureAttendence> ParseAttendanceFromXml(string xml)
        {
            var attendance = new List<FixtureAttendence>();
            var responseXml = XDocument.Parse(xml);

            foreach (var node in responseXml.Descendants("pupil"))
            {
                var temp = new FixtureAttendence
                {
                    fixtureid = Convert.ToInt32(node.Element("eventid")?.Value),
                    pupilid = node.Element("pupilid")?.Value,
                    attendancestatus = node.Element("attendancestatus")?.Value,
                    consentstatus = Convert.ToInt32(node.Element("consentstatus")?.Value),
                    transportfromconfirmed = Convert.ToBoolean(node.Element("transporttoconfirmed")?.Value),
                    transporttooption = node.Element("transporttooption")?.Value,
                    transporttoconfirmed = Convert.ToBoolean(node.Element("transportfromconfirmed")?.Value),
                    transportfromoption = node.Element("transportfromoption")?.Value
                };
                attendance.Add(temp);
            }
            return attendance;
        }
    }
}