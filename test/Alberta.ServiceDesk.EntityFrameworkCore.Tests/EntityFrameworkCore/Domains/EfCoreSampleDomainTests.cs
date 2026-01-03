using Alberta.ServiceDesk.Samples;
using Xunit;

namespace Alberta.ServiceDesk.EntityFrameworkCore.Domains;

[Collection(ServiceDeskTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<ServiceDeskEntityFrameworkCoreTestModule>
{

}
