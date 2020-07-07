using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;

namespace OYMLCN.AspNetCore.Test
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.Configure<TencentCloudOptions>(Configuration.GetSection("TencentCloud"));
            //services.AddScoped<SmsSender>();
            //services.AddTencentCloud();

            services.AddTransferJob();
            //services.AddJwtAuthentication();

            services
                .AddSingleton<DemoTest.IDAL.IPersonDal, DemoTest.DAL.PersonDal>()
                //.AddSingleton<DemoTest.IService.IPersonService, DemoTest.Service.PersonService>()
                .AddRpcServer(options => options
                    //.SetRpcUrl(null)
                    //.AddNameSpace("Test.IService")
                    .AddInterface<DemoTest.IService.IPersonService>()
                    .SetDefaultInterface<DemoTest.IService.IPersonService>());

            //services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseCalculateExecutionTime();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseRouting();

            //// 用于调试部分异常以显示敏感信息，正式项目不要出现此句
            //IdentityModelEventSource.ShowPII = true;

            //app.UseAuthentication();
            //app.UseAuthorization();

            app.UseRpcMiddleware();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapDefaultControllerRoute();
            //});
        }
    }
}
