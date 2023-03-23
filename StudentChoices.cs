using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using SocsFeeds.objects;

namespace SocsFeeds
{
    public class StudentChoices : IDisposable
    {
        public async Task<List<Choices>> StudentClubs(string academicYear, string term, string category, int schoolID, string apiKey)
        {
            string socsUrl = $"https://www.socscms.com/socs/xml/proactivityClubParticipationreport.ashx?ID={schoolID}&key={apiKey}";

            if (!string.IsNullOrEmpty(category))
            {
                socsUrl += $"&Term={term}&AcademicYear={academicYear}&Category=LIKE:{category}";
            }
            else
            {
                socsUrl += $"&Term={term}&AcademicYear={academicYear}";
            }

            var client = new HttpClient();
            var response = await client.GetAsync(socsUrl);
            response.EnsureSuccessStatusCode();

            var xml = await response.Content.ReadAsStringAsync();
            var xmlDoc = XDocument.Parse(xml);

            var choicesNodes = xmlDoc.Descendants("pupil");
            var choices = new List<Choices>();

            foreach (var node in choicesNodes)
            {
                var choice = new objects.Choices
                {
                    ActivityName = node.Element("Activity")?.Value,
                    Term = node.Element("Term")?.Value,
                    PupilID = node.Element("PupilID")?.Value,
                    AcademicYear = node.Element("Year")?.Value,
                    Gender = node.Element("Gender")?.Value,
                    DayTime = node.Element("DayTime")?.Value,
                    YearGroups = node.Element("YearGroups")?.Value
                };

                choices.Add(choice);
            }
            return choices;
        }

        public void Dispose()
        {

        }
    }
}