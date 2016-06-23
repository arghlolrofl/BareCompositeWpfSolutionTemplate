using Autofac;
using Nexus.Contracts.Events;

namespace Nexus.Shared.Events {
    public class ModuleLoadEvent : IModuleLoadEvent {
        public Module Module { get; set; }


        public ModuleLoadEvent() {

        }

        public ModuleLoadEvent(Module module) {
            Module = module;
        }
    }
}
