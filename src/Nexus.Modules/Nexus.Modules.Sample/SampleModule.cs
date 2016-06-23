using Autofac;
using Nexus.Shared.Modularity.Base;
using Prism.Events;
using Prism.Logging;
using Prism.Regions;

namespace Nexus.Modules.Sample {
    /// <summary>
    /// Prism sample module.
    /// 
    /// Remember:
    ///     To use the new module, one needs to install the NuGet package 'XAMLMarkupExtensions'
    ///     and add a reference to System.Xaml
    /// </summary>
    public class SampleModule : ModuleBase {
        /// <summary>
        /// Creates a new instance of the SampleModule class, which is used by 
        /// the PRISM framework to load the dll at runtime and 'integrates' it
        /// into the shell application.
        /// </summary>
        /// <param name="regionManager">Prism's RegionManager</param>
        /// <param name="eventAggregator">Prism's EventAggregator</param>
        /// <param name="logger">Prism's default logger</param>
        public SampleModule(IRegionManager regionManager, IEventAggregator eventAggregator, ILoggerFacade logger, ILifetimeScope scope)
            : base(logger, eventAggregator, regionManager, scope) {

        }

        /// <summary>
        /// Uses PRISM's region manager to register all views of the module when loaded.
        /// </summary>
        protected override void RegisterViews() {
            //_regionManager.RegisterViewWithRegion(ShellRegion.Ribbon, typeof(FinancesModuleRibbon));
            //_regionManager.RegisterViewWithRegion(ShellRegion.MainContent, typeof(TaxRateView));
        }
    }
}
