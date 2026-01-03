using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Volo.Abp.Account.Web;
using Volo.Abp.Account.Web.Pages.Account;

namespace Alberta.ServiceDesk.Web.Pages.Account
{
    public class LoginModel : Volo.Abp.Account.Web.Pages.Account.LoginModel
    {
        public LoginModel(
            IAuthenticationSchemeProvider schemeProvider,
            IOptions<AbpAccountOptions> accountOptions)
            : base(schemeProvider, accountOptions)
        {
        }
    }
}