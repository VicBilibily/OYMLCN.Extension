using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace OYMLCN.Helpers
{
    /// <summary>
    /// ControllerHelpers
    /// </summary>
    public static class ControllerHelpers
    {
        /// <summary>
        /// 调用控制器并由控制器修改Response输出结果（调用此方法需要注入方法构造工厂 IActionInvokerFactory）
        /// <para>services.AddControllers();</para>
        /// <para>services.AddMvc();</para>
        /// </summary>
        public static async Task InvokeControllerActionAsync<TController>(string actionName, HttpContext context, RouteData routeData)
        {
            var actionDesciptor = CreateActionDescriptor<TController>(actionName, routeData);
            var actionContext = new ActionContext(context, routeData, actionDesciptor);
            var actionInvokerFactory = context.RequestServices.GetRequiredService<IActionInvokerFactory>(); //ActionInvokerFactory
            var invoker = actionInvokerFactory.CreateInvoker(actionContext); //ControllerActionInvoker
            await invoker.InvokeAsync();
        }
        private static ActionDescriptor CreateActionDescriptor<TController>(string actionName, RouteData routeData)
        {
            var controllerType = typeof(TController);
            var actionDesciptor = new ControllerActionDescriptor()
            {
                ControllerName = controllerType.Name,
                ActionName = actionName,
                FilterDescriptors = new List<FilterDescriptor>(),
                MethodInfo = controllerType.GetMethod(actionName, BindingFlags.Public | BindingFlags.Instance),
                ControllerTypeInfo = controllerType.GetTypeInfo(),
                Parameters = new List<ParameterDescriptor>(),
                Properties = new Dictionary<object, object>(),
                BoundProperties = new List<ParameterDescriptor>()
            };

            if (actionDesciptor.MethodInfo == null)
                throw new ArgumentNullException($"未找到名称为 {actionName} 的公开方法");

            // 把控制器和方法名称加入路由，以用于视图查找
            routeData.Values.TryAdd("controller", actionDesciptor.ControllerName.Replace("Controller", ""));
            routeData.Values.TryAdd("action", actionDesciptor.ActionName);

            // 绑定方法调用数据模型
            foreach (var routeValue in routeData.Values)
            {
                var parameter = new ParameterDescriptor();
                parameter.Name = routeValue.Key;
                var attributes = new object[]
                {
                    new FromRouteAttribute { Name = parameter.Name },
                };
                parameter.BindingInfo = BindingInfo.GetBindingInfo(attributes);
                parameter.ParameterType = routeValue.Value.GetType();
                actionDesciptor.Parameters.Add(parameter);
            }

            return actionDesciptor;
        }

        /// <summary>
        /// 渲染Razor视图（调用此方法需要注入视图渲染引擎 ICompositeViewEngine）
        /// <para>services.AddControllersWithViews();</para>
        /// <para>services.AddMvc();</para>
        /// </summary>
        /// <param name="executingFilePath">当前执行视图的绝对路径[~/]</param>
        /// <param name="viewPath">视图的路径[~/Views/---/---.cshtml]</param>
        /// <param name="isMainPage">正在查找的页是否为Action的主页</param>
        /// <param name="context">请求上下文</param>
        /// <param name="routeData">路由数据</param>
        /// <returns></returns>
        public static async Task RenderRazorPageAsync(string executingFilePath, string viewPath, bool isMainPage, HttpContext context, RouteData routeData)
        {
            // ContentType设置为text/html，使浏览器以正常页面的格式显示
            context.Response.ContentType = "text/html";

            // 获取渲染引擎
            var engine = context.RequestServices.GetRequiredService<ICompositeViewEngine>();
            // 找到特定视图
            var viewResult = engine.GetView(executingFilePath, viewPath, true);
            if (!viewResult.Success)
                throw new FileNotFoundException("未能找到指定的视图文件", viewPath);

            // 创建临时的StringWriter实例，配置到视图上下文中
            using (var output = new StringWriter())
            {
                //视图上下文对于视图渲染来说很重要，视图中的前后台交互都需要它
                var viewContext = new ViewContext()
                {
                    HttpContext = context,
                    Writer = output,
                    RouteData = routeData,
                    View = viewResult.View,
                    FormContext = new FormContext(),
                    ActionDescriptor = new ActionDescriptor()
                };
                // 对视图进行渲染
                await viewResult.View.RenderAsync(viewContext);
                // 将输出写到响应
                await context.Response.WriteAsync(output.ToString());
            }
        }

    }
}
