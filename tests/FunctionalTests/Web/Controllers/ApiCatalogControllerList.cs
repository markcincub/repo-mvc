using Microsoft.eShopWeb.Web;
using Microsoft.eShopWeb.Web.ViewModels;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Microsoft.eShopWeb.FunctionalTests.Web.Controllers
{
    [TestFixture]
    public class ApiCatalogControllerList //: IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public ApiCatalogControllerList(CustomWebApplicationFactory<Startup> factory)
        {
            Client = factory.CreateClient();
        }

        public HttpClient Client { get; }

        [Test]
        public async Task ReturnsFirst10CatalogItems()
        {
            var response = await Client.GetAsync("/api/catalog/list");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<CatalogIndexViewModel>(stringResponse);

            Assert.AreEqual(10, model.CatalogItems.Count());
        }

        [Test]
        public async Task ReturnsLast2CatalogItemsGivenPageIndex1()
        {
            var response = await Client.GetAsync("/api/catalog/list?page=1");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<CatalogIndexViewModel>(stringResponse);

            Assert.AreEqual(2, model.CatalogItems.Count());
        }
    }
}
