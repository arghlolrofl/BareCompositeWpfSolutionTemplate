using Autofac;
using Nexus.Shared.ViewModels.Base;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System.Windows.Input;

namespace Nexus.Shell.Views {
    public class MainViewModel : CommonViewModel {
        #region Commands

        private ICommand _shutdownCommand;
        public ICommand ShutdownCommand {
            get {
                return _shutdownCommand ?? (_shutdownCommand = new DelegateCommand(ShutdownApplication));
            }
        }

        #endregion

        private string _windowTitle;

        public string WindowTitle {
            get { return _windowTitle; }
            set { _windowTitle = value; RaisePropertyChanged(); }
        }


        public MainViewModel(ILifetimeScope scope, IEventAggregator eventAggregator, IRegionManager regionManager)
            : base(scope, eventAggregator, regionManager) {
            WindowTitle = "Sample Shell Window";
        }

        private void ShutdownApplication() {
            App.Current.Shutdown();
        }

    }
}
