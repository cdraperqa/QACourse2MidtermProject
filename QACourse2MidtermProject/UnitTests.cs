using FluentAssertions;
using FluentAssertions.Execution;
//using System.Text.Json;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using System.Xml.XPath;
using Xunit.Abstractions;
//using static QACourse2MidtermProject.RestfulApiResponse;

namespace QACourse2MidtermProject
{
    public class UnitTests
    {
        private ITestOutputHelper _logger;

        public UnitTests(ITestOutputHelper logger)
        {
            _logger = logger;
        }

        [Fact]
        public async Task TestGetSingleObject()
        {
            //string url = "https://api.restful-api.dev/objects";
            HttpClient client = new();
            client.BaseAddress = new Uri("https://api.restful-api.dev/");
            string urlSuffix = "/objects/5";

            var result = await client.GetAsync(urlSuffix);
            var content = await result.Content.ReadAsStringAsync();
            content = content.Replace("[", "").Replace("]", "");
            _logger.WriteLine(content);

            var response = System.Text.Json.JsonSerializer.Deserialize<RestfulApiResponse>(content);

            using (new AssertionScope())
            {
                result.IsSuccessStatusCode.Should().BeTrue();
                response.id.Should().Be("5");

            }
        }

        [Fact]
        public async Task TestPostNewItem()
        {
            HttpClient client = new();
            client.BaseAddress = new Uri("https://api.restful-api.dev/objects");
            string urlSuffix = "/objects";
            var contentSent = new StringContent("{\r\n   \"name\": \"markers\",\r\n   \"data\": {\r\n      \"description\": \"Crayola Washable Markers\",\r\n      \"color\": \"multicolor\",\r\n      \"itemcount:\": \"8\",\r\n      \"quantity\": \"1\"\r\n   }\r\n}", null, "application/json");

            var result = await client.PostAsync(urlSuffix, contentSent);
            var content = await result.Content.ReadAsStringAsync();
            //content = content.Replace("[", "").Replace("]", "");
            //var contentSent = new RestfulApiResponse
            //{
            //    name = "markers",
            //    data = new Data
            //    {
            //        Description = "Crayola Washable Markers",
            //        color = "multicolor",
            //        itemcount = 8,
            //        quantity = 1
            //    }
            //};

            var response = System.Text.Json.JsonSerializer.Deserialize<RestfulApiPostResponse>(content);


            using (new AssertionScope())
            {
                result.IsSuccessStatusCode.Should().BeTrue();
                //response<name>.Should().Be("markers");

            }

        }

        [Fact]
        public async Task TestPutExistingItem()
        {
            HttpClient client = new();
            client.BaseAddress = new Uri("https://api.restful-api.dev/objects");
            string urlSuffix = "/objects/{newId}";
            var contentSent = new StringContent("{\r\n   \"name\": \"folder\",\r\n   \"data\": {\r\n      \"description\": \"two-pocket folder with prongs\",\r\n      \"color\": \"rainbow\",\r\n      \"itemcount:\": \"1\",\r\n      \"quantity\": \"1\"\r\n   }\r\n}", null, "application/json");

            var result = await client.PutAsync(urlSuffix, contentSent);
            var content = await result.Content.ReadAsStringAsync();

            var response = System.Text.Json.JsonSerializer.Deserialize<RestfulApiPostResponse>(content);


            using (new AssertionScope())
            {
                result.IsSuccessStatusCode.Should().BeTrue();
                //response<name>.Should().Be("markers");

            }

        }

    }
}