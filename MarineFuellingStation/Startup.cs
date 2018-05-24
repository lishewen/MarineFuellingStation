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
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Text;
using Senparc.Weixin.Exceptions;

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
            services.AddScoped<ChargeLogRepository>();
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
                //app.UseExceptionHandler("/Home/Error");
                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "application/json";

                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            var ex = error.Error;

                            //当异常为微信类型异常时候尝试重新注册刷新AccessToken
                            if (ex is WeixinException)
                            {
                                try
                                {
                                    RegisterWeixinAccessToken(option);
                                }
                                catch(Exception e)
                                {
                                    ex = e;
                                }
                            }

                            await context.Response.WriteAsync(new ResultJSON<string>()
                            {
                                Code = 500,
                                Msg = ex.Message,
                                Data = ex.Source
                            }.ToString(), Encoding.UTF8);
                        }
                    });
                });
            }

            app.UseSession();
            app.UseStaticFiles();
            app.UseWebSockets();
            app.UseSignalR(routes =>
            {
                routes.MapHub<PrintHub>("/hubs/print");
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

            RegisterWeixinAccessToken(option);

            #endregion
        }

        private static void RegisterWeixinAccessToken(IOptionsSnapshot<WorkOption> option)
        {
            //注册微信
            AccessTokenContainer.Register(option.Value.CorpId, option.Value.Secret);
            AccessTokenContainer.Register(option.Value.CorpId, option.Value.销售单Secret);
            AccessTokenContainer.Register(option.Value.CorpId, option.Value.收银Secret);
            AccessTokenContainer.Register(option.Value.CorpId, option.Value.加油Secret);
        }
    }
}
