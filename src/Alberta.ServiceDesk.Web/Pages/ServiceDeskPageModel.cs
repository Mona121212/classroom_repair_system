using Alberta.ServiceDesk.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Alberta.ServiceDesk.Web.Pages;

public abstract class ServiceDeskPageModel : AbpPageModel
{
    protected ServiceDeskPageModel()
    {
        LocalizationResourceType = typeof(ServiceDeskResource);
    }
}
