using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AnalogClockHost
{
    /// <summary>
    /// AnalogClock.xaml の相互作用ロジック
    /// </summary>
    public partial class AnalogClock : UserControl
    {
        public AnalogClock()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty TargetDateTimeProperty = DependencyProperty.Register(
          "TargetDateTime",
          typeof(DateTime),
          typeof(AnalogClock),
          new FrameworkPropertyMetadata(DateTime.MinValue,
              FrameworkPropertyMetadataOptions.AffectsRender,
              new PropertyChangedCallback(OnTargetDateTimeChanged)
          )
        );

        public DateTime TargetDateTime
        {
            get { return (DateTime)GetValue(TargetDateTimeProperty); }
            set { SetValue(TargetDateTimeProperty, value); }
        }

        public static readonly DependencyProperty BaseDateTimeProperty = DependencyProperty.Register(
          "BaseDateTime",
          typeof(DateTime),
          typeof(AnalogClock),
          new FrameworkPropertyMetadata(DateTime.Now,
              FrameworkPropertyMetadataOptions.AffectsRender,
              new PropertyChangedCallback(OnBaseDateTimeChanged)
          )
        );

        public DateTime BaseDateTime
        {
            get { return (DateTime)GetValue(BaseDateTimeProperty); }
            set { SetValue(BaseDateTimeProperty, value); }
        }

        private static void OnTargetDateTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is DateTime newDateTime)
            {
                DateTime zeroMinute = new DateTime(newDateTime.Year, newDateTime.Month, newDateTime.Day, newDateTime.Hour, 0, 0);
                double longHandAngle = (newDateTime - zeroMinute).TotalMinutes / 60.0 * 360;
                
                DateTime zeroHour = newDateTime.Date;
                double shortHandAngle = (newDateTime - zeroHour).TotalHours % 12.0 / 12.0 * 360;

                if (d is AnalogClock analogClock)
                {
                    analogClock.LongHand.RenderTransform = new RotateTransform(longHandAngle);
                    analogClock.ShortHand.RenderTransform = new RotateTransform(shortHandAngle);
                    analogClock.AMPMText.Text = newDateTime.ToString("tt");

                    ChangeDaysAfterText(d, analogClock.BaseDateTime);
                }
            }
        }

        private static void OnBaseDateTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is DateTime newBaseDateTime)
            {
                ChangeDaysAfterText(d, newBaseDateTime);
            }
        }

        private static void ChangeDaysAfterText(DependencyObject d, DateTime baseDateTime)
        {
            if (d is AnalogClock analogClock)
            {
                int daysAfter = (int)((analogClock.TargetDateTime.Date - baseDateTime.Date).TotalDays);

                string daysAfterText = null;
                if (daysAfter == 0)
                {
                    daysAfterText = "当日";
                }
                else if (daysAfter == 1)
                {
                    daysAfterText = "翌日";
                }
                else if (daysAfter == -1)
                {
                    daysAfterText = "前日";
                }
                else if (daysAfter > 1)
                {
                    daysAfterText = $"{ daysAfter }日後";
                }
                else
                {
                    daysAfterText = $"{ daysAfter * -1 }日前";
                }

                analogClock.DaysAfterText.Text = daysAfterText;
            }
        }
    }
}
