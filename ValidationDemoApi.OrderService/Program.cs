using MassTransit;

namespace ValidationDemoApi.OrderService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddHttpClient();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            //builder.Services.AddMassTransit(x =>
            //{
            //    // elided...
            //    x.UsingRabbitMq((context, cfg) =>
            //    {
            //        cfg.Host("localhost", "/", h => {
            //            h.Username("guest");
            //            h.Password("guest");
            //        });
            //        cfg.ConfigureEndpoints(context);
            //        cfg.UseRetry(retryConfig =>
            //        {
            //            retryConfig.Interval(3, TimeSpan.FromSeconds(5));
            //        });
            //        cfg.UseCircuitBreaker(cbConfig =>
            //        {
            //            cbConfig.TrackingPeriod = TimeSpan.FromMinutes(1);
            //            cbConfig.TripThreshold = 15;
            //            cbConfig.ActiveThreshold = 10;
            //            cbConfig.ResetInterval = TimeSpan.FromMinutes(5);
            //        });
            //    });
            //});
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}