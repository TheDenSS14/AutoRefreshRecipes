using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace DEN.AutoRefreshRecipes;

public static class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, builder)=>
            {
                var env = context.HostingEnvironment;
                builder.AddYamlFile("appsettings.yml", false);
                builder.AddYamlFile($"appsettings.{env.EnvironmentName}.yml", true);
                builder.AddYamlFile("appsettings.Secret.yml", true);
            })
            .UseSerilog((ctx, cfg) =>
                cfg.ReadFrom.Configuration(ctx.Configuration))
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
}