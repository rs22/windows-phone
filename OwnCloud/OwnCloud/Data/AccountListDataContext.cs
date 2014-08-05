using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using OwnCloud.Common.Accounts;
using OwnCloud.Storage;
using System.Threading.Tasks;

namespace OwnCloud.Data
{
    public class AccountListDataContext : Entity
    {
        public AccountListDataContext()
        {
            Loaddata();
        }

        public ObservableCollection<Account> Accounts
        {
            get;
            set;
        }

        public async void Loaddata()
        {
            Accounts = new ObservableCollection<Account>();
            
            // protect us from zombie binding trigger values
            //App.DataContext.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, App.DataContext.Accounts);
            foreach (var account in await App.AccountService.GetAccounts())
                Accounts.Add(account);
        }
    }
}
