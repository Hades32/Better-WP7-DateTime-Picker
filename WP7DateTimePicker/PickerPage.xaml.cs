using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Text.RegularExpressions;

namespace WP7DateTimePicker
{
    public partial class PickerPage : PhoneApplicationPage
    {
        public static DateTime? DateTime { get; set; }
        public static bool TimeVisible { get; set; }
        public static bool DateVisible { get; set; }

        static PickerPage()
        {
            DateTime = null;
            TimeVisible = true;
            DateVisible = true;
        }

        public PickerPage()
        {
            InitializeComponent();

            if (!TimeVisible && !DateVisible)
                throw new InvalidOperationException("Either Time or Date bust be visible!");
            else
            {
                this.timeStack.Visibility = TimeVisible ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
                this.dateStack.Visibility = DateVisible ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            }

            if (DateTime == null)
                DateTime = System.DateTime.Now;

            this.hourTB.Text = DateTime.Value.Hour.ToString();
            this.minuteTB.Text = DateTime.Value.Minute.ToString();

            this.dayTB.Text = DateTime.Value.Day.ToString();
            this.monthTB.Text = DateTime.Value.Month.ToString();
            this.yearTB.Text = DateTime.Value.Year.ToString();
        }

        private void OKappbtn_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime = new DateTime(toInt(this.yearTB), toInt(this.monthTB), toInt(this.dayTB),
                                        toInt(this.hourTB), toInt(this.minuteTB), 0);
                this.NavigationService.GoBack();
            }
            catch
            {
                MessageBox.Show("Not a valid date or time");
            }
        }

        private void CancelAppBtn_Click(object sender, EventArgs e)
        {
            DateTime = null;
            try
            {
                this.NavigationService.GoBack();
            }
            catch
            { /* navigation... */ }
        }

        private static int toInt(TextBox tb)
        {
            if (string.IsNullOrEmpty(tb.Text))
                return 0;
            else
                return Convert.ToInt32(tb.Text);
        }

        private void TB_GotFocus(object sender, RoutedEventArgs e)
        {
            var tb = (TextBox)sender;
            tb.SelectAll();
        }

        private void hourTB_KeyDown(object sender, KeyEventArgs e)
        {
            checkNumber(e, 0, 23, 2, this.minuteTB);
        }

        private void minuteTB_KeyDown(object sender, KeyEventArgs e)
        {
            checkNumber(e, 0, 59, 2, this.dayTB);
        }

        private void dayTB_KeyDown(object sender, KeyEventArgs e)
        {
            checkNumber(e, 1, 31, 2, this.monthTB);
        }

        private void monthTB_KeyDown(object sender, KeyEventArgs e)
        {
            checkNumber(e, 1, 12, 2, this.yearTB);
        }

        private void yearTB_KeyDown(object sender, KeyEventArgs e)
        {
            checkNumber(e, System.DateTime.MinValue.Year, System.DateTime.MaxValue.Year, 4, this.hourTB);
        }

        Regex numberKey = new Regex("^D[0-9]$", RegexOptions.Compiled);

        private void checkNumber(KeyEventArgs e, int min, int max, int max_length, TextBox nextTB)
        {
            if (numberKey.IsMatch(e.Key.ToString()))
            {
                var newText = ((TextBox)e.OriginalSource).Text + e.Key.ToString()[1];
                var sel = ((TextBox)e.OriginalSource).SelectedText;
                if (string.IsNullOrEmpty(sel) == false)
                {
                    newText = newText.Replace(sel, "");
                }
                int x;
                if (int.TryParse(newText, out x))
                {
                    if (x > max || newText.Length > max_length)
                    {
                        e.Handled = true;
                        nextTB.Focus();
                    }
                }
            }
            else if (e.Key == Key.Back)
            {
                // always ok
            }
            else if (e.PlatformKeyCode == 190) //if (e.Key == Key.Decimal) doesn't work...
            {
                // neither date or time needs a decimal point (or comma) so
                // we (mis-)use it as a "tab" key
                e.Handled = true;
                nextTB.Focus();
            }
            else
            {
                e.Handled = true;
            }
        }
    }
}