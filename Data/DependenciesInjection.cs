


using Data.Repository.BillRepo;
using Data.Repository.CashierRepo;
using Data.Repository.CustomerRepo;
using Data.Repository.DiscountRepo;
using Data.Repository.GemRepo;
using Data.Repository.GoldRepo;
using Data.Repository.ProductBillRepo;
using Data.Repository.OldProductRepo;
using Data.Repository.ProductGemRepo;
using Data.Repository.ProductRepo;
using Data.Repository.UserRepo;


using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;

namespace Data
{
    public static class DependenciesInjection
    {
        public static IServiceCollection AddRepository(this IServiceCollection services, IConfiguration configuration)
        {
            var assembly = typeof(DependenciesInjection).Assembly;
            services.AddScoped<IUserRepo, UserRepo>();

            services.AddScoped<ICustomerRepo, CustomerRepo>();
            services.AddScoped<IProductRepo, ProductRepo>();
            services.AddScoped<IGemRepo, GemRepo>();
            services.AddScoped<IGoldRepo,GoldRepo>();
            services.AddScoped<IProductGemRepo,ProductGemRepo>();
            services.AddScoped<ICashierRepo, CashierRepo>();
            services.AddScoped<IDiscountRepo, DiscountRepo>();
            services.AddScoped<IProductBillRepo,ProductBillRepo>();
            services.AddScoped <IBillRepo,BillRepo>();

            services.AddScoped<IOldProductRepo, OldProductRepo>();
            return services;
        }

    }
}
