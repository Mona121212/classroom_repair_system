using Alberta.ServiceDesk.Samples;
using Xunit;

namespace Alberta.ServiceDesk.EntityFrameworkCore.Applications;

[Collection(ServiceDeskTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<ServiceDeskEntityFrameworkCoreTestModule>
{

}
