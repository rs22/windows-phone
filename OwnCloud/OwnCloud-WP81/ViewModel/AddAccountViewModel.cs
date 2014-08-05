using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using OwnCloud.Common.Accounts;
using OwnCloud.Common.Connection;
using OwnCloud.Infrastructure;
using OwnCloud.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnCloud.ViewModel {
    public class AddAccountViewModel : ViewModelBase {
        private ConnectionService _connectionService;
        private AccountService _accountService;
        private INavigationService _navigationService;

        private ConnectionInfo _connectionInfo;

        private string _host;
        public string Host {
            get { return _host; }
            set { Set(() => Host, ref _host, value); }
        }

        private string _userName;
        public string UserName {
            get { return _userName; }
            set {
                Set(() => UserName, ref _userName, value);
                if (AddAccountCommand != null)
                    AddAccountCommand.RaiseCanExecuteChanged();
            }
        }

        private string _password;
        public string Password {
            get { return _password; }
            set {
                Set(() => Password, ref _password, value);
                if (AddAccountCommand != null)
                    AddAccountCommand.RaiseCanExecuteChanged();
            }
        }


        private bool _anonymousAccess;
        public bool AnonymousAccess {
            get { return _anonymousAccess; }
            set {
                Set(() => AnonymousAccess, ref _anonymousAccess, value);
                if (AddAccountCommand != null)
                    AddAccountCommand.RaiseCanExecuteChanged();
            }
        }

        private bool _isHostValid;
        public bool IsHostValid {
            get { return _isHostValid; }
            set {
                Set(() => IsHostValid, ref _isHostValid, value);
                if (AddAccountCommand != null)
                    AddAccountCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand CheckHostCommand { get; set; }
        public RelayCommand AddAccountCommand { get; set; }

        public AddAccountViewModel(ConnectionService connectionService, AccountService accountService, INavigationService navigationService) {
            _connectionService = connectionService;
            _accountService = accountService;
            _navigationService = navigationService;

            AnonymousAccess = false;
            IsHostValid = false;

            CheckHostCommand = new RelayCommand(async () => {
                // TODO: check if really changed
                IsHostValid = false;
                var host = Host;
                if (!host.EndsWith("/")) host = host + "/";

                _connectionInfo = await _connectionService.GetConnectionInfo(host);
                if (_connectionInfo != null) {
                    var auth = await _connectionService.GetAuthenticationInfo(_connectionInfo);
                    if (auth != AuthenticationMethod.Unknown)
                        IsHostValid = true;
                    AnonymousAccess = auth == AuthenticationMethod.None;
                }
            });

            AddAccountCommand = new RelayCommand(async () => {
                var newAccount = new Account {
                    IsAnonymous = AnonymousAccess,
                    Protocol = _connectionInfo.WebDAVUrl.Scheme,
                    ServerDomain = _connectionInfo.WebDAVUrl.Host,
                    WebDAVPath = _connectionInfo.WebDAVUrl.LocalPath
                };

                if (!AnonymousAccess) {
                    newAccount.UsernamePlain = UserName;
                    newAccount.PasswordPlain = Password;
                    await _accountService.UpdateCredentials(newAccount);
                }
                _accountService.AddAccount(newAccount);
                await _accountService.SaveAccounts();

                if (_navigationService.CanGoBack)
                    _navigationService.GoBack();
                else
                    _navigationService.Navigate(typeof(MainPage));
            }, () => IsHostValid &&
                        (AnonymousAccess ||
                        (!string.IsNullOrWhiteSpace(UserName) &&
                             !string.IsNullOrEmpty(Password))));
        }
    }
}
