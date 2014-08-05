using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using OwnCloud.Common.Accounts;
using OwnCloud.Infrastructure;
using OwnCloud.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnCloud.ViewModel {
    public class MainViewModel : ViewModelBase {
        private AccountService _accountService;
        private INavigationService _navigationService;

        private ObservableCollection<Account> _accounts;

        public ObservableCollection<Account> Accounts {
            get { return _accounts; }
            set { Set(() => Accounts, ref _accounts, value); }
        }

        public RelayCommand AddAccountCommand { get; set; }

        public RelayCommand<Account> BrowseAccountCommand { get; set; }

        public MainViewModel(AccountService accountService, INavigationService navigationService) {
            _accountService = accountService;
            _navigationService = navigationService;

            if (IsInDesignMode)
                LoadData();

            AddAccountCommand = new RelayCommand(() => {
                _navigationService.Navigate(typeof(AddAccountPage));
            });

            BrowseAccountCommand = new RelayCommand<Account>(account => {
                _navigationService.Navigate(typeof(BrowsePage), account);
            });
        }

        public async void LoadData() {
            if (IsInDesignMode) {
                Accounts = new ObservableCollection<Account>(new[] {
                    new Account { ServerDomain="owncloud.org" }
                });
            } else {
                Accounts = new ObservableCollection<Account>(
                    await _accountService.GetAccounts());
            }
        }

        public void OnNavigatedTo(object navigationParameter) {
            LoadData();
        }
    }
}
