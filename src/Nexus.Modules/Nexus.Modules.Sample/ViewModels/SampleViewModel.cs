using Autofac;
using Nexus.Shared.ViewModels.Base;
using Prism.Events;
using Prism.Regions;

namespace Nexus.Modules.Sample.ViewModels {
    public class SampleViewModel : CommonViewModel, INavigationAware {
        #region INavigationAware Members

        public void OnNavigatedTo(NavigationContext navigationContext) {
            IsActive = true;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext) {
            IsActive = false;
        }

        #endregion


        #region Initialization

        /// <summary>
        /// Creates a new instance of the SampleViewModel
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="eventAggregator"></param>
        /// <param name="regionManager"></param>
        public SampleViewModel(ILifetimeScope scope, IEventAggregator eventAggregator, IRegionManager regionManager)
            : base(scope, eventAggregator, regionManager) {

        }

        #endregion
    }
}
