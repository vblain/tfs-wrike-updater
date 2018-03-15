using System.Web.Http;

namespace Rocklan.WrikeUpdater
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            // disable the XML formatter for WebAPI, as we only support JSON
            config.Formatters.Remove(config.Formatters.XmlFormatter);
        }
    }
}