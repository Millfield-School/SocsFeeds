using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using SocsFeeds.objects;

namespace SocsFeeds
{
    public class MusicLessons : IDisposable
    {
        public async Task<List<Tuition>> GetLessonsAsync(DateTime lessonDate, int schoolID, string apiKey)
        {
            string SOCSURL = $"https://www.socscms.com/socs/xml/tuition.ashx?ID={schoolID}&key={apiKey}&data=musiclessons";

            List<Tuition> ml = new List<Tuition>();
            XmlDocument xmlDocument = new XmlDocument();
            using (var httpClient = new HttpClient())
            {
                xmlDocument.LoadXml(await httpClient.GetStringAsync(SOCSURL + "&startdate=" + lessonDate.ToLongDateString()));
            }
            XmlNodeList xmlNodeList = xmlDocument.SelectNodes("lessons");
            foreach (XmlNode lesson in xmlNodeList)
            {
                foreach (XmlNode ld in lesson.ChildNodes)
                {
                    Tuition l = new Tuition();
                    l.LessonID = Convert.ToInt32(ld["lessonid"].InnerText);
                    var lsd = ld.SelectSingleNode("startdate");
                    if (lsd != null)
                        l.LessonStartDate = Convert.ToDateTime(ld["startdate"].InnerText);
                    var lst = ld.SelectSingleNode("starttime");
                    if (lst != null)
                        l.LessonStartTime = ld["starttime"].InnerText;
                    var let = ld.SelectSingleNode("endtime");
                    if (let != null)
                        l.LessonEndTime = ld["endtime"].InnerText;
                    var ins = ld.SelectSingleNode("instrument");
                    if (ins != null)
                        l.LessonType = ld["instrument"].InnerText;
                    var tit = ld.SelectSingleNode("title");
                    if (tit != null)
                        l.LessonTitle = ld["title"].InnerText;
                    var loc = ld.SelectSingleNode("location");
                    if (loc != null)
                        l.Location = ld["location"].InnerText;
                    var cts = ld.SelectSingleNode("costschool");
                    if (cts != null)
                        l.LessonCostSchool = Convert.ToDecimal(ld["costschool"].InnerText);
                    var ctp = ld.SelectSingleNode("costpupil");
                    if (ctp != null)
                        l.LessonCostPupil = Convert.ToDecimal(ld["costpupil"].InnerText);
                    l.StaffID = ld["staffid"].InnerText;
                    l.PupilID = ld["pupilid"].InnerText;
                    var att = ld.SelectSingleNode("attendance");
                    if (att != null)
                        l.Attendance = ld["attendance"].InnerText;
                    ml.Add(l);
                }
            }
            return ml;
        }

        public void Dispose()
        {

        }
    }
}
