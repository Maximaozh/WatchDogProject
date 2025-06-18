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
    
    public partial class AccessTimeWindow : Window
    {
        private CheckRepository _db;
        private List<StatusMonitor> _monitors;
        private bool _report;
        public AccessTimeWindow(CheckRepository db, List<StatusMonitor> monitor = null, bool report = false)
        {
            InitializeComponent();
            _db = db;
            _monitors = monitor;
            if (monitor != null && monitor.Count == 1)
                SiteAddressTextBox.Text = _monitors.First<StatusMonitor>().Address;
            
            if(_monitors.Count > 1)
            {
                SiteAddressLabel.Visibility = Visibility.Collapsed;
                SiteAddressTextBox.Visibility = Visibility.Collapsed;
            }

            _report = report;
        }

        private void OnStartButtonClick(object sender, RoutedEventArgs e)
        {
            string siteAddress = SiteAddressTextBox.Text;
            DateTime? startDate = StartDatePicker.SelectedDate;
            DateTime? endDate = EndDatePicker.SelectedDate;

            if (string.IsNullOrWhiteSpace(siteAddress) && _monitors.Count < 2)
            {
                MessageBox.Show("Пожалуйста, введите адрес сайта.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!startDate.HasValue)
            {
                MessageBox.Show("Пожалуйста, выберите дату начала.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!endDate.HasValue)
            {
                MessageBox.Show("Пожалуйста, выберите дату окончания.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (startDate > endDate)
            {
                MessageBox.Show("Пожалуйста, введите корректную дату начала и завершения отслеживания сайта.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if(_report)
            {
                Random rand = new Random();
                string filePath = "UPTIME " +
                   DateTime.Now.Date.Day.ToString() + "" +
                   DateTime.Now.Date.Month.ToString() + "" +
                   DateTime.Now.Date.Year.ToString() + " (" +
                   rand.Next() + ")" +
                   DateTime.Now.Date.Hour.ToString() + " .docx";

                ReportGenerator reportGenerator = new ReportGenerator();
                reportGenerator.GenerateTimeWorkReport(_db.GetUpStateInfoInRange(startDate.Value, endDate.Value), filePath, startDate.ToString() + " -> " + startDate.ToString());
                Close();
            } else
            {
                double time = AccessTime.CalculateAvailabilityPercentage(startDate.Value, endDate.Value, siteAddress, _db.ConnectionString);
                MessageBox.Show("На заданный период времени доступность сайта составляет:" + time.ToString("0.00") + "%", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }           
        }
    }
}
