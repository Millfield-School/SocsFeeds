using System;
using System.Collections.Generic;
using System.Text;

namespace SocsFeeds.objects
{
    public class FixtureAttendence
    {
       public int fixtureid { get; set; }
       public string pupilid { get; set; }
       public string attendancestatus { get; set; }
       public int consentstatus { get; set; }
       public bool transporttoconfirmed { get; set; }
       public string transporttooption { get; set; }
       public bool transportfromconfirmed { get; set; }
       public string transportfromoption { get; set; }

    }
}
