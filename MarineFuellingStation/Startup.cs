using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using MFS.Models;
using MFS.Repositorys;
using MFS.Hubs;
using Senparc.Weixin.Work.Containers;
using Microsoft.Extensions.Options;

namespace MFS
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<EFContext>(option =>
            {
                option.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]);
            });
            services.AddSession();
            // Add framework services.
            services.AddMvc();
            services.AddSignalR();

            services.Configure<WorkOption>(Configuration.GetSection("WorkOption"));
            //注入仓储类
            services.AddScoped<SalesPlanRepository>();
            services.AddScoped<ProductRepository>();
            services.AddScoped<ProductTypeRepository>();
            services.AddScoped<OrderRepository>();
            services.AddScoped<StoreRepository>();
            services.AddScoped<StoreTypeRepository>();
            services.AddScoped<PurchaseRepository>();
            services.AddScoped<AssayRepository>();
            services.AddScoped<MoveStoreRepository>();
            services.AddScoped<BoatCleanRepository>();
            services.AddScoped<CompanyRepository>();
            services.AddScoped<ClientRepository>();
            services.AddScoped<SurveyRepository>();
            services.AddScoped<InAndOutLogRepository>();
            services.AddScoped<PaymentRepository>();
            services.AddScoped<UserRepository>();
            services.AddScoped<WageRepository>();
            services.AddScoped<ChargeLogRepository>();
            services.AddScoped<NoticeRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IOptionsSnapshot<WorkOption> option)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseSession();
            app.UseStaticFiles();
            app.UseWebSockets();
            app.UseSignalR(routes =>
            {
                routes.MapHub<PrintHub>("hubs/print");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
            #region 微信相关

            //注册微信
            AccessTokenContainer.Register(option.Value.CorpId, option.Value.Secret);

            #endregion
        }
    }
}
