using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PreScripds.Infrastructure.Security;

namespace PreScripds.UI.Common
{
    public class Common
    {
        public static string CreatePasswordHash(string password, string salt)
        {
            var encriptionServcie = new EncryptionService();
            return encriptionServcie.CreatePasswordHash(password, salt);
        }
    }
}