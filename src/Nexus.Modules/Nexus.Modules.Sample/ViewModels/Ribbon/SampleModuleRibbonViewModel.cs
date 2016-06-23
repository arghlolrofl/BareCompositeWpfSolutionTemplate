using Autofac;
using Nexus.Modules.Sample.Views;
using Nexus.Shared.ViewModels.Base;
using Nexus.Shared.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System.Windows.Input;

namespace Nexus.Modules.Sample.ViewModels.Ribbon {
    class SampleModuleRibbonViewModel : CommonViewModel {
        #region Commands

        private ICommand _goToSampleViewCommand;

        public ICommand GoToSampleViewCommand {
            get { return _goToSampleViewCommand ?? (_goToSampleViewCommand = new DelegateCommand(GoToSampleViewCommand_OnExecute)); }
        }

        #endregion


        public SampleModuleRibbonViewModel(ILifetimeScope scope, IRegionManager regionManager, IEventAggregator eventAggregator)
            : base(scope, eventAggregator, regionManager) {

        }


        private void GoToSampleViewCommand_OnExecute() {
            NavigateToView(nameof(SampleView), ShellRegion.MainContent);
        }
    }
}
