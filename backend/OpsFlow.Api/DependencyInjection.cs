
using OpsFlow.Application.Assets.CreateAsset;
using OpsFlow.Application.Assets.GetAllAssets;
using OpsFlow.Application.Assets.GetAssetById;
using OpsFlow.Application.Assets.UpdateAsset;
using OpsFlow.Application.Authentication.Login;
using OpsFlow.Application.Customers.GetAllCustomers;
using OpsFlow.Application.Customers.GetCustomerById;
using OpsFlow.Application.Customers.UpdateCustomer;
using OpsFlow.Application.Interfaces;
using OpsFlow.Application.Sites.CreateSite;
using OpsFlow.Application.Sites.GetAll;
using OpsFlow.Application.Sites.GetById;
using OpsFlow.Application.Sites.UpdateSite;
using OpsFlow.Application.Users.CreateUser;
using OpsFlow.Application.Users.GetUsers;
using OpsFlow.Application.Users.UpdateUser;
using OpsFlow.Infrastructure.Repositories;
using OpsFlow.Infrastructure.Security;
using OpsFlow.Application.Customers.CreateCustomer;

namespace OpsFlow.Api
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddOpsFlowServices(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ISiteRepository, SiteRepository>();
            services.AddScoped<IAssetRepository, AssetRepository>();
            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<LoginUseCase>();
            services.AddScoped<CreateUserUseCase>();
            services.AddScoped<GetAllUsersUseCase>();
            services.AddScoped<UpdateUserUseCase>();
            services.AddScoped<GetAllCustomersUseCase>();
            services.AddScoped<GetCustomerByIdUseCase>();
            services.AddScoped<UpdateCustomerUseCase>();
            services.AddScoped<CreateSiteUseCase>();
            services.AddScoped<GetAllUseCase>();
            services.AddScoped<GetBySiteIdUseCase>();
            services.AddScoped<UpdateSiteUseCase>();
            services.AddScoped<CreateAssetUseCase>();
            services.AddScoped<GetAllAssetsUseCase>();
            services.AddScoped<GetAssetByIdUseCase>();
            services.AddScoped<UpdateAssetUseCase>();
            services.AddScoped<CreateCustomerUseCase>();

            return services;
        }
    }
}
