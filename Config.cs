using System;
using System.Collections.Generic;
using System.Text;

namespace SocsFeeds
{
    public class Config
    {
        private static string SchoolID = null;
        private static string APIKey = null;
        public static string SOCsSchoolID
        {
            get
            {
                if (SchoolID != null)
                    return SchoolID;
                else
                    return "School ID Not Set";
            }
            set
            {
                SchoolID = value;
            }
        }

        public static string SOCsAPIKey
        {
            get
            {
                if (APIKey != null)
                    return APIKey;
                else
                    return "API Key Value not Supplied";
            }
            set
            {
                APIKey = value;
            }
        }
    }
}
