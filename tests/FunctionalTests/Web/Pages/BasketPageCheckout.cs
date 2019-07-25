﻿using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.eShopWeb.FunctionalTests.Web.Controllers;
using Microsoft.eShopWeb.Web;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
//using Xunit;
using NUnit.Framework;

namespace Microsoft.eShopWeb.FunctionalTests.WebRazorPages
{
    [TestFixture]
    public class BasketPageCheckout //: IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public BasketPageCheckout(CustomWebApplicationFactory<Startup> factory)
        {
            Client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = true
            });
        }

        public HttpClient Client { get; }

        private string GetRequestVerificationToken(string input)
        {
            string regexpression = @"name=""__RequestVerificationToken"" type=""hidden"" value=""([-A-Za-z0-9+=/\\_]+?)""";
            var regex = new Regex(regexpression);
            var match = regex.Match(input);
            return match.Groups.LastOrDefault().Value;
        }

        [Test]
        public async Task RedirectsToLoginIfNotAuthenticated()
        {
            // Arrange & Act

            // Load Home Page
            var response = await Client.GetAsync("/");
            response.EnsureSuccessStatusCode();
            var stringResponse1 = await response.Content.ReadAsStringAsync();

            string token = GetRequestVerificationToken(stringResponse1);

            // Add Item to Cart
            var keyValues = new List<KeyValuePair<string, string>>();
            keyValues.Add(new KeyValuePair<string, string>("id", "2"));
            keyValues.Add(new KeyValuePair<string, string>("name", "shirt"));

            keyValues.Add(new KeyValuePair<string, string>("price", "19.49"));
            keyValues.Add(new KeyValuePair<string, string>("__RequestVerificationToken", token));

            var formContent = new FormUrlEncodedContent(keyValues);

            var postResponse = await Client.PostAsync("/basket/index", formContent);
            postResponse.EnsureSuccessStatusCode();
            var stringResponse = await postResponse.Content.ReadAsStringAsync();

            // Assert
            Assert.That(stringResponse.Contains(".NET Black &amp; White Mug"));

            keyValues.Clear();
            keyValues.Add(new KeyValuePair<string, string>("__RequestVerificationToken", token));

            formContent = new FormUrlEncodedContent(keyValues);
            var postResponse2 = await Client.PostAsync("/Basket/Checkout", formContent);
            Assert.That(postResponse2.RequestMessage.RequestUri.ToString().Contains("/Identity/Account/Login"));
        }
    }
}
