using Microsoft.AspNetCore.Mvc;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Techtoniq.Example.Controllers
{
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    [AutoValidateAntiforgeryToken]
    public class ExampleController : BasePluginController
    {
        #region Fields

        private readonly IPermissionService _permissionService;

        #endregion Fields

        #region Ctor

        public ExampleController (
            IPermissionService permissionService
        )
        {
            _permissionService = permissionService;
        }

        #endregion Ctor

        public IActionResult Configure()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageExternalAuthenticationMethods))
                return AccessDeniedView();

            return View($"~/Plugins/{Domain.Constants.Plugin.SYSTEM_NAME}/Views/Configure.cshtml");
        }
    }
}
