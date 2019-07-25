using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.eShopWeb.Web;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Microsoft.eShopWeb.FunctionalTests.Web.Controllers
{
    [TestFixture]
    public class OrderIndexOnGet //: IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public OrderIndexOnGet(CustomWebApplicationFactory<Startup> factory)
        {
            Client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        public HttpClient Client { get; }

        [Test]
        public async Task ReturnsRedirectGivenAnonymousUser()
        {
            var response = await Client.GetAsync("/order/my-orders");
            var redirectLocation = response.Headers.Location.OriginalString;

            Assert.AreEqual(HttpStatusCode.Redirect, response.StatusCode);
            //Assert.Contains("/Account/Login", redirectLocation);
        }
    }
}
