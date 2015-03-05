using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Web.Http;
using NUnit.Framework;
using WebApiTesting;
using WebApiTesting.Controllers;

namespace WebTests
{
    [TestFixture]
    public class WebApiTests
    {
        [Test]
        public void ShouldUpdateQual()
        {
            var httpConfiguration = new HttpConfiguration();
            httpConfiguration.MapHttpAttributeRoutes();
            httpConfiguration.Routes.MapHttpRoute("Default", "api/{controller}/{id}", new {id = RouteParameter.Optional});
            httpConfiguration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            //WebApiConfig.Register(httpConfiguration);
            var server = new HttpServer(httpConfiguration);
            var controller = new ValuesController();
            using (var client = new HttpMessageInvoker(server))
            {
                using (var request = new HttpRequestMessage(HttpMethod.Put, "http://localhost/api/values/5"))
                {
                    request.Content = new StringContent("{\"value\":5}", Encoding.UTF8, "application/json");
                    using (var response = client.SendAsync(request, CancellationToken.None).Result)
                    {
                        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
                    }
                }
            }
        }

        [Test]
        public void ShouldSendResponse()
        {
            var httpConfiguration = new HttpConfiguration();
            httpConfiguration.MapHttpAttributeRoutes();
            httpConfiguration.Routes.MapHttpRoute("Default", "api/{controller}/{id}", new {id = RouteParameter.Optional});

            httpConfiguration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            //WebApiConfig.Register(httpConfiguration);
            var server = new HttpServer(httpConfiguration);

            using (var client = new HttpClient(server))
            {
                using (var request = CreateRequest("api/values/5", "application/json", HttpMethod.Put))
                {
                    //request.Content = new StringContent(@"value:5", Encoding.UTF8, "application/json");
                    using (
                        var response =
                            client.PutAsync(request.RequestUri, request.Content, CancellationToken.None).Result)
                    {
                        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
                    }
                }
            }
        }

        [Test]
        public void GeValues_ReturnsNotFound()
        {
            var controller = new ValuesController();
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute("Default", "api/{controller}/{id}");
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