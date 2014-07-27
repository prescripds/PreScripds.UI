using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;


namespace PreScripds.Infrastructure
{
    public static class Constants
    {
        public static string SiteSession = "SiteSession"; // Session Key
        public const string ERRORFILE = "ErrorFile";
        public const string TEMP_FILE_REPOSITORY_PATH = "~/Assets/ImageUpload"; //Temporary file uplaod path

       

        public const string UserPubs = "UserPubs";
        public static class CacheKeys
        {
            public static string DEPARTMENTS = "Departments";
            public static string PERMISSION = "Permission";
            public static string COUNTRY = "Country";
            public static string STATE = "State";
            public static string CITY = "City";
            public static string MODULE = "Module";
            public static string CODE_VALUE = "CodeValue";
            public static string SECURITY_QUESTION = "SecurityQuestions";
            public static string PUB_MEMBERSHIP = "PubMembership";
            public static string MASTER_SOCIAL = "MasterSocial";
            public static string ALL_BRANDS = "AllBrands";
        }

        public static class ApplicationConstants
        {
            public static int DEFAULT_PAGE_COUNT = 0;
            public static int DEFAULT_LOCALE = 1033;
            public static int DEFAULT_CUSTOMER_SOURCE = 3;
            public static string EXCEL_CONTENT_TYPE = "application/ms-excel";
            public static string EXCEL_CUSTOMER_CONTENT_TYPE = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            public static string EXCEL_FILE_NAME_CUSTOMER_LIST = "CustomerList";
            public static string EXCEL_FILE_NAME_CUSTOMER_REPORT = "CustomerReport.xls";
            public static string EXCEL_FILE_NAME_OCCASION_REPORT = "OccasionReport.xls";
            public static string EXCEL_FILE_NAME_SOCIAL_REPORT = "SocialReport.xls";
            public static string EXCEL_FILE_NAME_ENGAGEMENT_REPORT = "EngagementReport.xls";
            public static string EXCEL_FILE_NAME_BANQUET_REPORT = "BanquetEnquiryReport.xls";
            public static string FILE_SIZE = "FileSize";
            public static string LOGO_FILE = "_logo";
            public static string HEADER_FILE = "_header";
            public static string FOOTER_FILE = "_footer";
            public static string CONTENT_BACKGROUND_FILE = "_contentbackground";
            public static string CONTENT_FILE = "_content";
            public static string ASSETS_FOLDER = "assets";
            public static string EMAIL_ASSETS_FOLDER = "emailassets";
            public static string NEWSLETTER_ASSETS_FOLDER = "newsletterassets";
            public static string PROMOTION_ASSETS_FOLDER = "promotionassets";


            public static string DEFAULT_PASSWORD = "Pass@321";
            public static int MINIMUM_PASSWORD_COUNT = 8;
            public static int MAXIMUM_PASSWORD_COUNT = 16;

            public static string SOCIAL = "Social";
            public static string ONLINE_REVIEWS = "Online Reviews";

            public static string ABOVE25 = "Above 25";
            public static string BELOW25 = "Below 25";
            public static string UNASSIGNED = "Unassigned";
            public const string BIRTHDAY = "Birthdays";
            public const string ANNIVERSARY = "Anniversaries";
            public const int MAX_SIZE = 50000;

        }

        public static class ConfigKeys
        {
            public static string PAGE_SIZE = "PageSize";
            public static string MAX_SIZE = "MaxSize";
            public static string MAX_DB_COUNT = "MaxDbCount";
            public static string ASSETS_BASE_PATH = "AssetsBasePath";
        }
        public static class ReportFont
        {


        }
        public static T GetConfigValue<T>(string configKey)
        {
            return ConfigurationManager.AppSettings[configKey].As<T>();
        }

        public static string GetConfigValue(string configKey)
        {
            return ConfigurationManager.AppSettings[configKey];
        }
    }
}
