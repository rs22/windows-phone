using OwnCloud.Common.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OwnCloud.Common.Accounts {
    public class AccountService {
        private const string FILE_NAME = "accounts.xml";
        IProtectedDataService _protectedDataService;
        ISerializer _serializer;
        List<Account> _accounts;

        public AccountService(IProtectedDataService protectedDataService, ISerializer serializer) {
            _protectedDataService = protectedDataService;
            _serializer = serializer;
        }

        public async Task<List<Account>> GetAccounts() {
            if (_accounts != null)
                return _accounts;

            _accounts = await _serializer.Open<List<Account>>(FILE_NAME);
            return _accounts;
        }

        public async Task<Account> GetAccountByID(Guid id) {
            return (await GetAccounts()).SingleOrDefault(x => x.GUID == id);
        }

        /// <summary>
        /// Returns the Username & Password for
        /// the account. This works also in encrypted mode.
        /// </summary>
        /// <returns></returns>
        public async Task<NetworkCredential> GetCredentials(Account account) {
            var copy = account.GetCopy();
            if (!copy.IsAnonymous) await RestoreCredentials(copy);
            return copy.IsAnonymous ? new NetworkCredential() : new NetworkCredential(copy.UsernamePlain, copy.PasswordPlain);
        }

        /// <summary>
        /// Encrypts username and password text
        /// </summary>
        public async Task UpdateCredentials(Account account) {
            if (!account.IsAnonymous) {
                if (!string.IsNullOrEmpty(account.UsernamePlain)) {
                    account.Username = await _protectedDataService.EncryptString(account.UsernamePlain);
                } else {
                    account.Username = string.Empty;
                }
                if (!string.IsNullOrEmpty(account.PasswordPlain)) {
                    account.Password = await _protectedDataService.EncryptString(account.PasswordPlain);
                } else {
                    account.Password = string.Empty;
                }
            } else {
                account.Username = account.Password = string.Empty;
            }
        }

        /// <summary>
        /// Decrypts username and password text
        /// </summary>
        public async Task RestoreCredentials(Account account) {
            if (!account.IsAnonymous && (string.IsNullOrEmpty(account.UsernamePlain)|| string.IsNullOrEmpty(account.PasswordPlain))) {
                account.UsernamePlain = await _protectedDataService.DecryptString(account.Username);
                account.PasswordPlain = await _protectedDataService.DecryptString(account.Password);
            }
        }

        public Task SaveAccounts() {
            return _serializer.Save(FILE_NAME, _accounts);
        }

        public void AddAccount(Account account) {
            account.GUID = Guid.NewGuid();
            _accounts.Add(account);
        }

        public async Task DeleteAccount(Account account) {
            var accountInList = await GetAccountByID(account.GUID);
            _accounts.Remove(accountInList);
        }
    }
}
