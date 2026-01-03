using Volo.Abp.Modularity;

namespace Alberta.ServiceDesk;

[DependsOn(
    typeof(ServiceDeskDomainModule),
    typeof(ServiceDeskTestBaseModule)
)]
public class ServiceDeskDomainTestModule : AbpModule
{

}
