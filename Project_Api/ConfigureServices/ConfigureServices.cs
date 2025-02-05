namespace Project_Api.ConfigureServices
{
    public static class ConfigureServices
    {
        public static void AddWebUIServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
           // services.AddScoped<ICurrentUserService, CurrentUserService>();
        }

    }
}
