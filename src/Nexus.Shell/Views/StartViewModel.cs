using Autofac;
using Nexus.Shared.ViewModels.Base;
using Prism.Events;
using Prism.Regions;

namespace Nexus.Shell.Views {
    public class StartViewModel : CommonViewModel {
        public StartViewModel(ILifetimeScope scope, IEventAggregator eventAggregator, IRegionManager regionManager) 
            : base(scope, eventAggregator, regionManager) {
        }
    }
}
