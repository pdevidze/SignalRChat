using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SignalRChat
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddSignalR();

      services.AddHostedService<MyBackgroundService>();

      services.AddSingleton<IWorker, MyWorker>();

      services.AddCors(options =>
      {
        options.AddDefaultPolicy(builder =>
        {
          builder
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        });
      });
    }

    public void Configure(IApplicationBuilder app)
    {
      app.UseCors();

      app.UseRouting();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapHub<ChatHub>("/chathub");
      });
    }
  }
}
