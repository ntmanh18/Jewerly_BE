<<<<<<< Updated upstream
﻿using Data.Repository.UserRepo;
=======
﻿using Data.Repository.CustomerRepo;
using Data.Repository.GemRepo;
using Data.Repository.ProductRepo;
using Data.Repository.UserRepo;
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
            
=======
            services.AddScoped<ICustomerRepo, CustomerRepo>();
            services.AddScoped<IProductRepo, ProductRepo>();
            services.AddScoped<IGemRepo, GemRepo>();
>>>>>>> Stashed changes
            return services;
        }

    }
}
