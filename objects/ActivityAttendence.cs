using System;

namespace SocsFeeds.objects
{
    public class ActivityAttendence
    {
        public string txtSchoolID { get; set; }
        public DateTime ActivityDateTime { get; set; }
        public string ActivityName { get; set; }
        public string AcademicTerm { get; set; }
        public string AcademicYear { get; set; }
        public string RecordedBy { get; set; }
        public string tic { get; set; }
        public bool? excused { get; set; }
        public string excusedReason { get; set; }
        public string excusedby { get; set; }
        public DateTime LastModDate { get; set; }
        public string ReportPath { get; set; }
        public bool MAP { get; set; }
        public bool GAMES { get; set; }
    }
}
