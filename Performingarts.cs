using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using SocsFeeds.objects;

namespace SocsFeeds
{
    public class PerformingArts
    {
        public static async Task<(List<Tuition>,string)> GetLessons(DateTime lessonDate, int schoolID, string apiKey)
        {
            var socsUrl = $"https://www.socscms.com/socs/xml/tuition.ashx?ID={schoolID}&key={apiKey}&data=performingarts&startdate={lessonDate.ToLongDateString()}";

            using var client = new HttpClient();
            var response = await client.GetAsync(socsUrl);

            // If the response indicates failure, return an error message
            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = $"Error retrieving Performing Arts data. Status code: {response.StatusCode}";
                return (null, errorMessage);
            }

            var xml = await response.Content.ReadAsStringAsync();

            var xmlDoc = XDocument.Parse(xml);

            var lessonNodes = xmlDoc.Descendants("lessons").Elements();

            var lessons = new List<Tuition>();

            foreach (var lessonNode in lessonNodes)
            {
                var lesson = new Tuition
                {
                    LessonID = int.TryParse(lessonNode.Element("lessonid")?.Value, out int lessonId) ? lessonId : 0,
                    LessonStartDate = DateTime.TryParse(lessonNode.Element("startdate")?.Value, out DateTime lessonStartDate) ? lessonStartDate : default,
                    LessonStartTime = lessonNode.Element("starttime")?.Value,
                    LessonEndTime = lessonNode.Element("endtime")?.Value,
                    LessonType = lessonNode.Element("instrument")?.Value,
                    LessonTitle = lessonNode.Element("title")?.Value,
                    Location = lessonNode.Element("location")?.Value,
                    LessonCostSchool = decimal.TryParse(lessonNode.Element("costschool")?.Value, out decimal lessonCostSchool) ? lessonCostSchool : 0,
                    LessonCostPupil = decimal.TryParse(lessonNode.Element("costpupil")?.Value, out decimal lessonCostPupil) ? lessonCostPupil : 0,
                    StaffID = lessonNode.Element("staffid")?.Value,
                    PupilID = lessonNode.Element("pupilid")?.Value,
                    Attendance = lessonNode.Element("attendance")?.Value,
                };
                lessons.Add(lesson);
            }

            return (lessons,null);
        }

        
    }
}
