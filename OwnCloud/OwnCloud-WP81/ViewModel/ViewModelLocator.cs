using CommonServiceLocator.NinjectAdapter;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Ninject;
using OwnCloud.Common.Accounts;
using OwnCloud.Common.Connection;
using OwnCloud.Common.Storage;
using OwnCloud.Infrastructure;
using OwnCloud.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnCloud.ViewModel {
    public class ViewModelLocator {
        public MainViewModel Main {
            get {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public BrowseViewModel Browse {
            get {
                return ServiceLocator.Current.GetInstance<BrowseViewModel>();
            }
        }

        public AddAccountViewModel AddAccount {
            get {
                return ServiceLocator.Current.GetInstance<AddAccountViewModel>();
            }
        }

        static ViewModelLocator() {
            var kernel = new StandardKernel();
            ServiceLocator.SetLocatorProvider(() => new NinjectServiceLocator(kernel));
            kernel.Bind<ISerializer>().To<Serializer>();
            kernel.Bind<IProtectedDataService>().To<ProtectedDataService>();
            kernel.Bind<INavigationService>().To<NavigationService>();

            kernel.Bind<AccountService>().ToSelf().InSingletonScope();
        }
    }
}
