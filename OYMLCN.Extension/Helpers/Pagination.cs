using System;
using System.Collections.Generic;

namespace OYMLCN.Helpers
{
    /// <summary>
    /// 分页页码辅助
    /// </summary>
    public class PaginationHelpers
    {
        /// <summary>
        /// 总数据长度
        /// </summary>
        public long Total { get; private set; }
        /// <summary>
        /// 单页条目
        /// </summary>
        public int Limit { get; private set; }
        /// <summary>
        /// 有效页码
        /// </summary>
        public int Page { get; internal set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int Pages { get; internal set; }

        /// <summary>
        /// 上一页页码
        /// </summary>
        public int PrevPage { get; internal set; }
        /// <summary>
        /// 下一页页码
        /// </summary>
        public int NextPage { get; internal set; }
        /// <summary>
        /// 是否可显示首页
        /// </summary>
        public bool ShowFirst { get; internal set; }
        /// <summary>
        /// 是否可显示末页
        /// </summary>
        public bool ShowLast { get; internal set; }
        /// <summary>
        /// 是否需要左分隔符
        /// </summary>
        public bool LeftSplit { get; internal set; }
        /// <summary>
        /// 是否需要右分隔符
        /// </summary>
        public bool RightSplit { get; internal set; }
        /// <summary>
        /// 当前是最后一页
        /// </summary>
        public bool IsLastPage => Page == Pages;
        /// <summary>
        /// 是否显示下一页
        /// </summary>
        public bool ShowPrev => Page > 1 && Pages > 1;
        /// <summary>
        /// 是否显示下一页
        /// </summary>
        public bool ShowNext => Page != Pages && Pages > 2;
        /// <summary>
        /// 主要页码数组
        /// </summary>
        public int[] PageArray { get; internal set; }

        /// <summary>
        /// 分页页码辅助
        /// </summary>
        /// <param name="total">总页码</param>
        /// <param name="page">当前页码</param>
        /// <param name="limit">单页数量</param>
        /// <param name="length">页码栏长度</param>
        public PaginationHelpers(long total, int page = 1, int limit = 10, int length = 5)
        {
            Total = total;
            Limit = limit;
            Pages = Convert.ToInt32(Total / Limit + (Total % Limit == 0 ? 0 : 1)); ;
            GetValidPage(page);
            GetPaginationArray(length);
            var willDelete = MaxPagination;
        }

        /// <summary>
        /// 最大页码
        /// </summary>
        [Obsolete("please use Pages, will be delete in next main version.")]
        public int MaxPagination => Pages;

        /// <summary>
        /// 获取有效页码
        /// </summary>
        /// <param name="targetPage">要跳转的页码</param>
        /// <returns></returns>
        public int GetValidPage(int targetPage)
        {
            int max = Pages;
            int page;
            if (max == 0)
                page = 0;
            else if (targetPage <= 0)
                page = 1;
            else if (targetPage > max)
                page = max;
            else
                page = targetPage;

            if (page > 0)
            {
                PrevPage = page - 1;
                NextPage = page + 1;
                if (PrevPage < 1) PrevPage = page;
                if (NextPage > max) NextPage = max;
            }

            this.Page = page;
            return page;
        }

        /// <summary>
        /// 获取基本页码列表
        /// </summary>
        /// <param name="length"></param>
        public PaginationHelpers GetPaginationArray(int length = 5)
        {
            var mid = new List<int>();

            int max = Pages, page = Page, len = length;
            if (len % 2 == 0) { len++; }
            var half = len / 2;
            var start = page - half;
            var end = page + half;
            if (start < 1)
            {
                var p = 1 - start;
                start += p;
                end += p;
            }
            if (end > max)
            {
                var p = end - max;
                end = max;
                if (start - p >= 1) { start -= p; }
                else { start = 1; }
            }

            for (int i = start; i <= end; i++)
                mid.Add(i);

            this.PageArray = mid.ToArray();
            this.LeftSplit = start > 2;
            this.RightSplit = end + 1 < Pages;
            if (start > 1) this.ShowFirst = true;
            if (end > 2 && end != max) this.ShowLast = true;
            return this;
        }

    }
}
