using System.Configuration;

namespace PreScripds.Infrastructure
{
    public class CacheSettings:ConfigurationSection 
    {
        static class Constants
        {
            public const string CacheDurationInMinutes = "cacheDurationInMinutes";
            public const string Enabled = "enabled";
            public const string Provider = "provider";
        } 

        private static readonly CacheSettings Settings = ConfigurationManager.GetSection("cache") as CacheSettings;

        public static CacheSettings Instance
        {
            get
            {
                return Settings ?? new CacheSettings();
            }
        }

        [ConfigurationProperty(Constants.CacheDurationInMinutes, DefaultValue = 5, IsRequired = false)]
        [IntegerValidator(MinValue = 1)]
        public int CacheDurationInMinutes
        {
            get { return (int)this[Constants.CacheDurationInMinutes]; }
            set { this[Constants.CacheDurationInMinutes] = value; }
        }

        [ConfigurationProperty(Constants.Provider, DefaultValue = null, IsRequired = false)]
        public string Provider
        {
            get { return (string)this[Constants.Provider]; }
            set { this[Constants.Provider] = value; }
        }

        [ConfigurationProperty(Constants.Enabled, DefaultValue = false, IsRequired = false)]
        public bool Enabled
        {
            get { return (bool)this[Constants.Enabled]; }
            set { this[Constants.Enabled] = value; }
        }
    }
}
