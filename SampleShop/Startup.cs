using SampleShop;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;


using SampleShop.Interfaces;
using SampleShop.Services;
using SampleShop.Utilities;
using SampleShop.Queries;
using System.Reflection.Metadata.Ecma335;

[assembly: WebJobsStartup(typeof(Startup))]
namespace SampleShop
{
    public class Startup : IWebJobsStartup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore();
            services.AddSingleton<ILogger, Logger>();

            services.AddScoped<IAuditService, AuditService>();
            services.AddScoped<IItemsService, ItemsService>();
            services.AddScoped<IOrdersService, OrdersService>();
            services.AddScoped<ITransactionService, TransactionService>();

            services.AddSingleton(db => DatabaseFactory.CreateDatabase());

            services.AddScoped<GetAllItemsQuery, GetAllItemsQuery>();
            services.AddScoped<GetAllOrdersQuery,GetAllOrdersQuery>();
            services.AddScoped<GetOrderByIdQuery, GetOrderByIdQuery>();

        }

        public void Configure(IWebJobsBuilder builder)
        {
            ConfigureServices(builder.Services);
        }

    }
}
