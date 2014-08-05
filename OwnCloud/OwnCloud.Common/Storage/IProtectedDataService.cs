using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnCloud.Common.Storage {
    public interface IProtectedDataService {
        Task<string> EncryptString(string input);
        Task<string> DecryptString(string input);
    }
}
