using Alberta.ServiceDesk.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Alberta.ServiceDesk.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class ServiceDeskController : AbpControllerBase
{
    protected ServiceDeskController()
    {
        LocalizationResource = typeof(ServiceDeskResource);
    }
}
