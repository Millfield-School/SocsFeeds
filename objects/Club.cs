using System.Collections.Generic;

namespace SocsFeeds.objects
{
    public class Club
    {
        public int ClubID { get; set; }
        public string Term { get; set; }
        public string AcademicYear { get; set; }
        public string Category { get; set; }
        public string ClubName { get; set; }
        public string Gender { get; set; }
        public List<int> YearGroups { get; set; }
        public List<string> StaffIDs { get; set; }
    }
}

