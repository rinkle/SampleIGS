using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globalsetting
{
    public static class UserRoles
    {
        public const string Admin = "Admin";
        public const string SuperAdmin = "SuperAdmin";
        public const string User = "User";
    }
    public enum NotifyType
    {
        Success,
        Error,
        Info
    }
    public class PageSection
    {
        public const string HomeCarousel = "Carousel";
    }

    public enum PageEnum
    {
        Home = 1,
        Team = 2,
        TeamBio = 3,
        Experience = 4,
        ExperienceDetails = 5,
        Careers = 6,
        Contact = 7,
        PrivacyPolicy = 8,
        TermsofUse = 9
    }
}
