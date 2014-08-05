using System;

namespace OwnCloud.Infrastructure {
    public interface INavigationService {
        void GoBack();
        void Navigate(Type sourcePageType);
        void Navigate(Type sourcePageType, object parameter);

        bool CanGoBack { get; }
    }
}