using FluentAssertions;
using FluentAssertions.Execution;
using System.Text.Json;
//using Newtonsoft.Json;
using System.Collections.Immutable;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using System.Xml.XPath;
using Xunit.Abstractions;

namespace QACourse2MidtermProject
{
    namespace QACourse2MidtermProject
    {
        public class UnitTests
        {
            private int _userId = 2;

            private ITestOutputHelper _logger;

            public UnitTests(ITestOutputHelper logger)
            {
                _logger = logger;
            }

            [Fact]
            public async Task TestGetListOfUsers()
            {
                HttpClient client = new();
                client.BaseAddress = new Uri("https://reqres.in/");
                string urlSuffix = "api/users?page=2";

                var result = await client.GetAsync(urlSuffix);
                var content = await result.Content.ReadAsStringAsync();
                _logger.WriteLine(content);

                var response = System.Text.Json.JsonSerializer.Deserialize<ReqresApiResponseModel>(content);

                using (new AssertionScope())
                {
                    result.IsSuccessStatusCode.Should().BeTrue();
                    response.total.Should().Be(12);

                }
            }

            [Fact]
            public async Task TestGetSpecificUser()
            {
                HttpClient client = new();
                client.BaseAddress = new Uri("https://reqres.in/");
                string urlSuffix = $"api/users/{_userId}";

                var result = await client.GetAsync(urlSuffix);
                var content = await result.Content.ReadAsStringAsync();
                _logger.WriteLine(content);

                result.IsSuccessStatusCode.Should().BeTrue();

            }

            [Fact]
            public async Task TestUserNotFound()
            {
                HttpClient client = new();
                client.BaseAddress = new Uri("https://reqres.in/");
                string urlSuffix = "api/users/23";

                var result = await client.GetAsync(urlSuffix);
                var content = await result.Content.ReadAsStringAsync();
                _logger.WriteLine(content);

                try
                {
                    var response = System.Text.Json.JsonSerializer.Deserialize<ReqresApiResponseModel>(content);
                }
                catch (JsonException e)
                {
                    _logger.WriteLine("Unable to parse response content. Response body was {content}, status code was {result.StatusCode}.");
                    _logger.WriteLine(e.Message);
                    _logger.WriteLine(e.StackTrace);
                    throw;
                }

                result.ReasonPhrase.Should().Be("Not Found");
            }

            [Fact]
            public async Task TestDeleteUser()
            {
                HttpClient client = new();
                client.BaseAddress = new Uri("https://reqres.in/");
                string urlSuffix = "/api/users/2";

                var result = await client.DeleteAsync(urlSuffix);
                var content = await result.Content.ReadAsStringAsync();
                _logger.WriteLine(content);

                result.ReasonPhrase.Should().Be("No Content");
            }

            [Fact]
            public async Task TestCreateNewUser()
            {
                HttpClient client = new();
                client.BaseAddress = new Uri("https://reqres.in/");
                string urlSuffix = "/api/users";
                var content = new StringContent("{\r\n    \"name\": \"morpheus\",\r\n    \"job\": \"leader\"\r\n}");
                var response = await client.PostAsync(urlSuffix, content);

                var result = await client.PostAsJsonAsync(urlSuffix, content);

                using (new AssertionScope())
                {
                    result.IsSuccessStatusCode.Should().BeTrue();
                    response.ReasonPhrase.Should().Be("Created");

                }
            }


        }
    }
}
