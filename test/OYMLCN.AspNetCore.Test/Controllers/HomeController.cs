using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OYMLCN.Extensions;
using OYMLCN.Helpers;

namespace OYMLCN.AspNetCore.Test.Controllers
{

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Content("Hello World!");
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
        public IActionResult TestAuth()
        {
            var info = this.GetUserInfo<UserInfo>();
            return Content($"验证通过，身份信息：{info.ToJsonString()}");
        }
    }
}