using SocsFeeds.objects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SocsFeeds.helpers;
using System.Xml.Linq;

namespace SocsFeeds
{
    public class SportsCoaching
    {
        public class Root
        {
            public List<Tuition> TuitionList { get; set; } = new List<Tuition>();
        }

        public static async Task<Response<Root>> GetLessons(DateTime lessonDate)
        {
            try
            {
                var extraParameters = new Dictionary<string, string>
                {
                    {"data", "sportcoaching"},
                    {"startdate", lessonDate.ToLongDateString()}
                };
                var response = await ApiClientProvider.GetApiResponseAsync("tuition", extraParameters);

                if (response.IsSuccessStatusCode)
                {
                    var responseXml = await response.Content.ReadAsStringAsync();
                    var lessons = ParseFromXml(responseXml);
                    return Response<Root>.Success(new Root { TuitionList = lessons });
                }

                return Response<Root>.Error(response.ReasonPhrase);
            }
            catch (Exception e)
            {
                return Response<Root>.Error($"Error retrieving SportsCoaching data - {e.Message}");
            }
        }

        private static List<Tuition> ParseFromXml(string xml)
        {
            var lessons = new List<Tuition>();
            var responseXml = XDocument.Parse(xml);

            foreach (var node in responseXml.Descendants("lesson"))
            {
                var lesson = new Tuition
                {
                    LessonID = int.TryParse(node.Element("lessonid")?.Value, out int lessonId) ? lessonId : 0,
                    LessonStartDate = DateOnly.FromDateTime(Convert.ToDateTime(node.Element("startdate")?.Value)),
                    LessonStartTime = node.Element("starttime")?.Value,
                    LessonEndTime = node.Element("endtime")?.Value,
                    LessonType = node.Element("subject")?.Value,
                    LessonTitle = node.Element("title")?.Value,
                    Location = node.Element("location")?.Value,
                    LessonCostSchool = decimal.TryParse(node.Element("costschool")?.Value, out decimal costSchool) ? costSchool : 0,
                    LessonCostPupil = decimal.TryParse(node.Element("costpupil")?.Value, out decimal costPupil) ? costPupil : 0,
                    StaffID = node.Element("staffid")?.Value,
                    PupilID = node.Element("pupilid")?.Value,
                    Attendance = node.Element("attendance")?.Value
                };
                lessons.Add(lesson);
            }
            return lessons;
        }
        
    }
}
