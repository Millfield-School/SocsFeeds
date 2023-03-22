using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using SocsFeeds.objects;

namespace SocsFeeds
{
    public class Clubs : IDisposable
    {
        public async Task<List<Club>> GetClubDetails(int schoolID, string apiKey)
        {
            string socsUrl = $"https://www.socscms.com/socs/xml/cocurricular.ashx?ID={schoolID}&key={apiKey}&data=clubs&staff=1";

            using (var client = new HttpClient())
            using (var response = await client.GetAsync(socsUrl))
            {
                var xml = await response.Content.ReadAsStringAsync();

                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);

                var clubNodes = xmlDoc.SelectNodes("//club");

                var clubs = new List<Club>();

                foreach (XmlNode clubNode in clubNodes)
                {
                    var club = new Club
                    {
                        Term = clubNode.SelectSingleNode("term")?.InnerText,
                        AcademicYear = clubNode.SelectSingleNode("academicyear")?.InnerText,
                        Category = clubNode.SelectSingleNode("category")?.InnerText,
                        ClubID = int.TryParse(clubNode.SelectSingleNode("clubid")?.InnerText, out int clubId) ? clubId : 0,
                        ClubName = clubNode.SelectSingleNode("clubname")?.InnerText,
                        Gender = clubNode.SelectSingleNode("gender")?.InnerText,
                        YearGroups = clubNode.SelectNodes("yeargroups/yeargroup").Cast<XmlNode>().Select(x => x.InnerText).ToList(),
                        StaffIDs = clubNode.SelectNodes("staff/staffid").Cast<XmlNode>().Select(x => x.InnerText).ToList(),
                    };
                    clubs.Add(club);
                }
                return clubs;
            }
        }

        public void Dispose()
        {

        }
    }
}