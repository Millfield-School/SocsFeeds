using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using SocsFeeds.objects;

namespace SocsFeeds
{
    public class Fixtures : IDisposable
    {
        public async Task<List<Fixture>> GetFixtureDetails(DateTime startDate, DateTime finishDate, int schoolId, string apiKey)
        {
            string url = $"https://www.schoolssports.com/school/xml/fixturecalendar.ashx?ID={schoolId}&key={apiKey}&TS=1&startdate={startDate.ToLongDateString()}&enddate={finishDate.ToLongDateString()}";
            var fixtures = new List<Fixture>();
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(content);
                var clubNodes = xmlDocument.GetElementsByTagName("fixtures");

                foreach (XmlNode clubNode in clubNodes)
                {
                    foreach (XmlNode fixtureNode in clubNode.ChildNodes)
                    {
                        var fixture = new Fixture();
                        fixture.EventID = Convert.ToInt32(fixtureNode["eventid"].InnerText);
                        fixture.Sport = fixtureNode["sport"].InnerText;
                        fixture.FixtureDate = Convert.ToDateTime(fixtureNode["date"].InnerText);
                        fixture.MeetTime = fixtureNode["meettime"]?.InnerText;
                        fixture.FixtureTime = fixtureNode["time"].InnerText;
                        fixture.ReturnTime = fixtureNode["returntime"]?.InnerText;
                        fixture.TeamName = fixtureNode["team"].InnerText;
                        fixture.Opposition = fixtureNode["opposition"].InnerText;
                        fixture.Oppositionteam = fixtureNode["oppositionteam"].InnerText;
                        fixture.Location = fixtureNode["location"]?.InnerText;
                        fixture.Transport = fixtureNode["transport"]?.InnerText;
                        fixture.Details = fixtureNode["details"]?.InnerText;
                        fixture.URL = fixtureNode["url"]?.InnerText;
                        fixture.StartDateTimeFull = DateTime.TryParse(fixtureNode["startdatefull"].InnerText, out DateTime startDateTimeFull) ? startDateTimeFull : default;
                        fixture.EndDateTimeFull = DateTime.TryParse(fixtureNode["enddatefull"].InnerText, out DateTime endDateTimeFull) ? endDateTimeFull : default;
                        if (fixtureNode["pupils"] != null)
                        {
                            fixture.PupilsList = fixtureNode["pupils"].InnerText.Split(',').ToList();
                        }

                        if (fixtureNode["staff"] != null)
                        {
                            fixture.StaffList = fixtureNode["staff"].InnerText.Split(',').ToList();
                        }

                        fixtures.Add(fixture);
                    }
                }
            }
            return fixtures;
        }

        public void Dispose()
        {

        }
    }
}
