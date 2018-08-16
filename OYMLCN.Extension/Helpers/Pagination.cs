using System;

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
        /// 分页页码辅助
        /// </summary>
        /// <param name="total">总页码</param>
        /// <param name="limit">单页数量</param>
        public PaginationHelpers(long total, int limit = 10)
        {
            Total = total;
            Limit = limit;
        }

        /// <summary>
        /// 最大页码
        /// </summary>
        public int MaxPagination => Convert.ToInt32(Total / Limit + (Total % Limit == 0 ? 0 : 1));

        /// <summary>
        /// 获取有效页码
        /// </summary>
        /// <param name="targetPage">要跳转的页码</param>
        /// <returns></returns>
        public int GetValidPage(int targetPage)
        {
            int page = 0;
            int max = MaxPagination;
            if (max == 0)
                page = 0;
            else if (targetPage <= 0)
                page = 1;
            else if (targetPage > max)
                page = max;
            else
                page = targetPage;
            return Page = page;
        }

        ///// <summary>
        ///// 获取基本页码列表
        ///// </summary>
        ///// <param name="currentPage"></param>
        ///// <param name="pageArray"></param>
        ///// <param name="length"></param>
        //public void GetPaginationArray(int currentPage, out int[] pageArray, int length = 8)
        //{
        //    pageArray = new int[0];
        //}

    }
}
