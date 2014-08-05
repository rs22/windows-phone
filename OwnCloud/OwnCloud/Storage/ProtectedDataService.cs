using OwnCloud.Common.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OwnCloud.Storage {
    public class ProtectedDataService : IProtectedDataService {
        public Task<string> EncryptString(string input) {
            if (input == null) return TaskEx.FromResult("");
            byte[] crypted = ProtectedData.Protect(Encoding.UTF8.GetBytes(input), null);
            return TaskEx.FromResult(Convert.ToBase64String(crypted, 0, crypted.Length));
        }

        public Task<string> DecryptString(string input) {
            if (input == null) return TaskEx.FromResult("");
            byte[] decrypted = ProtectedData.Unprotect(System.Convert.FromBase64String(input), null);
            return TaskEx.FromResult(Encoding.UTF8.GetString(decrypted, 0, decrypted.Length));
        }
    }
}
