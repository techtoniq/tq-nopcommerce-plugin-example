using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Nop.Plugin.Techtoniq.Example.IntegrationTests
{
    [TestFixture]
    public class TemplateControllerIntegrationTests : IntegrationTestBase
    {
        [TestFixture]
        public class ConfigureMethodTests
        {
            [Test]
            public async Task When_UserNotAuthorised_Then_RedirectToLogin()
            {
                // Arrange.

                var client = TestServer.CreateClient();

                // Act.
                var response = await client.GetAsync("/Admin/Example/Configure");

                // Assert.

                response.StatusCode.Should().Be(302);
                response.Headers.Location.PathAndQuery.Should().Be("/login?ReturnUrl=%2FAdmin%2FExample%2FConfigure");
            }
        }
    }
}