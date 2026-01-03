using Alberta.ServiceDesk.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Alberta.ServiceDesk.Permissions;

public class ServiceDeskPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(ServiceDeskPermissions.GroupName);

        //Define your own permissions here. Example:
        //myGroup.AddPermission(ServiceDeskPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ServiceDeskResource>(name);
    }
}
