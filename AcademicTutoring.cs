using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using SocsFeeds.objects;

namespace SocsFeeds
{
    public class AcademicTutoring 

    {
        public static async Task<(List<Tuition>, string)> GetLessons(DateTime lessonDate, int schoolID, string apiKey)
        {
            try
            {
                string socsUrl = $"https://www.socscms.com/socs/xml/tuition.ashx?ID={schoolID}&key={apiKey}&data=academictutoring&startdate={lessonDate.ToLongDateString()}";
                var lessons = new List<Tuition>();

                using var client = new HttpClient();
                using var response = await client.GetAsync(socsUrl);

                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = $"Error retrieving Tuition data. Status code: {response.StatusCode}";
                    return (null, errorMessage);
                }

                var xml = await response.Content.ReadAsStringAsync();

                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);

                var lessonNodes = xmlDoc.SelectNodes("//lesson");

                foreach (XmlNode lessonNode in lessonNodes)
                {
                    var lesson = new Tuition
                    {
                        LessonID = int.Parse(lessonNode.SelectSingleNode("lessonid")?.InnerText ?? "0"),
                        LessonStartDate = DateTime.Parse(lessonNode.SelectSingleNode("startdate")?.InnerText ?? string.Empty),
                        LessonStartTime = lessonNode.SelectSingleNode("starttime")?.InnerText!,
                        LessonEndTime = lessonNode.SelectSingleNode("endtime")?.InnerText!,
                        LessonType = lessonNode.SelectSingleNode("subject")?.InnerText!,
                        LessonTitle = lessonNode.SelectSingleNode("title")?.InnerText!,
                        Location = lessonNode.SelectSingleNode("location")?.InnerText!,
                        LessonCostSchool = decimal.Parse(lessonNode.SelectSingleNode("costschool")?.InnerText ?? "0"),
                        LessonCostPupil = decimal.Parse(lessonNode.SelectSingleNode("costpupil")?.InnerText ?? "0"),
                        StaffID = lessonNode.SelectSingleNode("staffid")?.InnerText!,
                        PupilID = lessonNode.SelectSingleNode("pupilid")?.InnerText!,
                        Attendance = lessonNode.SelectSingleNode("attendance")?.InnerText!,
                    };

                    lessons.Add(lesson);
                }

                return (lessons, null);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An error occurred while retrieving Tuition data: {ex.Message}";
                return (null, errorMessage);
            }
        }


    }
}

