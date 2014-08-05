using OwnCloud.Common.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.DataProtection;

namespace OwnCloud.Storage {
    public class ProtectedDataService : IProtectedDataService {
        public async Task<string> EncryptString(string input) {
            if (input == null) return "";
            var provider = new DataProtectionProvider("LOCAL=user");
            var inputBuffer = CryptographicBuffer.ConvertStringToBinary(input, BinaryStringEncoding.Utf8);
            var crypted = await provider.ProtectAsync(inputBuffer);
            return CryptographicBuffer.EncodeToBase64String(crypted);
        }

        public async Task<string> DecryptString(string input) {
            var provider = new DataProtectionProvider();
            var inputBuffer = CryptographicBuffer.DecodeFromBase64String(input);
            var decrypted = await provider.UnprotectAsync(inputBuffer);
            return CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, decrypted);
        }
    }
}
