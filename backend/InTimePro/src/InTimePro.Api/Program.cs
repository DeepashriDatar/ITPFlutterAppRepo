using System;
using System.Configuration;
using Microsoft.Owin.Hosting;

namespace InTimePro.Api
{
    internal static class Program
    {
        private static void Main()
        {
            var baseUrl = ConfigurationManager.AppSettings["BaseUrl"] ?? "http://localhost:5055";

            using (WebApp.Start<Startup>(baseUrl))
            {
                Console.WriteLine("InTimePro API running at: " + baseUrl);
                Console.WriteLine("Swagger UI: " + baseUrl + "/swagger");
                Console.WriteLine("Press Enter to stop...");
                Console.ReadLine();
            }
        }
    }
}
