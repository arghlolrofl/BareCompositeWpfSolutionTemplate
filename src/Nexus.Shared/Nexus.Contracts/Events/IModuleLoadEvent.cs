using Autofac;

namespace Nexus.Contracts.Events {
    public interface IModuleLoadEvent {
        Module Module { get; set; }
    }
}
