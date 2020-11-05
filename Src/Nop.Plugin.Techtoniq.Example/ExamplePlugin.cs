using Nop.Core;
using Nop.Services.Common;
using Nop.Services.Plugins;

namespace Nop.Plugin.Techtoniq.Example
{
    public class ExamplePlugin : BasePlugin, IMiscPlugin
    {
        #region Fields

        private readonly IWebHelper _webHelper;

        #endregion Fields

        #region Ctor

        public ExamplePlugin(IWebHelper webHelper)
        {
            _webHelper = webHelper;
        }

        #endregion Ctor

        #region Methods

        public override void Install()
        {
            base.Install();
        }

        public override void Uninstall()
        {
            base.Uninstall();
        }
        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/Example/Configure";
        }

        #endregion Methods
    }
}
