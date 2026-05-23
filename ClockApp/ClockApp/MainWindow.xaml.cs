using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ClockApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TxtAlarmTime.Text = DateTime.Now.AddMinutes(1).ToString("HH:mm");
        }

        private void ComboFaceColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MyClock == null) return;
            var item = (ComboBoxItem)ComboFaceColor.SelectedItem;
            MyClock.ClockBackendColor = (Brush)new BrushConverter().ConvertFromString(item.Content.ToString());
        }

        private void ComboArrowsColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MyClock == null) return;
            var item = (ComboBoxItem)ComboArrowsColor.SelectedItem;
            MyClock.ArrowsColor = (Brush)new BrushConverter().ConvertFromString(item.Content.ToString());
        }

        private void ChkAlarmEnabled_Checked(object sender, RoutedEventArgs e)
        {
            if (TimeSpan.TryParse(TxtAlarmTime.Text, out TimeSpan parsedTime))
            {
                MyClock.AlarmTime = parsedTime;
                MyClock.IsAlarmEnabled = true;
                TxtAlarmTime.IsEnabled = false; // блокируем ввод времени во время работы будильника
            }
            else
            {
                MessageBox.Show("Формат должен быть ЧЧ:ММ!");
                ChkAlarmEnabled.IsChecked = false;
            }
        }

        private void ChkAlarmEnabled_Unchecked(object sender, RoutedEventArgs e)
        {
            MyClock.IsAlarmEnabled = false;
            TxtAlarmTime.IsEnabled = true;
        }

        // Ловим событие "Будильник сработал" из нашего UserControl
        private void MyClock_AlarmTriggered(object sender, EventArgs e)
        {
            MessageBox.Show("⏰ ДЗЫЫЫЫНЬ! Будильник сработал!", "Будильник", MessageBoxButton.OK, MessageBoxImage.Information);
            ChkAlarmEnabled.IsChecked = false; 
        }
    }
}