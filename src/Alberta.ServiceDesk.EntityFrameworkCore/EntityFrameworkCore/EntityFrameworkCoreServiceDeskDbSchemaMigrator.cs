using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Alberta.ServiceDesk.Data;
using Volo.Abp.DependencyInjection;

namespace Alberta.ServiceDesk.EntityFrameworkCore;

public class EntityFrameworkCoreServiceDeskDbSchemaMigrator
    : IServiceDeskDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreServiceDeskDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the ServiceDeskDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<ServiceDeskDbContext>()
            .Database
            .MigrateAsync();
    }
}
