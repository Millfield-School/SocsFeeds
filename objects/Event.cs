using System;
using System.Collections.Generic;
using System.Text;

namespace SocsFeeds.objects
{
    public class Event
    {
        public int eventid { get; set; }
        public int clubid { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string EventTitle { get; set; }
        public bool AlldayEvent { get; set; }
        public string RecurringID { get; set; }
        public List<String> pupilID { get; set; }
        public List<String> staffID { get; set; }
    }
}
