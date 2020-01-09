using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace OYMLCN.WPF.Extensions
{
    /// <summary>
    /// TextBoxExtension
    /// </summary>
    public static class TextBoxExtensions
    {
        /// <summary>
        /// 只允许输入数字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void OnlyNumberKeyDown(object sender, KeyEventArgs e)
        {
            TextBox txt = sender as TextBox;
            if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Decimal || e.Key == Key.Enter)
            {
                if (txt.Text.Contains(".") && e.Key == Key.Decimal)
                {
                    e.Handled = true;
                    return;
                }
                e.Handled = false;
            }
            else if (((e.Key >= Key.D0 && e.Key <= Key.D9) || e.Key == Key.OemPeriod) && e.KeyboardDevice.Modifiers != ModifierKeys.Shift)
            {
                if (txt.Text.Contains(".") && e.Key == Key.OemPeriod)
                {
                    e.Handled = true;
                    return;
                }
                e.Handled = false;
            }
            else
                e.Handled = true;
        }
        /// <summary>
        /// 只允许包含数字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void OnlyNumberTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            TextChange[] change = new TextChange[e.Changes.Count];
            e.Changes.CopyTo(change, 0);

            int offset = change[0].Offset;
            if (change[0].AddedLength > 0)
            {
                if (!double.TryParse(textBox.Text, out double num))
                {
                    textBox.Text = textBox.Text.Remove(offset, change[0].AddedLength);
                    textBox.Select(offset, 0);
                }
            }
        }

    }
}
