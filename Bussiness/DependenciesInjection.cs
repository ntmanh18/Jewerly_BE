using Bussiness.Services.AccountService;
using Bussiness.Services.AuthenticateService;
using Microsoft.Extensions.DependencyInjection;

namespace Bussiness
{
    public static class DependenciesInjection
    {
        public static IServiceCollection AddBusiness(this IServiceCollection services)
        {
            var assembly = typeof(DependenciesInjection).Assembly;
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAuthenticateService, AuthenticateService>();


            return services;
        }

    }
}
