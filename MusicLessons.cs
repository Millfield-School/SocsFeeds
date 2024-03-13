using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using SocsFeeds.helpers;
using SocsFeeds.objects;

namespace SocsFeeds
{
    public class MusicLessons
    {
        public class Root
        {
            public List<Tuition> TuitionList { get; set; } = new List<Tuition>();
        }
        
        public static async Task<Response<Root>> GetLessons(DateTime lessonDate = default)
        {
            try
            {
                var extraParameters = new Dictionary<string, string>
                {
                    {"data", "musiclessons"}
                };

                if (lessonDate != DateTime.MinValue)
                    extraParameters.Add("startdate", lessonDate.ToLongDateString());

                var response = await ApiClientProvider.GetApiResponseAsync("tuition", extraParameters);

                if (response.IsSuccessStatusCode)
                {
                    var responseXml = await response.Content.ReadAsStringAsync();
                    var lesons = ParseFromXml(responseXml);
                    return Response<Root>.Success(new Root { TuitionList = lesons });
                }

                return Response<Root>.Error(response.ReasonPhrase);
            }
            catch (Exception e)
            {
                return Response<Root>.Error($"Error retrieving MusicLessons data - {e.Message}");
            }
        }

        private static List<Tuition> ParseFromXml(string xml)
        {
            var tuitions = new List<Tuition>();
            var responseXml = XDocument.Parse(xml);

            foreach (var node in responseXml.Descendants("lesson"))
            {
                var temp = new Tuition
                {
                    LessonID = int.TryParse(node.Element("lessonid")?.Value, out int lessonId) ? lessonId : 0,
                    LessonStartDate = DateOnly.TryParse(node.Element("startdate")?.Value, out DateOnly lessonStartDate) ? lessonStartDate : default,
                    LessonStartTime = node.Element("starttime")?.Value,
                    LessonEndTime = node.Element("endtime")?.Value,
                    LessonType = node.Element("instrument")?.Value,
                    LessonTitle = node.Element("title")?.Value,
                    Location = node.Element("location")?.Value,
                    LessonCostSchool = decimal.TryParse(node.Element("costschool")?.Value, out decimal lessonCostSchool) ? lessonCostSchool : 0,
                    LessonCostPupil = decimal.TryParse(node.Element("costpupil")?.Value, out decimal lessonCostPupil) ? lessonCostPupil : 0,
                    StaffID = node.Element("staffid")?.Value,
                    PupilID = node.Element("pupilid")?.Value,
                    Attendance = node.Element("attendance")?.Value,
                };
                tuitions.Add(temp);
            }
            return tuitions;
        }
    }

}
