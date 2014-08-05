using System;
using System.Linq;
using Microsoft.Phone.Shell;
using OwnCloud.Data;
using OwnCloud.Common.Accounts;
using OwnCloud.Storage;
using System.Threading.Tasks;

namespace OwnCloud.Extensions
{
    public static class TileHelper
    {

        public static async Task AddCalendarToTile(Guid _accountID)
        {
            string name = Resource.Localization.AppResources.Tile_KalendarTitle;

            try
            {
                using (var context = new OwnCloudDataContext())
                {
                    var account = await App.AccountService.GetAccountByID(_accountID);
                    // TODO: this will edit the account in the list
                    await App.AccountService.RestoreCredentials(account);
                    name = account.Username + " " + name;
                }
            }
// ReSharper disable EmptyGeneralCatchClause
            catch
// ReSharper restore EmptyGeneralCatchClause
            {
                //Do nothing
            }

            var invokeUrl = new Uri( "/View/Page/CalendarMonthPage.xaml?uid=" + _accountID.ToString(), UriKind.Relative);

            PinUrlToStart(invokeUrl, name, "Resource/Image/CalendarLogo.png");
        }

        public static async Task AddOnlineFilesToTile(Guid _accountID)
        {
            string name = Resource.Localization.AppResources.Tile_RemoteFileTitle;

            try
            {
                using (var context = new OwnCloudDataContext())
                {
                    var account = await App.AccountService.GetAccountByID(_accountID);
                    // TODO: this will edit the account in the list
                    await App.AccountService.RestoreCredentials(account);
                    name = account.Username + " " + name;
                }
            }
            // ReSharper disable EmptyGeneralCatchClause
            catch
            // ReSharper restore EmptyGeneralCatchClause
            {
                //Do nothing
            }

            var invokeUrl = new Uri("/View/Page/RemoteFiles.xaml?account=" + _accountID, UriKind.Relative);

            PinUrlToStart(invokeUrl, name, "Resource/Image/RemoteFolderLogo.png");
        }


        private static void PinUrlToStart(Uri invokeUrl, string name, string logoUrl)
        {
            if (ShellTile.ActiveTiles.Any(o => o.NavigationUri.Equals(invokeUrl)))
                return;

            ShellTile.Create(invokeUrl, new StandardTileData()
                {
                    Title = name,
                    BackgroundImage = new Uri(logoUrl, UriKind.Relative)
                });
        }
    }
}
