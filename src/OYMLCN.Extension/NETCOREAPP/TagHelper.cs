#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Linq;
using OYMLCN.Extensions;
using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Html;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.Routing;
using System.IO;
#if NETCOREAPP3_1
using Microsoft.AspNetCore.Mvc.Razor.Infrastructure;
#endif

namespace Microsoft.AspNetCore.Mvc.TagHelpers
{
    [HtmlTargetElement("__taghelper__")]
    public class ViewContextTagHelper : TagHelper
    {
        [ViewContext]
        public ViewContext ViewContext { get; set; }
        /// <summary>
        /// 传入属性值对比是否等于当前请求的Controller
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public bool IsEqualController(string controller)
            => controller.Equals(ViewContext.RouteData.Values["controller"]?.ToString());
        /// <summary>
        /// 传入属性值对比是否等于当前请求的Controller
        /// </summary>
        /// <param name="controllers"></param>
        /// <returns></returns>
        public bool IsEqualControllers(string controllers)
            => IsEqualControllers(controllers.SplitAuto());
        /// <summary>
        /// 传入属性值对比是否等于当前请求的Controller
        /// </summary>
        /// <param name="controllers"></param>
        /// <returns></returns>
        public bool IsEqualControllers(params string[] controllers)
            => controllers.Contains(ViewContext.RouteData.Values["controller"]?.ToString());
        /// <summary>
        /// 传入属性值对比是否等于当前请求的Action
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool IsEqualAction(string action)
            => action.Equals(ViewContext.RouteData.Values["action"]?.ToString());
        /// <summary>
        /// 传入属性值对比是否等于当前请求的Action与Controller
        /// </summary>
        /// <param name="action"></param>
        /// <param name="controller"></param>
        /// <returns></returns>
        public bool IsEqualAction(string action, string controller)
            => controller.IsNullOrWhiteSpace() ? IsEqualAction(action) : IsEqualController(controller) && IsEqualAction(action);
        /// <summary>
        /// 传入属性值对比是否等于当前请求的Area区域
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public bool IsEqualArea(string area)
            => area.Equals(ViewContext.RouteData.Values["area"]?.ToString());
    }

    [HtmlTargetElement("input", Attributes = "asp-checked")]
    public class InputCheckedHelper : TagHelper
    {
        [HtmlAttributeName("asp-checked")]
        public bool? Attribute { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Attribute == true)
                output.Attributes.SetAttribute("checked", null);
            else
                output.RemoveAttribute("checked");

            base.Process(context, output);
        }
    }
    [HtmlTargetElement("option", Attributes = "asp-selected")]
    public class SelectOptionSelectedHelper : TagHelper
    {
        [HtmlAttributeName("asp-selected")]
        public bool? Attribute { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Attribute == true)
                output.Attributes.SetAttribute("selected", null);
            else
                output.RemoveAttribute("selected");
            base.Process(context, output);
        }
    }

    [HtmlTargetElement("*", Attributes = "all-class-data")]
    [HtmlTargetElement("*", Attributes = "class-*")]
    public class ClassNameAppendTagHelper : LayuiThisTagHelper
    {
        /// <summary>
        /// 根据条件添加class
        /// </summary>
        [HtmlAttributeName("all-class-data", DictionaryAttributePrefix = "class-")]
        public IDictionary<string, bool?> RouteValues { get; set; } = new Dictionary<string, bool?>();

        /// <summary>
        /// Process
        /// </summary>
        /// <param name="context"></param>
        /// <param name="output"></param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            foreach (var cn in RouteValues)
                if (cn.Value == true)
                    output.AddClass(cn.Key);
            base.Process(context, output);
        }
    }


#if NETCOREAPP3_1
    [HtmlTargetElement("link", Attributes = "href,auto-use-minify")]
    public class LinkAutoUseMinifyHelper : LinkTagHelper
    {
        public LinkAutoUseMinifyHelper(IWebHostEnvironment hostingEnvironment, TagHelperMemoryCacheProvider cacheProvider, IFileVersionProvider fileVersionProvider, HtmlEncoder htmlEncoder, JavaScriptEncoder javaScriptEncoder, IUrlHelperFactory urlHelperFactory) : base(hostingEnvironment, cacheProvider, fileVersionProvider, htmlEncoder, javaScriptEncoder, urlHelperFactory)
        {
        }

        /// <summary>
        /// 发布环境下将*.css更改为*.min.css
        /// </summary>
        [HtmlAttributeName("auto-use-minify")]
        public bool? AutoUseMinify { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (AutoUseMinify == true && !HostingEnvironment.IsDevelopment())
            {
                var pathQuery = Href.Split('?');
                var path = pathQuery[0];
                if (!Path.GetFileName(path).Contains(".min."))
                    pathQuery[0] = Path.ChangeExtension(path, $"min{Path.GetExtension(path)}");
                Href = pathQuery.Join("?");
                output.Attributes.SetAttribute("href", Href);
            }
            base.Process(context, output);
        }
    }
    [HtmlTargetElement("script", Attributes = "src,auto-use-minify")]
    public class SrciptAutoUseMinifyHelper : ScriptTagHelper
    {
        public SrciptAutoUseMinifyHelper(IWebHostEnvironment hostingEnvironment, TagHelperMemoryCacheProvider cacheProvider, IFileVersionProvider fileVersionProvider, HtmlEncoder htmlEncoder, JavaScriptEncoder javaScriptEncoder, IUrlHelperFactory urlHelperFactory) : base(hostingEnvironment, cacheProvider, fileVersionProvider, htmlEncoder, javaScriptEncoder, urlHelperFactory)
        {
        }
        /// <summary>
        /// 发布环境下将*.js更改为*.min.js
        /// </summary>
        [HtmlAttributeName("auto-use-minify")]
        public bool? AutoUseMinify { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (AutoUseMinify == true && !HostingEnvironment.IsDevelopment())
            {
                var pathQuery = Src.Split('?');
                var path = pathQuery[0];
                if (!Path.GetFileName(path).Contains(".min."))
                    pathQuery[0] = Path.ChangeExtension(path, $"min{Path.GetExtension(path)}");
                Src = pathQuery.Join("?");
                output.Attributes.SetAttribute("src", Src);
            }
            base.Process(context, output);
        }
    }
#endif
}

namespace OYMLCN.Extensions
{
    public static class TagHelperExtensions
    {
        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="output"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static TagHelperAttribute GetAttribute(this TagHelperOutput output, string name) =>
            output.Attributes.Where(d => d.Name.EqualsIgnoreCase(name)).FirstOrDefault();
        /// <summary>
        /// 移除并返回属性
        /// </summary>
        /// <param name="output"></param>
        /// <param name="name"></param>
        /// <returns>被移除的属性</returns>
        public static TagHelperAttribute RemoveAttribute(this TagHelperOutput output, string name)
        {
            var old = output.GetAttribute(name);
            if (old.IsNotNull())
                output.Attributes.Remove(old);
            return old;
        }

        /// <summary>
        /// 添加 Class 属性
        /// </summary>
        /// <param name="output"></param>
        /// <param name="className"></param>
        public static void AddClass(this TagHelperOutput output, string className)
        {
            var classNames = output.RemoveAttribute("class")?.Value.ToString().Split(" ").ToList() ?? new List<string>();
            classNames.Add(className);
            output.Attributes.Add("class", classNames.Distinct().Join(" "));
        }
    }
}