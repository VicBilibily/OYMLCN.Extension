#if NET461
using System.Windows.Forms;

namespace OYMLCN.WinFormExtensions
{
    /// <summary>
    /// ListViewExtension
    /// </summary>
    public static class ListViewExtensions
    {
        /// <summary>
        /// 1.开始更新
        /// </summary>
        /// <param name="view"></param>
        public static void _1_BeginUpdate(this ListView view)
        {
            view.BeginUpdate();
            view.Clear();
            view.GridLines = true;
            view.View = View.Details;
            view.Scrollable = true;
            view.FullRowSelect = true;
        }
        /// <summary>
        /// 2.添加标题
        /// </summary>
        /// <param name="view"></param>
        /// <param name="title"></param>
        /// <param name="width"></param>
        public static void _2_AddHeader(this ListView view, string title, int width = 100) => view.Columns.Add(title, width);
        /// <summary>
        /// 3.添加数据
        /// </summary>
        /// <param name="view"></param>
        /// <param name="data"></param>
        public static void _3_AddData(this ListView view, params object[] data)
        {
            if (data.Length == 0)
                return;
            ListViewItem lvi = new ListViewItem()
            {
                Text = data[0].ToString()
            };
            for (var i = 1; i < data.Length; i++)
                lvi.SubItems.Add(data[i]?.ToString());
            view.Items.Add(lvi);
        }
        /// <summary>
        /// 4.更新完毕
        /// </summary>
        /// <param name="view"></param>
        public static void _4_EndUpdate(this ListView view)
        {
            view.EndUpdate();
            view.SelectedItems.Clear();
        }
    }
}
#endif