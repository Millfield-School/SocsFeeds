using System;
using System.Collections.Generic;
using System.Text;

namespace SocsFeeds.objects
{
    public class Clubs
    {
        public int clubID { get; set; }
        public string term { get; set; }
        public string academicYear { get; set; }
        public  string category { get; set; }
        public string clubname { get; set; }
        public string gender { get; set; }
        public List<String>yearGroups { get; set; }
        public List<String> staffID { get; set; }
    }
}

