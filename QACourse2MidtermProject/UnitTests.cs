using FluentAssertions;
using FluentAssertions.Execution;
using System.Net.Http.Json;
using System.Text.Json;
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

                var response = JsonSerializer.Deserialize<ReqresApiResponseModel>(content);

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

                var response = JsonSerializer.Deserialize<ReqresApiSingleResponseModel>(content);

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

                var response = JsonSerializer.Deserialize<ReqresApiResponseModel>(content);

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

                using (new AssertionScope())
                {
                    result.IsSuccessStatusCode.Should().BeTrue();
                    result.ReasonPhrase.Should().Be("No Content");
                }
            }

            [Fact]
            public async Task TestCreateNewUser()
            {
                HttpClient client = new();
                client.BaseAddress = new Uri("https://reqres.in/");
                string urlSuffix = "/api/users";

                ReqResApiCreateUserModel userContent = new();
                userContent.name = "morpheus";
                userContent.job = "leader";

                var contentToSend = new StringContent(JsonSerializer.Serialize<ReqResApiCreateUserModel>(userContent));
                var result = await client.PostAsync(urlSuffix, contentToSend);
                var content = await result.Content.ReadAsStringAsync();

                var response = JsonSerializer.Deserialize<ReqresApiCreateResponseModel>(content);

                using (new AssertionScope())
                {
                    result.IsSuccessStatusCode.Should().BeTrue();
                    response.id.Should().NotBeNullOrEmpty();
                    response.createdAt.ToString("MM/dd/yyyy").Should().Be(DateTime.Now.ToUniversalTime().ToString("MM/dd/yyyy"));
                    //response does not return name or job, so cannot assert on those
                }
            }
            
            [Fact]
            public async Task TestUpdateExistingUser()
            {
                HttpClient client = new();
                client.BaseAddress = new Uri("https://reqres.in/");
                string urlSuffix = "/api/users/2";

                ReqResApiCreateUserModel userContent = new();
                userContent.name = "morpheus";
                userContent.job = "zion resident";

                var contentToSend = new StringContent(JsonSerializer.Serialize<ReqResApiCreateUserModel>(userContent));
                var result = await client.PatchAsync(urlSuffix, contentToSend);
                var content = await result.Content.ReadAsStringAsync();

                var response = JsonSerializer.Deserialize<ReqresApiPatchResponseModel>(content);

                using (new AssertionScope())
                {
                    result.IsSuccessStatusCode.Should().BeTrue();
                    response.updatedAt.ToString("MM/dd/yyyy").Should().Be(DateTime.Now.ToUniversalTime().ToString("MM/dd/yyyy"));
                }
            }

            [Fact]
            public async Task TestRegisterUser()
            {
                HttpClient client = new();
                client.BaseAddress = new Uri("https://reqres.in/");
                string urlSuffix = "/api/register";

                ReqResApiRegisterModel userContent = new();
                userContent.email = "eve.holt@reqres.in";
                userContent.password = "pistol";

                var result = await client.PostAsJsonAsync(urlSuffix, userContent);
                var content = await result.Content.ReadAsStringAsync();

                var response = JsonSerializer.Deserialize<ReqResApiRegisterResponseModel>(content);

                using (new AssertionScope())
                {
                    result.IsSuccessStatusCode.Should().BeTrue();
                    response.token.Should().Be("QpwL5tke4Pnpja7X4");
                }
            }

        }
    }
}
