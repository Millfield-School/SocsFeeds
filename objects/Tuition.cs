using System;
using System.Collections.Generic;
using System.Text;

namespace SocsFeeds.objects
{
    public class Tuition
    {
        public int LessonID { get; set; }
        public DateTime LessonStartDate { get; set; }
        public string LessonStartTime { get; set; }
        public string LessonEndTime { get; set; }
        public string LessonType { get; set; }
        public string LessonTitle { get; set; }
        public string Location { get; set; }
        public decimal LessonCostSchool { get; set; }
        public decimal LessonCostPupil { get; set; }
        public string StaffID { get; set; }
        public string PupilID { get; set; }
        public string Attendance { get; set; }
    }
}
