using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Values;

namespace ValidationDemoApi.Gateway;

internal class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();


        builder.Configuration.AddJsonFile("ocelot.json",optional:false,reloadOnChange:true);
        builder.Services.AddOcelot(builder.Configuration);
        var app = builder.Build();


        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        await app.UseOcelot();

        app.Run();
    }
}