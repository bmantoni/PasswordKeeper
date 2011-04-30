using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using PasswordKeeperCore.Encryption;

namespace PasswordKeeperWpf
{
    public static class EncryptionUtilityFactory
    {
        public static IEncryptionUtility BuildEncryptionUtility ()
        {
            Type t = Type.GetType ( GetEncryptionUtilityClass() );

            IEncryptionUtility util = ( IEncryptionUtility )
                Activator.CreateInstance ( t );

            return util;
        }

        private static string GetEncryptionUtilityClass ()
        {
            return ConfigurationManager.AppSettings[ "encryptionUtility" ];
        }

        //private const string RIJNDAEL_CIPHER_ENCRYPTION =
        //@"PasswordKeeperWpf.Encryption.RijndaelEncryption, PasswordKeeperWpf";
    }
}
