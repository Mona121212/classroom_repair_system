using Volo.Abp.Modularity;

namespace Alberta.ServiceDesk;

[DependsOn(
    typeof(ServiceDeskApplicationModule),
    typeof(ServiceDeskDomainTestModule)
)]
public class ServiceDeskApplicationTestModule : AbpModule
{

}
