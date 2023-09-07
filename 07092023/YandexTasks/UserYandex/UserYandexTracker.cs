using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YandexTask.UserYandex
{
    public class UserYandexTracker
    {
        public string self { get; set; }

        public string uid { get; set; }

        public string login { get; set; }

        public string trackerUid { get; set; }

        public string passportUid { get; set; }

        public string fitstname { get; set; }

        public string lastname { get; set; }

        public string display { get; set; }

        public string email { get; set; }

        public bool external { get; set; }

        public bool hasLicense { get; set; }

        public bool dismissed { get; set; }

        public bool useNewFilters { get; set; }

        public bool disableNotifications { get; set; }

        public string firstLoginDate { get; set; }

        public string lastLoginDate { get; set;}

        public bool welcomeMailSent { get; set; }
    }
}
