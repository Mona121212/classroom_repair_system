using System.Threading.Tasks;

namespace Alberta.ServiceDesk.Data;

public interface IServiceDeskDbSchemaMigrator
{
    Task MigrateAsync();
}
