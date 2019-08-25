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

        public static readonly DependencyProperty AutoBaseDateTimeProperty = DependencyProperty.Register(
          "AutoBaseDateTime",
          typeof(bool),
          typeof(AnalogClock),
          new FrameworkPropertyMetadata(false,
              FrameworkPropertyMetadataOptions.AffectsRender,
              new PropertyChangedCallback(OnAutoBaseDateTimeChanged)
          )
        );

        public bool AutoBaseDateTime
        {
            get { return (bool)GetValue(AutoBaseDateTimeProperty); }
            set { SetValue(AutoBaseDateTimeProperty, value); }
        }

        public static readonly DependencyProperty DaysLaterIndicationVisibilityProperty = DependencyProperty.Register(
          "DaysLaterIndicationVisibility",
          typeof(Visibility),
          typeof(AnalogClock),
          new FrameworkPropertyMetadata(Visibility.Visible,
              FrameworkPropertyMetadataOptions.AffectsRender,
              new PropertyChangedCallback(OnDaysLaterIndicationVisibilityChanged)
          )
        );

        public Visibility DaysLaterIndicationVisible
        {
            get { return (Visibility)GetValue(DaysLaterIndicationVisibilityProperty); }
            set { SetValue(DaysLaterIndicationVisibilityProperty, value); }
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
                    analogClock.AMPMDesignator.Text = newDateTime.ToString("tt");

                    if (analogClock.AutoBaseDateTime)
                    {
                        analogClock.BaseDateTime = DateTime.Today;
                    }

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

        private static void OnAutoBaseDateTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is bool newAutoBaseDateTime)
            {
                if (d is AnalogClock analogClock)
                {
                    if (newAutoBaseDateTime)
                    {
                        analogClock.BaseDateTime = DateTime.Today;
                    }

                    ChangeDaysAfterText(d, analogClock.BaseDateTime);
                }
            }
        }

        private static void ChangeDaysAfterText(DependencyObject d, DateTime baseDateTime)
        {
            if (d is AnalogClock analogClock)
            {
                int daysLater = (int)((analogClock.TargetDateTime.Date - baseDateTime.Date).TotalDays);

                string daysLaterText;
                if (daysLater == 0)
                {
                    daysLaterText = "当日";
                }
                else if (daysLater == 1)
                {
                    daysLaterText = "翌日";
                }
                else if (daysLater == -1)
                {
                    daysLaterText = "前日";
                }
                else if (daysLater > 1)
                {
                    daysLaterText = $"{ daysLater }日後";
                }
                else
                {
                    daysLaterText = $"{ daysLater * -1 }日前";
                }

                analogClock.DaysLaterIndication.Text = daysLaterText;
            }
        }

        private static void OnDaysLaterIndicationVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is Visibility visibility)
            {
                if (d is AnalogClock analogClock)
                {
                    analogClock.DaysLaterIndication.Visibility = visibility;
                }
            }
        }
    }
}
