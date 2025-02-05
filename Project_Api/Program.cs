
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Project_Api.Data;
using Project_Api.ErrorHandling;
using Project_Api.Interfaces;
using Project_Api.Mappers;
using Project_Api.Models;
using Project_Api.Repositries;
using Project_Api.ServiceRegistration;
using Project_Api.Services;
using System.Text;

namespace Project_Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add DbContext
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                 options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddDependencyInjection(builder.Configuration);

            // Add AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile));

           
            // Add Authentication
          //  builder.Services.AddAuthenticationConfigure(builder.Configuration);

            // Add CORS
            builder.Services.AddCors();

            // Add Logging
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors(x => x.AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowed(o => true).AllowCredentials());

            app.UseAuthentication();
            app.UseAuthorization();

            // Use Custom exception middleware
            app.UseMiddleware<ExceptionMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }

   
}
