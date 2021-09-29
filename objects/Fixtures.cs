using System;
using System.Collections.Generic;
using System.Text;

namespace SocsFeeds.objects
{
    public class Fixtures
    {
        public int EventID { get; set; }
        public string Sport { get; set; }
        public DateTime FixtureDate { get; set; }
        public string MeetTime { get; set; }
        public string FixtureTime { get; set; }
        public string ReturnTime { get; set; }
        public string TeamName { get; set; }
        public string Opposition { get; set; }
        public string Oppositionteam { get; set; }
        public string Location { get; set; }
        public string Transport { get; set; }
        public string Details { get; set; }
        public string URL { get; set; }
        public DateTime StartDateTimeFull { get; set; }
        public DateTime EndDateTimeFull { get; set; }
        public List<string> PupilsList { get; set; }
        public List<string> StaffList { get; set; }
    }
}
