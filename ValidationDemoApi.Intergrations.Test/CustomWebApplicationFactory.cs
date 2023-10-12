using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ValidationDemoApi.DAL;

namespace ValidationDemoApi.Intergrations.Test
{
    // Test Factory Setup
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Find the descriptor for the original DbContext
                var dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ContactContext>));

                // Remove the original DbContext registration
                if (dbContextDescriptor != null)
                {
                    services.Remove(dbContextDescriptor);
                }

                // Register the in-memory DbContext
                services.AddDbContext<ContactContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");

                });
                
               
            });

            base.ConfigureWebHost(builder);
        }
    }

}