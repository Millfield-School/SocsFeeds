using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using SocsFeeds.helpers;
using SocsFeeds.objects;

namespace SocsFeeds
{
    public class Clubs 
    {
        public class Root
        {
            public List<Club> Clubs { get; set; } = new List<Club>();
        }
        
        public static async Task<Response<Root>> GetClubDetails()
        {
            try
            {
                var extraParameters = new Dictionary<string, string>
                {
                    {"data", "clubs"},
                    {"staff", "1"}
                };
                var response = await ApiClientProvider.GetApiResponseAsync("cocurricular", extraParameters);

                if (response.IsSuccessStatusCode)
                {
                    var responseXml = await response.Content.ReadAsStringAsync();
                    var clubs = ParseFromXml(responseXml);
                    return Response<Root>.Success(new Root { Clubs = clubs });
                }

                return Response<Root>.Error(response.ReasonPhrase);
            }
            catch (Exception e)
            {
                return Response<Root>.Error($"Error retrieving Club data - {e.Message}");
            }
        }

        private static List<Club> ParseFromXml(string xml)
        {
            var tuitions = new List<Club>();
            var responseXml = XDocument.Parse(xml);

            foreach (var node in responseXml.Descendants("club"))
            {
                var temp = new Club
                {
                    Term = node.Element("term")?.Value,
                    AcademicYear = node.Element("academicyear")?.Value,
                    Category = node.Element("category")?.Value,
                    ClubID = int.TryParse(node.Element("clubid")?.Value, out int clubId) ? clubId : 0,
                    ClubName = node.Element("clubname")?.Value,
                    Gender = node.Element("gender")?.Value,
                    YearGroups = ParserUtility.ParseCommaSeparatedValues<int>(node.Element("yeargroups")?.Value),
                    StaffIDs = ParserUtility.ParseCommaSeparatedValues<string>(node.Element("staff")?.Value)
                };
                tuitions.Add(temp);
            }
            return tuitions;
        }
    }
}