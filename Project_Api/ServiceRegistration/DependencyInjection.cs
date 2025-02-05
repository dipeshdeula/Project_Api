using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Project_Api.Interfaces;
using Project_Api.Interfaces.AdminInterface;
using Project_Api.Repositries;
using Project_Api.Repositries.AdminRepository;
using Project_Api.Services;
using Project_Api.Services.AdminService;
using Project_Api.Utilities;
using System.Text;

namespace Project_Api.ServiceRegistration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            // Add Repositories and Services
            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IProductService, ProductService>();
           services.AddScoped<IProductRepository, ProductRepo>();
           services.AddScoped<IOrderRepository, OrderRepository>();
           services.AddScoped<IOrderService, OrderService>();
           services.AddScoped<ICustomerRepository, CustomerRepository>();
           services.AddScoped<ICustomerService, CustomerService>();
           services.AddScoped<ICartRepository, CartRepository>();
           services.AddScoped<ICartService, CartService>();
           services.AddScoped<IPaymentRepository, PaymentRepository>();
           services.AddScoped<IPaymentService, PaymentService>();

            services.AddScoped<JwtTokenHelper>();

            //configure JWT authentication middleware
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
            });


            return services;

        }
    }
}
