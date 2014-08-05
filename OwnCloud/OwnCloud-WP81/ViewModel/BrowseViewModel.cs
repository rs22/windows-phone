using GalaSoft.MvvmLight;
using OwnCloud.Common.Accounts;
using OwnCloud.WebDAV;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnCloud.ViewModel {
    public class BrowseViewModel : ViewModelBase {
        private AccountService _accountService;
        private Account _account;

        private ObservableCollection<WebDAVFile> _files;
        public ObservableCollection<WebDAVFile> Files {
            get { return _files; }
            set { Set(() => Files, ref _files, value); }
        }

        private string _accountName;
        public string AccountName {
            get { return _accountName; }
            set { Set(() => AccountName, ref _accountName, value); }
        }

        public BrowseViewModel(AccountService accountService) {
            _accountService = accountService;
        }

        public async void LoadData(Account account) {
            _account = account;
            AccountName = _account.Name;

            var webDav = new WebDAVClient(_account.GetUri(), await _accountService.GetCredentials(_account));
            try {
                var files = await webDav.GetEntries(_account.WebDAVPath, true);
                Files = new ObservableCollection<WebDAVFile>
                    // except current folder
                    (files.Where(x => x.FilePath.ToString() != _account.WebDAVPath));
            } catch {

            }
        }

        public void OnNavigatedTo(object navigationParameter) {
            if (navigationParameter as Account != null)
                LoadData(navigationParameter as Account);
        }
    }
}
