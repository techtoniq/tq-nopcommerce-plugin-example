using FluentAssertions;
using Nop.Services.Stores;
using NUnit.Framework;

namespace Nop.Plugin.Techtoniq.Example.IntegrationTests
{
    [TestFixture]
    public class StoreServiceIntegrationTests : IntegrationTestBase
    {
        [TestFixture]
        public class GetAllStoresMethodTests
        {
            [Test]
            public void When_DatabaseSeeded_Then_ReturnDefaultStore()
            {
                // Arrange.

                IStoreService storeService = (IStoreService)TestServer.Services.GetService(typeof(IStoreService));

                // Act.

                var stores = storeService.GetAllStores();

                // Assert.

                stores.Should().HaveCount(1);
                stores[0].Name.Should().Be("Your store name");
                stores[0].Hosts.Should().Be("yourstore.com,www.yourstore.com");
            }
        }
    }
}
