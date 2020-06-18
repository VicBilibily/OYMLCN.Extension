using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using OYMLCN.Extensions;
using OYMLCN.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OYMLCN.AspNetCore.Test.Controllers
{

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            this.TransferJob(() => Console.WriteLine("Hello World Job0"));
            this.TransferJob<IWebHostEnvironment>(env => TaskJob(env));
            this.TransferJob<IWebHostEnvironment>(env => VoidJob(env));
            return Content("Hello World!");
        }
        private Task TaskJob(IWebHostEnvironment logger)
        {
            for (var i = 1; i <= 10; i++)
            {
                Console.WriteLine("Hello World Job" + i);
                Thread.Sleep(100);
            }
            return Task.CompletedTask;
        }
        private void VoidJob(IWebHostEnvironment logger)
        {
            Console.WriteLine("Hello World Void");
        }

        public class UserInfo : IUserInfo
        {
            public string ID { get; set; }
            public string Name { get; set; }
        }
        public IActionResult Login()
        {
            var jwt = this.BuildJwtSecurityToken(new UserInfo()
            {
                ID = StringHelper.RandCode(onlyNumber: true),
                Name = StringHelper.RandBlurCode(20)
            });
            this.SignInJwt(jwt);
            return (JsonResult)jwt;
        }

        public IActionResult Refresh(string refresh_token)
        {
            var jwt = this.RefreshJwtSecurityToken(refresh_token);
            if (jwt == null)
                return Content("refresh_token超时或已使用");
            this.SignInJwt(jwt);
            return (JsonResult)jwt;
        }

        [Authorize]
        public void TestAuth()
        {
            ControllerHelpers.InvokeControllerActionAsync<HomeController>("OtherAction", HttpContext, RouteData).Wait();
        }

        public IActionResult OtherAction()
        {
            var info = this.GetUserInfo<UserInfo>();
            return Content($"验证通过，身份信息：{info.ToJsonString()}");
        }

        public void RenderView()
        {
            ControllerHelpers.RenderRazorPageAsync("~/", "~/Views/Home/RenderView.cshtml", true, HttpContext, new RouteData()).Wait();
        }
    }
}