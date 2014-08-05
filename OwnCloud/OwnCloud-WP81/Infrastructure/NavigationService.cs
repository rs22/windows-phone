using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OwnCloud.Infrastructure {
    public class NavigationService : INavigationService {
        public void Navigate(Type sourcePageType) {
            ((Frame)Window.Current.Content).Navigate(sourcePageType);
        }
        public void Navigate(Type sourcePageType, object parameter) {
            ((Frame)Window.Current.Content).Navigate(sourcePageType, parameter);
        }
        public void GoBack() {
            ((Frame)Window.Current.Content).GoBack();
        }

        public bool CanGoBack {
            get {
                return ((Frame)Window.Current.Content).CanGoBack;
            }
        }
    }
}
