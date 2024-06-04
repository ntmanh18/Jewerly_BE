using Bussiness.Services.AccountService;
using Bussiness.Services.AuthenticateService;
<<<<<<< Updated upstream
=======
using Bussiness.Services.CustomerService;
using Bussiness.Services.GemService;
using Bussiness.Services.ProductService;
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
=======
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IGemService, GemService>();
>>>>>>> Stashed changes

            return services;
        }

    }
}
