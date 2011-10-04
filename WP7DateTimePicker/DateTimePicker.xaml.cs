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

namespace WP7DateTimePicker
{
    public partial class DateTimePicker : UserControl
    {
        #region DateTime

        /// <summary>
        /// DateTime Dependency Property
        /// </summary>
        public static readonly DependencyProperty DateTimeProperty =
            DependencyProperty.Register("DateTime", typeof(DateTime), typeof(DateTimePicker),
                new PropertyMetadata((DateTime)DateTime.Now,
                    new PropertyChangedCallback(OnDateTimeChanged)));

        /// <summary>
        /// Gets or sets the DateTime property. This dependency property 
        /// indicates the selected date and or time.
        /// </summary>
        public DateTime DateTime
        {
            get { return (DateTime)GetValue(DateTimeProperty); }
            set { SetValue(DateTimeProperty, value); }
        }

        /// <summary>
        /// Handles changes to the DateTime property.
        /// </summary>
        private static void OnDateTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DateTimePicker target = (DateTimePicker)d;
            DateTime oldValue = (DateTime)e.OldValue;
            DateTime newValue = target.DateTime;
            target.OnDateTimeChanged(oldValue, newValue);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the DateTime property.
        /// </summary>
        protected virtual void OnDateTimeChanged(DateTime oldValue, DateTime newValue)
        {
            updateText(newValue);
        }

        #endregion

        #region PickDate

        /// <summary>
        /// PickDate Dependency Property
        /// </summary>
        public static readonly DependencyProperty PickDateProperty =
            DependencyProperty.Register("PickDate", typeof(bool), typeof(DateTimePicker),
                new PropertyMetadata((bool)true,
                    new PropertyChangedCallback(OnPickDateChanged)));

        /// <summary>
        /// Gets or sets the PickDate property. This dependency property 
        /// indicates if a date is to be picked.
        /// </summary>
        public bool PickDate
        {
            get { return (bool)GetValue(PickDateProperty); }
            set { SetValue(PickDateProperty, value); }
        }

        /// <summary>
        /// Handles changes to the PickDate property.
        /// </summary>
        private static void OnPickDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DateTimePicker target = (DateTimePicker)d;
            bool oldValue = (bool)e.OldValue;
            bool newValue = target.PickDate;
            target.OnPickDateChanged(oldValue, newValue);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the PickDate property.
        /// </summary>
        protected virtual void OnPickDateChanged(bool oldValue, bool newValue)
        {
        }

        #endregion

        #region PickTime

        /// <summary>
        /// PickTime Dependency Property
        /// </summary>
        public static readonly DependencyProperty PickTimeProperty =
            DependencyProperty.Register("PickTime", typeof(bool), typeof(DateTimePicker),
                new PropertyMetadata((bool)true,
                    new PropertyChangedCallback(OnPickTimeChanged)));

        /// <summary>
        /// Gets or sets the PickTime property. This dependency property 
        /// indicates if a time is to be picked.
        /// </summary>
        public bool PickTime
        {
            get { return (bool)GetValue(PickTimeProperty); }
            set { SetValue(PickTimeProperty, value); }
        }

        /// <summary>
        /// Handles changes to the PickTime property.
        /// </summary>
        private static void OnPickTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DateTimePicker target = (DateTimePicker)d;
            bool oldValue = (bool)e.OldValue;
            bool newValue = target.PickTime;
            target.OnPickTimeChanged(oldValue, newValue);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the PickTime property.
        /// </summary>
        protected virtual void OnPickTimeChanged(bool oldValue, bool newValue)
        {
        }

        #endregion



        public DateTimePicker()
        {
            InitializeComponent();
            updateText(this.DateTime);
            this.Loaded += new RoutedEventHandler(DateTimePicker_Loaded);
        }

        private void updateText(System.DateTime dt)
        {            
            if (this.PickDate && this.PickTime)
            {
                this.Text.Text = dt.ToString();
            }
            else if (this.PickDate)
            {
                this.Text.Text = dt.ToLongDateString();
            }
            else //time
            {
                this.Text.Text = dt.ToLongTimeString();
            }

        }

        void DateTimePicker_Loaded(object sender, RoutedEventArgs e)
        {
            if (PickerPage.DateTime.HasValue)
            {
                this.DateTime = PickerPage.DateTime.Value;
                PickerPage.DateTime = null;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PickerPage.DateTime = this.DateTime;
            PickerPage.DateVisible = this.PickDate;
            PickerPage.TimeVisible = this.PickTime;
            var frame = Application.Current.RootVisual as Microsoft.Phone.Controls.PhoneApplicationFrame;
            if (frame != null)
            {
                frame.Navigate(new Uri("/WP7DateTimePicker;component/PickerPage.xaml", UriKind.Relative));
            }
            else
            {
                throw new InvalidOperationException("Applications RootVisual must be a PhoneApplicationFrame");
            }
        }
    }
}
