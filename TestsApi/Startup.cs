using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using TestsApi;

[assembly: OwinStartup(typeof(Startup))]
namespace TestsApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder builder)
        {
            var httpConfiguration = CreateHttpConfiguration();

            builder
                .UseCors(CorsOptions.AllowAll)
                .UseWebApi(httpConfiguration);
        }

        private HttpConfiguration CreateHttpConfiguration()
        {
            var config = new HttpConfiguration();
            
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new {id = RouteParameter.Optional}
            );
            
            config.Formatters.Add(new JsonMediaTypeFormatter());
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Formatting.Indented;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();
            config.Formatters.XmlFormatter.SupportedMediaTypes.Add(
                new MediaTypeHeaderValue("multipart/form-data"));

            return config;
        }
    }
}