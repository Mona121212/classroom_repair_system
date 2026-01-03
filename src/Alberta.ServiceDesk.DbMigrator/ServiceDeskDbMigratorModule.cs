using Alberta.ServiceDesk.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Alberta.ServiceDesk.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(ServiceDeskEntityFrameworkCoreModule),
    typeof(ServiceDeskApplicationContractsModule)
)]
public class ServiceDeskDbMigratorModule : AbpModule
{
}
