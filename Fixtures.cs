using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using SocsFeeds.objects; // Import any necessary namespaces

namespace SocsFeeds
{
    public class Fixtures
    {
        public static async Task<(List<Fixture>, string)> GetFixtureDetails(DateTime startDate, DateTime finishDate, int schoolId, string apiKey)
        {
            string url = $"https://www.schoolssports.com/school/xml/fixturecalendar.ashx?ID={schoolId}&key={apiKey}&TS=1&startdate={startDate.ToLongDateString()}&enddate={finishDate.ToLongDateString()}";
            var fixtures = new List<Fixture>();
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = $"Error retrieving fixture data. Status code: {response.StatusCode}";
                    return (null, errorMessage);
                }
                var content = await response.Content.ReadAsStringAsync();

                try
                {
                    var xmlDocument = XDocument.Parse(content);
                    var fixtureNodes = xmlDocument.Descendants("fixture");
                    foreach (var fixtureNode in fixtureNodes)
                    {
                        var fixture = new Fixture
                        {
                            EventID = Convert.ToInt32(fixtureNode.Element("eventid")?.Value),
                            Sport = fixtureNode.Element("sport")?.Value,
                            FixtureDate = Convert.ToDateTime(fixtureNode.Element("date")?.Value),
                            MeetTime = fixtureNode.Element("meettime")?.Value,
                            FixtureTime = fixtureNode.Element("time")?.Value,
                            ReturnTime = fixtureNode.Element("returntime")?.Value,
                            TeamName = fixtureNode.Element("team")?.Value,
                            Opposition = fixtureNode.Element("opposition")?.Value,
                            Oppositionteam = fixtureNode.Element("oppositionteam")?.Value,
                            Location = fixtureNode.Element("location")?.Value,
                            Transport = fixtureNode.Element("transport")?.Value,
                            Details = fixtureNode.Element("details")?.Value,
                            URL = fixtureNode.Element("url")?.Value,
                            StartDateTimeFull = DateTime.TryParse(fixtureNode.Element("startdatefull")?.Value, out DateTime startDateTimeFull) ? startDateTimeFull : default,
                            EndDateTimeFull = DateTime.TryParse(fixtureNode.Element("enddatefull")?.Value, out DateTime endDateTimeFull) ? endDateTimeFull : default
                        };
                        if (fixtureNode.Element("pupils") != null)
                        {
                            fixture.PupilsList = fixtureNode.Element("pupils")?.Value.Split(',').ToList();
                        }

                        if (fixtureNode.Element("staff") != null)
                        {
                            fixture.StaffList = fixtureNode.Element("staff")?.Value.Split(',').ToList();
                        }

                        fixtures.Add(fixture);
                    }
                    return (fixtures, null);
                }
                catch (Exception ex)
                {
                    string errorMessage = $"Error parsing fixture data. {ex.Message}";
                    return (null, errorMessage);
                }
            }
        }

        public static async Task<(List<FixtureAttendence>, string)> GetFixtureAttendance(DateTime startDate,
            int schoolId, string apiKey)
        {
            string url =
                $"https://www.schoolssports.com/school/xml/mso-sport.ashx?ID={schoolId}&key={apiKey}&data=registers&startdate={startDate.ToLongDateString()}";
            var fixtures = new List<FixtureAttendence>();
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = $"Error retrieving fixture data. Status code: {response.StatusCode}";
                    return (null, errorMessage);
                }

                var content = await response.Content.ReadAsStringAsync();

                try
                {
                    var xmlDocument = XDocument.Parse(content);
                    var fixtureNodes = xmlDocument.Descendants("pupil");
                    foreach (var fixtureNode in fixtureNodes)
                    {
                        var fixture = new FixtureAttendence()
                        {
                            fixtureid = Convert.ToInt32(fixtureNode.Element("eventid")?.Value),
                            pupilid = fixtureNode.Element("pupilid")?.Value,
                            attendancestatus = fixtureNode.Element("attendancestatus")?.Value,
                            consentstatus = Convert.ToInt32(fixtureNode.Element("consentstatus")?.Value),
                            transportfromconfirmed = Convert.ToBoolean(fixtureNode.Element("transporttoconfirmed")?.Value),
                            transporttooption = fixtureNode.Element("transporttooption")?.Value,
                            transporttoconfirmed = Convert.ToBoolean(fixtureNode.Element("transportfromconfirmed")?.Value),
                            transportfromoption = fixtureNode.Element("transportfromoption")?.Value
                            
                        };
                       

                        fixtures.Add(fixture);
                    }
                    return (fixtures, null);
                }
                catch (Exception ex)
                {
                    string errorMessage = $"Error parsing fixture data. {ex.Message}";
                    return (null, errorMessage);
                }
            }
        }
    }
}