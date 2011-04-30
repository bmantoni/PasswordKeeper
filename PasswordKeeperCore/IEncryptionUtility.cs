using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordKeeperCore.Encryption
{
    public interface IEncryptionUtility
    {
        string EncryptString (
            string inputText,
            string key );

        string DecryptString (
            string encryptedText,
            string key );

        byte [] EncryptBinary (
            byte [] input,
            string key );

        byte [] DecryptBinary (
            byte [] encryptedBytes,
            string key );
    }
}
