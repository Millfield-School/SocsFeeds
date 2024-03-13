using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using SocsFeeds.helpers;
using SocsFeeds.objects;

namespace SocsFeeds
{
    public class StudentChoices
    {
        public class Root
        {
            public List<Choices> StudentChoicesList { get; set; } = new List<Choices>();
        }

        public static async Task<Response<Root>> StudentClubs(string fullAcademicYear, string term, string category = null)
        {
            try
            {
                var extraParameters = new Dictionary<string, string>
                {
                    {"term", term},
                    {"AcademicYear", fullAcademicYear}
                };

                if (!string.IsNullOrEmpty(category))
                    extraParameters.Add("Category", $"LIKE:{category}");

                var response = await ApiClientProvider.GetApiResponseAsync("proactivityClubParticipationreport", extraParameters);

                if (response.IsSuccessStatusCode)
                {
                    var responseXml = await response.Content.ReadAsStringAsync();
                    var choices = ParseFromXml(responseXml);
                    return Response<Root>.Success(new Root { StudentChoicesList = choices });
                }

                return Response<Root>.Error(response.ReasonPhrase);
            }
            catch (Exception e)
            {
                return Response<Root>.Error($"Error retrieving Student Choices data - {e.Message}");
            }
        }

        private static List<Choices> ParseFromXml(string xml)
        {
            var choices = new List<Choices>();
            var responseXml = XDocument.Parse(xml);

            foreach (var node in responseXml.Descendants("pupil"))
            {
                var temp = new Choices
                {
                    ActivityName = node.Element("Activity")?.Value,
                    Term = node.Element("Term")?.Value,
                    PupilID = node.Element("PupilID")?.Value,
                    AcademicYear = node.Element("Year")?.Value,
                    Gender = node.Element("Gender")?.Value,
                    DayTime = node.Element("DayTime")?.Value,
                    YearGroups = node.Element("YearGroups")?.Value
                };
                choices.Add(temp);
            }
            return choices;
        }
    }
}