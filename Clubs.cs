using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SocsFeeds.objects;

namespace SocsFeeds
{
    public class Clubs : IDisposable
    {


        public List<objects.Clubs> ClubDetails(int SchoolID, string APIKey)
        {
            string SOCSURL = "https://www.socscms.com/socs/xml/cocurricular.ashx?ID=" + SchoolID + "&key=" + APIKey + "&data=";
            List<objects.Clubs> clb = new List<objects.Clubs>();
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(SOCSURL + "clubs&staff=1");
            XmlNodeList xmlNodeList = xmlDocument.SelectNodes("clubs");
            foreach (XmlNode clNode in xmlNodeList)
            {
                foreach (XmlNode p in clNode.ChildNodes)
                {
                    objects.Clubs e = new objects.Clubs();
                    e.term = p["term"].InnerText;
                    e.academicYear = p["academicyear"].InnerText;
                    e.category = p["category"].InnerText;
                    e.clubID = Convert.ToInt32(p["clubid"].InnerText);
                    e.clubname = p["clubname"].InnerText;
                    e.gender = p["gender"].InnerText;
                    var years = p.SelectSingleNode("yeargroups");
                    if (years != null)
                    {
                        string yr = p["yeargroups"].InnerText;
                       List<string> s = yr.Split(',').ToList();
                       e.yearGroups = s;
                    }
                    var staff = p.SelectSingleNode("staff");
                    if (years != null)
                    {
                        string staffid = p["staff"].InnerText;
                        List<string> d = staffid.Split(',').ToList();
                        e.staffID = d;
                    }
                    clb.Add(e);
                }
            }

            return clb;
        }

        public void Dispose()
        {

        }
    }
}