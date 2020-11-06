using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;

namespace Nop.Plugin.Techtoniq.Example.IntegrationTests
{
    public class IntegrationTestBase
    {
        public static string TestFolderRoot => System.IO.Path.GetFullPath($@"{TestContext.CurrentContext.TestDirectory}\..\..\..");
        public static string NopWebRoot => System.IO.Path.GetFullPath($@"{TestContext.CurrentContext.TestDirectory}\..\..\..\..\..\..\nopCommerce_4.30\Presentation\Nop.Web\wwwroot");

        private static TestServer _testServer;
        private static readonly object _padlock = new object();

        public static TestServer TestServer
        {
            get
            {
                if (null == _testServer)
                {
                    lock (_padlock)
                    {
                        if (null == _testServer)
                        {
                            var factory = new CustomWebApplicationFactory<Nop.Web.Startup>(TestFolderRoot, NopWebRoot);
                            _testServer = factory.Server;
                        }
                    }
                }
                return _testServer;
            }
        }
    }
}
