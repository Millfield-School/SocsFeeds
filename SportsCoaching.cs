using SocsFeeds.objects;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;

namespace SocsFeeds
{
    public class SportsCoaching 

    {
        public static async Task<(List<Tuition>,string)>GetLessons(DateTime lessonDate, int schoolID, string apiKey)
        {
            string socsUrl = $"https://www.socscms.com/socs/xml/tuition.ashx?ID={schoolID}&key={apiKey}&data=sportcoaching&startdate={lessonDate.ToLongDateString()}";

            var client = new HttpClient();
            var response = await client.GetAsync(socsUrl);

            // If the response indicates failure, return an error message
            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = $"Error retrieving Sports Coaching data. Status code: {response.StatusCode}";
                return (null, errorMessage);
            }

            var xml = await response.Content.ReadAsStringAsync();

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            var lessonNodes = xmlDoc.SelectNodes("//lesson");

            var lessons = new List<Tuition>();
            foreach (XmlNode lessonNode in lessonNodes)
            {
                var lesson = new Tuition
                {
                    LessonID = int.TryParse(lessonNode.SelectSingleNode("lessonid")?.InnerText, out int lessonId) ? lessonId : 0,
                    LessonStartDate = Convert.ToDateTime(lessonNode.SelectSingleNode("startdate")?.InnerText),
                    LessonStartTime = lessonNode.SelectSingleNode("starttime")?.InnerText,
                    LessonEndTime = lessonNode.SelectSingleNode("endtime")?.InnerText,
                    LessonType = lessonNode.SelectSingleNode("subject")?.InnerText,
                    LessonTitle = lessonNode.SelectSingleNode("title")?.InnerText,
                    Location = lessonNode.SelectSingleNode("location")?.InnerText,
                    LessonCostSchool = decimal.TryParse(lessonNode.SelectSingleNode("costschool")?.InnerText, out decimal costSchool) ? costSchool : 0,
                    LessonCostPupil = decimal.TryParse(lessonNode.SelectSingleNode("costpupil")?.InnerText, out decimal costPupil) ? costPupil : 0,
                    StaffID = lessonNode.SelectSingleNode("staffid")?.InnerText,
                    PupilID = lessonNode.SelectSingleNode("pupilid")?.InnerText,
                    Attendance = lessonNode.SelectSingleNode("attendance")?.InnerText
                };
                lessons.Add(lesson);
            }
            return (lessons,null);
        }

        
    }
}
