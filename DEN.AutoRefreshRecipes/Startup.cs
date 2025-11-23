using DEN.AutoRefreshRecipes.Configuration;
using DEN.AutoRefreshRecipes.Managers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace DEN.AutoRefreshRecipes;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    
    private IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<CookbookOptions>(Configuration.GetSection(CookbookOptions.Position));
        services.AddSingleton<CookbookManager>();
        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSerilogRequestLogging();
        
        var pathBase = Configuration.GetValue<string>("PathBase");
        
        if (!string.IsNullOrEmpty(pathBase))
            app.UsePathBase(pathBase);
        
        app.UseAuthentication();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(e => e.MapControllers());
    }
}