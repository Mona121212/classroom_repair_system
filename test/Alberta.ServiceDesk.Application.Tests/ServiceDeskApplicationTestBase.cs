using Volo.Abp.Modularity;

namespace Alberta.ServiceDesk;

public abstract class ServiceDeskApplicationTestBase<TStartupModule> : ServiceDeskTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
