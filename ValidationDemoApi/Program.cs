using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
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

            //builder.Services.AddSingleton(typeof(IRepository<Contact>), x => new FileRepository<Contact>("Contacts.txt", mapper));
            builder.Services.AddTransient<IRepository<Contact>, EFRepository<Contact>>();
            builder.Services.AddTransient<IRepository<Order>, EFRepository<Order>>();
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
             options.TokenValidationParameters = new TokenValidationParameters
             {
                 ValidateIssuer = true,
                 ValidateAudience = true,
                 ValidateLifetime = true,
                 ValidateIssuerSigningKey = true,

                 ValidIssuer = "http://localhost:2000",
                 ValidAudience = "http://localhost:2000",
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
          
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}