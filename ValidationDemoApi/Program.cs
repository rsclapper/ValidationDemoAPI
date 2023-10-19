// using GreenPipes;
// using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MassTransit;
using ValidationDemoApi.CORE.Interfaces;
using ValidationDemoApi.CORE.Models;
using ValidationDemoApi.DAL;
using ValidationDemoApi.Helpers;

namespace ValidationDemoApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            IMapper<Contact> mapper = new ContactMapper();
            // Add services to the container.

            builder.Services.AddDbContext<ContactContext>(options => 
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );
// Add CORS support
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .WithOrigins("http://172.22.0.1:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });
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
          
            //builder.Services.AddSingleton(typeof(IRepository<Contact>), x => new FileRepository<Contact>("Contacts.txt", mapper));
            builder.Services.AddTransient<IRepository<Contact>, EFRepository<Contact>>();
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
         .AddJwtBearer(options =>
         {
             options.RequireHttpsMetadata = false;
             options.TokenValidationParameters = new TokenValidationParameters
             {
                 ValidateIssuer = true,
                 ValidateAudience = true,
                 ValidateLifetime = true,
                 ValidateIssuerSigningKey = true,

                 ValidIssuer = "https://localhost:7124",
                 ValidAudience = "https://localhost:7124",
                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("KeyForSignInSecret@1234"))
             };
             // builder.Services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
         });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                
            }
          
            using(var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ContactContext>();
                context.Database.EnsureCreated();
                
            }
            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}