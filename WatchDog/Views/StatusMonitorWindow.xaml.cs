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
using System.Windows.Shapes;

namespace WatchDog.Views
{
    /// <summary>
    /// Логика взаимодействия для StatusMonitorWindow.xaml
    /// </summary>
    public partial class StatusMonitorWindow : Window
    {
        public StatusMonitor? MonitorToEdit { get; set; }
        public StatusMonitor NewMonitor { get; private set; }

        public StatusMonitorWindow(List<CheckBase> methods, StatusMonitor? monitorToEdit = null)
        {
            InitializeComponent();
            MonitorToEdit = monitorToEdit;

            // Заполняем комбобокс методами
            CheckMethodComboBox.ItemsSource = methods;

            if (monitorToEdit != null)
            {
                // Если редактируем элемент, заполняем поля
                AddressTextBox.Text = monitorToEdit.Address;
                IntervalTextBoxs.Text = monitorToEdit.Interval.ToString();
                CheckMethodComboBox.SelectedItem = monitorToEdit.CheckMethod;
                ConfirmButton.Content = "Сохранить";
            }
            else
            {
                NewMonitor = new StatusMonitor(); // Создаем новый экземпляр
                ConfirmButton.Content = "Добавить";
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (MonitorToEdit != null)
            {
                // Обновляем существующий экземпляр
                MonitorToEdit.Address = AddressTextBox.Text;
                MonitorToEdit.Interval = double.TryParse(IntervalTextBoxs.Text, out var interval) ? interval : 10000;
                MonitorToEdit.CheckMethod = (CheckBase)CheckMethodComboBox.SelectedItem;
            }
            else
            {
                // Создаем новый экземпляр
                NewMonitor = new StatusMonitor
                {
                    Address = AddressTextBox.Text,
                    Interval = double.TryParse(IntervalTextBoxs.Text, out var interval) ? interval : 10000,
                    CheckMethod = (CheckBase)CheckMethodComboBox.SelectedItem
                };
            }

            DialogResult = true; // Устанавливаем результат диалога как "успех"
            Close(); // Закрываем окно
        }
    }
}
