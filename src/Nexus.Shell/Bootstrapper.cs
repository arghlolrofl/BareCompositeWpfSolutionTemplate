using Autofac;
using Microsoft.Practices.ServiceLocation;
using Nexus.Contracts.Events;
using Nexus.Shared.Events;
using Nexus.Shared.Views;
using Nexus.Shell.Views;
using Prism.Autofac;
using Prism.Events;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using Prism.RibbonRegionAdapter;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls.Ribbon;

namespace Nexus.Shell {
    public class Bootstrapper : AutofacBootstrapper {
        protected override IModuleCatalog CreateModuleCatalog() {
            return new ConfigurationModuleCatalog();
        }

        /// <summary>
        /// Configures the module catalog for the application
        /// </summary>
        protected override void ConfigureModuleCatalog() {
            base.ConfigureModuleCatalog();

            // Modules that should be loaded ...
            // Loading by reflection is also possible here
            IEnumerable<Type> modules = new Type[] {
                //typeof(Modules.Sample.SampleModule),
            };

            // ... are added to the catalog here
            foreach (Type module in modules) {
                ModuleCatalog.AddModule(new ModuleInfo() {
                    ModuleName = module.Name,
                    ModuleType = module.AssemblyQualifiedName,
                    InitializationMode = InitializationMode.WhenAvailable
                });
            }
        }

        protected override void ConfigureContainerBuilder(ContainerBuilder builder) {
            base.ConfigureContainerBuilder(builder);

            builder.RegisterType<RibbonRegionAdapter>();

            builder.RegisterType<ModuleLoadEvent>().As<IModuleLoadEvent>();

            builder.RegisterType<MainWindow>();
            builder.RegisterType<MainViewModel>().SingleInstance();
        }

        protected override RegionAdapterMappings ConfigureRegionAdapterMappings() {
            var mappings = base.ConfigureRegionAdapterMappings();

            IRegionAdapter adapter = ServiceLocator.Current.GetInstance<RibbonRegionAdapter>();
            mappings.RegisterMapping(typeof(Ribbon), adapter);

            return mappings;
        }

        protected override void ConfigureViewModelLocator() {
            base.ConfigureViewModelLocator();

            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((viewType) => {
                var ns = viewType.FullName.Substring(0, viewType.FullName.LastIndexOf('.') + 1);
                var prefix = viewType.FullName.Replace(ns, String.Empty)
                                              .Replace("View", String.Empty)
                                              .Replace("Window", String.Empty);

                return Type.GetType(
                    String.Format(CultureInfo.InvariantCulture,
                        "{0}ViewModel, {1}",
                        ns + prefix,
                        viewType.Assembly.FullName
                    ));
            });

            //ViewModelLocationProvider.SetDefaultViewModelFactory(
            //    (t) => Container.Resolve(t)
            //);
        }


        protected override DependencyObject CreateShell() {
            ServiceLocator.Current
                          .GetInstance<IEventAggregator>()
                          .GetEvent<PubSubEvent<IModuleLoadEvent>>()
                          .Subscribe(PrismModule_OnLoad);

            return ServiceLocator.Current
                                 .GetInstance<MainWindow>();
        }

        protected override void InitializeShell() {
            Application.Current.MainWindow = (MainWindow)Shell;
            Application.Current.MainWindow.Show();

            var regionManager = ServiceLocator.Current.GetInstance<IRegionManager>();
            regionManager.RegisterViewWithRegion(ShellRegion.MainContent, typeof(StartView));
        }

        /// <summary>
        /// When loading a prism module (IModule.Initialize), we need to update the IoC container too.
        /// The Autofac.Module will be passed inside the <see cref="ModuleLoadEvent"/> class.
        /// </summary>
        /// <param name="e">Class containing the module configuration to be updated.</param>
        private void PrismModule_OnLoad(IModuleLoadEvent e) {
            var updater = new ContainerBuilder();
            updater.RegisterModule(e.Module);
            updater.Update(Container);
        }
    }
}
