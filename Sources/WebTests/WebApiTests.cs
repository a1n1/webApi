using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Web.Http;
using NUnit.Framework;
using Web;
using Web.Controllers;

namespace WebTests
{
    [TestFixture]
    public class WebApiTests
    {
        private const string JsonContentType = "application/json";

        [Test]
        public void ShouldUpdate()
        {
            var httpConfiguration = GetBaseHttpConfiguration();
            AddRouting(httpConfiguration);

            var server = new HttpServer(httpConfiguration);
            var controller = new ValuesController();
            using (var client = new HttpMessageInvoker(server))
            {
                using (var request = new HttpRequestMessage(HttpMethod.Put, "http://localhost/api/values/5"))
                {
                    request.Content = new StringContent("{\"value\":5}", Encoding.UTF8, JsonContentType);
                    using (var response = client.SendAsync(request, CancellationToken.None).Result)
                    {
                        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
                    }
                }
            }
        }

        private static void AddRouting(HttpConfiguration httpConfiguration)
        {
            httpConfiguration.MapHttpAttributeRoutes();
            httpConfiguration.Routes.MapHttpRoute("Default", "api/{controller}/{id}", new {id = RouteParameter.Optional});
        }

        [Test]
        public void ShouldSendResponse()
        {
            var httpConfiguration = GetBaseHttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            var server = new HttpServer(httpConfiguration);

            using (var client = new HttpClient(server))
            {
                using (var request = CreateRequest("api/values/5", JsonContentType, HttpMethod.Put))
                {
                    request.Content = new StringContent(@"{""Count"":35, ""Name"":""MyName""}", Encoding.UTF8,
                        "application/json");
                    using (
                        var response =
                            client.PutAsync(request.RequestUri, request.Content, CancellationToken.None).Result)
                    {
                        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
                    }
                }
            }
        }

        private HttpConfiguration GetBaseHttpConfiguration()
        {
            var httpConfiguration = new HttpConfiguration();
            httpConfiguration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            return httpConfiguration;
        }

        [Test]
        public void GeValues_ReturnsNotFound()
        {
            var controller = new ValuesController();
            var config = GetBaseHttpConfiguration();
            AddRouting(config);
            var server = new HttpServer(config);
            using (var client = new HttpMessageInvoker(server))
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/values/1"))
                using (var response = client.SendAsync(request, CancellationToken.None).Result)
                {
                    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                }
            }
        }

        [Test]
        public void Should_Get_Value()
        {
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            var server = new HttpServer(config);
            using (var client = new HttpMessageInvoker(server))
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/values/1"))
                using (var response = client.SendAsync(request, CancellationToken.None).Result)
                {
                    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                    Assert.That(response.Content.ReadAsStringAsync().Result.Contains("value"), Is.True);
                }
            }
        }

        private HttpRequestMessage CreateRequest(string url, string mthv, HttpMethod method)
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("http://localhost/" + url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mthv));
            request.Method = method;
            return request;
        }
    }
}