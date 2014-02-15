using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreScripds.Infrastructure.Security
{
    public class SecuritySettings : ISecuritySettings
    {
        #region Implementation of ISecuritySettings

        public string EncryptionKey { get; set; }
        public List<string> AdminAreaAllowedIpAddresses { get; set; }
        public bool HideAdminMenuItemsBasedOnPermissions { get; set; }

        #endregion
    }
}
