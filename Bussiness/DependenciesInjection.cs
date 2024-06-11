using Bussiness.Services.AccountService;
using Bussiness.Services.AuthenticateService;
using Bussiness.Services.CashierService;
using Bussiness.Services.CustomerService;
using Bussiness.Services.DiscountService;
using Bussiness.Services.GemService;
using Bussiness.Services.ProductBillService;
using Bussiness.Services.ProductGemService;
using Bussiness.Services.ProductService;


using Bussiness.Services.TokenService;
using Bussiness.Services.UserService;
using Bussiness.Services.Validate;
using Bussiness.Services.VoucherService;
using Data.Repository.DiscountRepo;
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
         
            
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IGemService, GemService>();
            services.AddScoped<IVoucherService, VoucherService>();
            services.AddScoped<IProductGemService,ProductGemService>();
            services.AddScoped<ICashierService, CashierService>();
            services.AddScoped<IDiscountService,DiscountService>();
            services.AddScoped<IProductBillService, ProductBillService>();  


            return services;
        }

    }
}
