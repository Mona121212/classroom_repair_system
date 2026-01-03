using Xunit;

namespace Alberta.ServiceDesk.EntityFrameworkCore;

[CollectionDefinition(ServiceDeskTestConsts.CollectionDefinitionName)]
public class ServiceDeskEntityFrameworkCoreCollection : ICollectionFixture<ServiceDeskEntityFrameworkCoreFixture>
{

}
