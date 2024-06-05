using Bussiness.Services.AccountService;
using Bussiness.Services.AuthenticateService;
using Bussiness.Services.CustomerService;
using Bussiness.Services.ProductService;
using Bussiness.Services.TokenService;
using Bussiness.Services.UserService;
using Bussiness.Services.Validate;
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
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserValidate, UserValidate>();
            services.AddScoped<IToken,TokenService>();
            services.AddScoped<IToken, TokenService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IProductService, ProductService>();

            return services;
        }

    }
}
