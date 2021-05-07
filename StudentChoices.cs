using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using SocsFeeds.objects;

namespace SocsFeeds
{
    public class StudentChoices
    {
        private static string SOCSURL =
            "https://www.socscms.com/socs/xml/proactivityClubParticipationreport.ashx?ID="+Config.SOCsSchoolID
                                                               +"&key="+Config.SOCsAPIKey;

        public static List<objects.Choices> StudentClubs(string AcademicYear, string term, string Category)
        {
            List<objects.Choices> sc = new List<Choices>();
            XmlDocument xmlDocument = new XmlDocument();
            if(!string.IsNullOrEmpty(Category))
                xmlDocument.Load(SOCSURL+ $"&Term={term}&AcademicYear={AcademicYear}&Category LIKE:{Category}");
            else
                xmlDocument.Load(SOCSURL+$"&Term={term}&AcademicYear={AcademicYear}");

            XmlNodeList clubnode = xmlDocument.SelectNodes("/ActivityClubParticipation/pupil");
            foreach (XmlNode i in clubnode)
            {
                objects.Choices c = new Choices();
                c.ActivityName = i["Activity"].InnerText;
                c.Term = i["Term"].InnerText;
                c.PupilID = i["PupilID"].InnerText;
                c.AcademicYear = i["Year"].InnerText;
                c.Gender = i["Gender"].InnerText;
                c.DayTime = i["DayTime"].InnerText;
                c.YearGroups = i["YearGroups"].InnerText;
                sc.Add(c);
            }

            return sc;

        }
    }
}
