using Autofac;
using AutoMapper;
using Firewall;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using TransactionStore.API;
using TransactionStore.API.Configuration;
using TransactionStore.API.MassTransit;
using TransactionStore.API.Secure;
using TransactionStore.Core.ConfigurationOptions;

namespace TransactionStore
{
    public class Startup
    {
        //public ConfigurationBuilder ConfigurationBuilder { get; } = new ConfigurationBuilder();
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder();

            builder.AddJsonFile("config.json", false, true);
            if (env.IsDevelopment())
                builder.AddJsonFile("config.development.json", false, true);

            builder.AddJsonFile("IPWhiteList.json", false, true);

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModule());
            builder.RegisterModule(new AutofacMassTransitModule());
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<IPWhiteListConfiguration>(Configuration);
            services.Configure<StorageOptions>(Configuration);
            services.Configure<UrlOptions>(Configuration);

            services.AddControllers();

            services.AddSwaggerGen(c =>
               c.SwaggerDoc(name: "v1", new OpenApiInfo { Title = "TransactionStore.API", Version = "v1" })
            );

            services.AddMassTransit(x =>
            {
                x.AddConsumer<RatesConsumer>();
            });

            services.AddMassTransitHostedService();

            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutomapperProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();

            app.UseSwaggerUI(c => c.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "TransactionStore.API V1"));

            app.UseFirewall(
                FirewallRulesEngine
                    .DenyAllAccess()
                    .ExceptFromLocalhost()
                    .ExceptFromIPAddresses(AllowedIPs.authorizedIPs));

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
