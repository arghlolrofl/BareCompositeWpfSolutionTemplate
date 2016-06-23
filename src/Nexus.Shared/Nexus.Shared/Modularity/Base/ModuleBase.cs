using Autofac;
using Nexus.Contracts.Events;
using Prism.Events;
using Prism.Logging;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Linq;

namespace Nexus.Shared.Modularity.Base {
    public abstract class ModuleBase : IModule {
        protected IEventAggregator _eventAggregator;
        protected ILoggerFacade _logger;
        protected IRegionManager _regionManager;
        protected ILifetimeScope _scope;

        public ModuleBase(ILoggerFacade logger, IEventAggregator eventAggregator, IRegionManager regionManager, ILifetimeScope scope) {
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;
            _logger = logger;
            _scope = scope;
        }

        /// <summary>
        /// Initializes and loads a Prism module.
        /// </summary>
        public virtual void Initialize() {
            Log(" ==> Loading " + GetType().Name);

            Log("   > Updating IoC container");
            RequestContainerUpdate();

            Log("   > Registering views");
            RegisterViews();

            Log("   > " + GetType().Name + " loaded successfully");
        }

        /// <summary>
        /// Updates the IoC-Container at runtime, when a module is being loaded,
        /// with the module registrations.
        /// </summary>
        protected virtual void RequestContainerUpdate() {
            Type[] assemblyTypes = GetType().Assembly.GetTypes();
            Type type = assemblyTypes.Single(t => t.IsSubclassOf(typeof(Autofac.Module)));

            Module moduleToLoad = (Module)type.GetConstructor(new Type[0]).Invoke(null);

            IModuleLoadEvent loadEvent = _scope.Resolve<IModuleLoadEvent>();
            loadEvent.Module = moduleToLoad;

            _eventAggregator.GetEvent<PubSubEvent<IModuleLoadEvent>>()
                            .Publish(loadEvent);
        }

        /// <summary>
        /// Prints module messages to debug output.
        /// </summary>
        /// <param name="message"></param>
        protected void Log(string message) {
            _logger.Log("MODULE: " + message, Category.Debug, Priority.Low);
        }

        /// <summary>
        /// Override in inherited classes to register all views of the module with
        /// Prism's RegionManager.
        /// </summary>
        protected abstract void RegisterViews();
    }
}
