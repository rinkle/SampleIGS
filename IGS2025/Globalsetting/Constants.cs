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
        public const string HomeAtAGlance = "At-A-Glance";
        public const string HomeTransactionsandGrowth = "Transactions and Growth";
        public const string HomeCoreAreasofFocus = "Core Areas of Focus";
        public const string PageHeader = "Page Header";
        public const string TransactionServicesCoreAreasofFocus = "Core Areas of Focus";
        public const string PortfolioServicesCoreAreasofFocus = "Core Areas of Focus";




    }

    public enum PageEnum
    {
        Home = 1,
        TransactionServices = 2,
        PortfolioServices = 3,
        Industries = 4,
        Experience = 5,
        ExperienceDetails = 6,
        Insights = 7,
        InsightInfo = 8,
        Team = 9,
        TeamInfo = 10,
        Careers = 11,
        ContactUs = 12,
        TermsofUse = 13,
        PrivacyPolicy = 14
    }

    public class DbImagePath
    {
        public const string HomeImage = "images/home/";
        public const string HomeCarousel = "images/home/Carousel/";
        public const string ContactImage = "images/contact/";
        public const string pageHeader = "images/pageHeader/";
        public const string TransactionservicesImage = "images/transactionServices/";
        public const string PortfolioServicesImage = "images/portfolioServices/";
        public const string ExperienceImage = "images/Experience/";
        public const string TeamGrid = "images/Team/Grid/";
        public const string TeamBio = "images/Team/Bio/";
        public const string InsightImage = "images/Insight/";





    }

    public class Message
    {
        public const string SuccessMessage = "Saved successfully";
        public const string DeleteSuccessMessage = "Record has been deleted successfully";
        public const string DataNotFoundMessage = "Data not found";
        public const string Error = "Error with Id: ";
        public const string DataNotSaved = "Data can not be saved, please contact admin";
    }
    public class Newstype
    {
        public const string External = "External";
        public const string Internal = "Internal";
        public const string PDF = "PDF";
    }


    public enum Newstypevalue
    {
        Externallink = 1,
        Internallink = 2,
        PDF = 3
    }
}
