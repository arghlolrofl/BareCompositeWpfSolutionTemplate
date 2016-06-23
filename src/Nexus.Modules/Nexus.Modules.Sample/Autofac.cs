using Autofac;

namespace Nexus.Modules.Sample {
    /// <summary>
    /// Sample Module definition for IoC-registrations.
    /// 
    /// Module's View and ViewModel registrations should be placed here.
    /// </summary>
    public class AutofacModule : Module {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder) {
            base.Load(builder);

            builder.RegisterType<SampleModule>();
        }
    }
}
