using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Xml.Linq;
using SocsFeeds.helpers;
using SocsFeeds.objects;

namespace SocsFeeds
{
    public class SportsTutoring
    {
        public class Root
        {
            public List<Tuition> Tuitions { get; set; } = new List<Tuition>();
        }
        
        public static async Task<Response<Root>> GetLessons(DateTime lessonDate = default)
        {
            try
            {
                var extraParameters = new Dictionary<string, string>
                {
                    {"data", "sportcoaching"}
                };

                if (lessonDate != DateTime.MinValue)
                    extraParameters.Add("startdate", lessonDate.ToLongDateString());

                var response = await ApiClientProvider.GetApiResponseAsync("tuition", extraParameters);

                if (response.IsSuccessStatusCode)
                {
                    var responseXml = await response.Content.ReadAsStringAsync();
                    var lessons = ParseFromXml(responseXml);
                    return Response<Root>.Success(new Root { Tuitions = lessons });
                }

                return Response<Root>.Error(response.ReasonPhrase);
            }
            catch (Exception e)
            {
                return Response<Root>.Error($"Error retrieving Tuition data - {e.Message}");
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
                    LessonID = Convert.ToInt32(node.Element("lessonid")?.Value ?? "0"),
                    LessonStartDate = DateOnly.ParseExact(node.Element("startdate")?.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    LessonStartTime = node.Element("starttime")?.Value,
                    LessonEndTime = node.Element("endtime")?.Value,
                    LessonType = node.Element("sport")?.Value!,
                    LessonTitle = node.Element("title")?.Value!,
                    Location = node.Element("location")?.Value!,
                    LessonCostSchool = decimal.Parse(node.Element("costschool")?.Value ?? "0"),
                    LessonCostPupil = decimal.Parse(node.Element("costpupil")?.Value ?? "0"),
                    StaffID = node.Element("staffid")?.Value!,
                    PupilID = node.Element("pupilid")?.Value ?? "0",
                    Attendance = node.Element("attendance")?.Value!
                };
                tuitions.Add(temp);
            }
            return tuitions;
        }
    }
}

