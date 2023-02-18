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

                var response = System.Text.Json.JsonSerializer.Deserialize<ReqresApiSingleResponseModel>(content);

                using (new AssertionScope())
                {
                    result.IsSuccessStatusCode.Should().BeTrue();
                    response.data.id.Should().Be(2);
                    response.data.email.Should().Be("janet.weaver@reqres.in");
                }
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

                var response = System.Text.Json.JsonSerializer.Deserialize<ReqresApiResponseModel>(content);

                using (new AssertionScope())
                {
                    result.ReasonPhrase.Should().Be("Not Found");
                    response.data.Should().BeNull();
                }
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
                var contentToSend = new StringContent("{\r\n    \"name\": \"morpheus\",\r\n    \"job\": \"leader\"\r\n}");
                var result = await client.PostAsync(urlSuffix, contentToSend);
                var content = await result.Content.ReadAsStringAsync();

                var response = System.Text.Json.JsonSerializer.Deserialize<ReqresApiCreateResponseModel>(content);

                using (new AssertionScope())
                {
                    result.IsSuccessStatusCode.Should().BeTrue();
                    response.id.Should().NotBeNullOrEmpty();
                    response.createdAt.Month.Should().Be(DateTime.Now.Month);
                    response.createdAt.Day.Should().Be(DateTime.Now.Day);
                    response.createdAt.Year.Should().Be(DateTime.Now.Year);
                }
            }
            
            [Fact]
            public async Task TestUpdateExistingUser()
            {
                HttpClient client = new();
                client.BaseAddress = new Uri("https://reqres.in/");
                string urlSuffix = "/api/users/2";
                var contentToSend = new StringContent("{\r\n    \"name\": \"morpheus\",\r\n    \"job\": \"zion resident\"\r\n}");
                var result = await client.PatchAsync(urlSuffix, contentToSend);
                var content = await result.Content.ReadAsStringAsync();

                var response = System.Text.Json.JsonSerializer.Deserialize<ReqresApiPatchResponseModel>(content);

                using (new AssertionScope())
                {
                    result.IsSuccessStatusCode.Should().BeTrue();
                    response.updatedAt.Month.Should().Be(DateTime.Now.Month);
                    response.updatedAt.Day.Should().Be(DateTime.Now.Day);
                    response.updatedAt.Year.Should().Be(DateTime.Now.Year);

                }
            }

        }
    }
}
