using Microsoft.eShopWeb.Web;
using System.Net.Http;
using System.Threading.Tasks;
//using Xunit;
using NUnit.Framework;

namespace Microsoft.eShopWeb.FunctionalTests.Web.Controllers
{
    [TestFixture]
    public class CatalogControllerIndex //: IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public CatalogControllerIndex(CustomWebApplicationFactory<Startup> factory)
        {
            Client = factory.CreateClient();
        }

        public HttpClient Client { get; }

        [Test]
        public async Task ReturnsHomePageWithProductListing()
        {
            // Arrange & Act
            var response = await Client.GetAsync("/");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.That(stringResponse.Contains(".NET Bot Black Sweatshirt"));
        }
    }
}
