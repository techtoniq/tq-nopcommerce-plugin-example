using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Nop.Plugin.Techtoniq.Example.IntegrationTests
{
    public class CustomWebApplicationFactory<TStartup>
                : WebApplicationFactory<TStartup> where TStartup : class
    {
        private readonly string _testFolderRoot;
        private readonly string _nopWebRoot;

        public CustomWebApplicationFactory(string testFolderRoot, string nopWebRoot)
        {
            _testFolderRoot = testFolderRoot;
            _nopWebRoot = nopWebRoot;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseContentRoot(_testFolderRoot);
            builder.UseWebRoot(_nopWebRoot);

            SymbolicLink.CreateJunction(@$"{_testFolderRoot}\Themes", @$"{_nopWebRoot}\..\Themes");
            SymbolicLink.CreateJunction(@$"{_testFolderRoot}\Plugins", @$"{_nopWebRoot}\..\Plugins");
        }
    }
}