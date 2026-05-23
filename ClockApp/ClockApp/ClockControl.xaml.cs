using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace ClockApp
{
    public partial class ClockControl : UserControl
    {
        private DispatcherTimer _timer;
        private bool _isAlarmTriggeredToday = false;

        public event EventHandler AlarmTriggered;

        public static readonly DependencyProperty ArrowsColorProperty =
            DependencyProperty.Register("ArrowsColor", typeof(Brush), typeof(ClockControl), new PropertyMetadata(Brushes.Black));

        public Brush ArrowsColor
        {
            get => (Brush)GetValue(ArrowsColorProperty);
            set => SetValue(ArrowsColorProperty, value);
        }

        public static readonly DependencyProperty ClockBackendColorProperty =
            DependencyProperty.Register("ClockBackendColor", typeof(Brush), typeof(ClockControl), new PropertyMetadata(Brushes.White));

        public Brush ClockBackendColor
        {
            get => (Brush)GetValue(ClockBackendColorProperty);
            set => SetValue(ClockBackendColorProperty, value);
        }

        public static readonly DependencyProperty AlarmTimeProperty =
            DependencyProperty.Register("AlarmTime", typeof(TimeSpan?), typeof(ClockControl), new PropertyMetadata(null));

        public TimeSpan? AlarmTime
        {
            get => (TimeSpan?)GetValue(AlarmTimeProperty);
            set => SetValue(AlarmTimeProperty, value);
        }

        public static readonly DependencyProperty IsAlarmEnabledProperty =
            DependencyProperty.Register("IsAlarmEnabled", typeof(bool), typeof(ClockControl), new PropertyMetadata(false));

        public bool IsAlarmEnabled
        {
            get => (bool)GetValue(IsAlarmEnabledProperty);
            set => SetValue(IsAlarmEnabledProperty, value);
        }

        public ClockControl()
        {
            InitializeComponent();

            // Запускаем таймер, который будет тикать каждую секунду
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (s, e) => UpdateClock();
            _timer.Start();

            UpdateClock(); // Сразу ставим стрелки в текущее время при запуске
        }

        private void UpdateClock()
        {
            DateTime now = DateTime.Now;

            // Считаем углы поворота в градусах
            double secAngle = now.Second * 6; // 360 градусов / 60 секунд = 6 градусов на секунду
            double minAngle = (now.Minute * 6) + (now.Second * 0.1);
            double hourAngle = ((now.Hour % 12) * 30) + (now.Minute * 0.5);

            SecondTransform.Angle = secAngle;
            MinuteTransform.Angle = minAngle;
            HourTransform.Angle = hourAngle;

            // Проверяем будильник
            if (IsAlarmEnabled && AlarmTime.HasValue)
            {
                if (now.Hour == AlarmTime.Value.Hours && now.Minute == AlarmTime.Value.Minutes)
                {
                    if (!_isAlarmTriggeredToday)
                    {
                        _isAlarmTriggeredToday = true;
                        AlarmTriggered?.Invoke(this, EventArgs.Empty); // Стреляем событием!
                    }
                }
                else
                {
                    _isAlarmTriggeredToday = false;
                }
            }
        }
    }
}