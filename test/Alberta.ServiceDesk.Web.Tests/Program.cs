using Microsoft.AspNetCore.Builder;
using Alberta.ServiceDesk;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();
builder.Environment.ContentRootPath = GetWebProjectContentRootPathHelper.Get("Alberta.ServiceDesk.Web.csproj"); 
await builder.RunAbpModuleAsync<ServiceDeskWebTestModule>(applicationName: "Alberta.ServiceDesk.Web");

public partial class Program
{
}
