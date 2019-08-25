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
using System.Windows.Threading;

namespace AnalogClockHost
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer _timer;
        private DateTime _now = DateTime.Now;

        public MainWindow()
        {
            InitializeComponent();

            AnalogClockView.TargetDateTime = _now;
            TargetDateTimeText.Text = _now.ToString();

            _timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(3)
            };

            _timer.Tick += _timer_Tick;
            _timer.Start();
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            _now = _now.AddMinutes(3);
            AnalogClockView.TargetDateTime = _now;
            TargetDateTimeText.Text = _now.ToString();
        }
    }
}
