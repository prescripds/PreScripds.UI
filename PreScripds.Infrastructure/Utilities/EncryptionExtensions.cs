using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USL.ClubD.Security;


namespace PreScripds.Infrastructure
{
    public class EncryptionExtensions
    {
        public static string CreatePasswordHash(string password, string salt)
        {
            var encriptionServcie = new EncryptionService();
            return encriptionServcie.CreatePasswordHash(password, salt);
        }

        public static string CreateSaltKey()
        {
            var encriptionServcie = new EncryptionService();
            return encriptionServcie.CreateSaltKey();
        }

        public static string Encrypt(string encryptKey)
        {
            var rjEncrypt = new RijndaelEnhanced(RijndaelEnhanced.passPhrase, RijndaelEnhanced.initVector);
            return rjEncrypt.Encrypt(encryptKey);
        }
        public static string Decrypt(string decryptKey)
        {
            var rjDecrypt = new RijndaelEnhanced(RijndaelEnhanced.passPhrase, RijndaelEnhanced.initVector);
            return rjDecrypt.Decrypt(decryptKey);
        }
    }
}
