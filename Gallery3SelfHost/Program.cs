using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace Gallery3SelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri _baseAddress = new Uri("http://localhost:60065");
            HttpSelfHostConfiguration config = new HttpSelfHostConfiguration(_baseAddress);
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional });

            HttpSelfHostServer server = new HttpSelfHostServer(config);

            server.OpenAsync().Wait();
            Console.WriteLine("Gallery Web-API Self hosted on" + _baseAddress);
            Console.WriteLine("Hit Enter to exit...");
            clsGalleryController clsGalleryController = new clsGalleryController();
            List<string> list = clsGalleryController.GetArtistNames();
            Console.WriteLine(list[1]);
            Console.ReadLine();
            server.CloseAsync().Wait();
        }
    }
}
