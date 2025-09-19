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

    public class DbImagePath
    {
        public const string HomeImage = "images/home/";
        public const string HomeCarousel = "images/home/Carousel/";
        public const string ContactImage = "images/contact/";
        public const string pageHeader = "images/pageHeader/";
        public const string CriteriaDataImage = "images/Criteria/";
        public const string PortHeader = "images/pageHeader/portfolio/";
        #region Team

        public const string TeamGrid = "images/Team/Grid/";
        public const string TeamBio = "images/Team/Bio/";
        public const string TeamOnHome = "images/Team/Home/";

        public const string ChildhoodPic = "images/Team/Childhood/";
        public const string TeamHeader = "images/Team/Header/";
        #endregion

        #region Experience
        public const string ExperienceOriginal = "images/Experience/Logo/Original/";
        public const string ExperienceThumbnail = "images/Experience/Logo/Thumbnail/";
        #endregion

        public const string Careers = "images/Careers/";

        #region vcard
        public const string vCard = "images/Team/vCard/";
        #endregion

        #region Company details Images
        public const string CompanyDetailsHeader = "images/Company/Header/";
        #endregion

        public const string Strategy = "images/Strategy/";
        public const string StrategyInvestmentCriteria = "images/Strategy/InvestmentCriteria/";
        public const string StrategyTransactionType = "images/Strategy/TransactionType/";
        public const string StrategyKeyDifferentiator = "images/Strategy/KeyDifferentiator/";
        public const string ContactInvestmentLeadership = "images/Contact/Members/InvestmentLeadership/";
        public const string ContactOperationsLeadership = "images/Contact/Members/OperationsLeadership/";
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
