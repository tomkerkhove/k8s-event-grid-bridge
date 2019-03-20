using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Kubernetes.EventBridge.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .ConfigureLogging(loggingBuilder => loggingBuilder.AddConsole())
                .ConfigureAppConfiguration(configurationBuilder=>configurationBuilder.AddEnvironmentVariables("K8S_BRIDGE_"))
                .UseStartup<Startup>();

    }
}