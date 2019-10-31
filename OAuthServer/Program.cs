using System;
using Microsoft.Owin.Hosting;
using Serilog;

namespace OAuthServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo
                .LiterateConsole(outputTemplate: "{Timestamp:HH:mm} [{Level}] ({Name:l}){NewLine} {Message}{NewLine}{Exception}")
                .CreateLogger();

            using (WebApp.Start<Startup>("http://localhost:5021"))
            {
                Console.WriteLine("server running...");
                Console.ReadLine();
            }
        }
    }
}
