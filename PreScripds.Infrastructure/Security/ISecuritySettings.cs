using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreScripds.Infrastructure.Security
{
    public interface ISecuritySettings
    {

        /// <summary>
        ///     Gets or sets an encryption key
        /// </summary>
        string EncryptionKey { get; set; }

        /// <summary>
        ///     Gets or sets a list of adminn area allowed IP addresses
        /// </summary>
        List<string> AdminAreaAllowedIpAddresses { get; set; }

        /// <summary>
        ///     Gets or sets a vaule indicating whether to hide admin menu items based on ACL
        /// </summary>
        bool HideAdminMenuItemsBasedOnPermissions { get; set; }

    }
}
