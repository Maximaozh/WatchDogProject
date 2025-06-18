using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using DocumentFormat.OpenXml.InkML;

namespace WatchDog.Views
{


    public partial class ReportWindow : Window
    {
        private ReportGenerator Report { get; set; }
        private CheckRepository _dbContext { get; set; }

        public ObservableCollection<string> Methods { get; set; }

        public ReportWindow(CheckRepository dbContext)
        {
            InitializeComponent();
            Report = new ReportGenerator();
            Methods = new ObservableCollection<string>();
            _dbContext = dbContext;

            Methods.Add("Записи по адресу");
            Methods.Add("Записи с ошибками");
            Methods.Add("Последние записи");

            this.DataContext = this;
        }

        private void ComboBoxReportType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lblAddress.Visibility = comboBoxReportType.SelectedItem.ToString() == "Записи по адресу" ? Visibility.Visible : Visibility.Collapsed;
            txtAddress.Visibility = lblAddress.Visibility;
        }

        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            lblStatus.Content = "Генерация отчета...";
            lblStatus.Foreground = System.Windows.Media.Brushes.Red;

            try
            {
                int recordCount = int.Parse(txtRecordCount.Text);

                Random rand = new Random();
                string filePath = "ALL " +
                   DateTime.Now.Date.Day.ToString() + "" +
                   DateTime.Now.Date.Month.ToString() + "" +
                   DateTime.Now.Date.Year.ToString() + " (" +
                   rand.Next() + ")" +
                   DateTime.Now.Date.Hour.ToString() + " .docx";


                if (comboBoxReportType.SelectedItem.ToString() == "Последние записи")
                {
                    var lastRecords = _dbContext.GetLastRecords(recordCount);
                    Report.GenerateReport(lastRecords, filePath);
                }
                else if (comboBoxReportType.SelectedItem.ToString() == "Записи с ошибками")
                {
                    var errorRecords = _dbContext.GetErrorResults(recordCount);
                    Report.GenerateReport(errorRecords, filePath);
                }
                else if (comboBoxReportType.SelectedItem.ToString() == "Записи по адресу")
                {
                    string address = txtAddress.Text;
                    var addressRecords = _dbContext.GetResultsByAddress(address, recordCount);
                    Report.GenerateReport(addressRecords, filePath);
                }

                lblStatus.Content = $"Отчет успешно создан: {filePath}";
                lblStatus.Foreground = System.Windows.Media.Brushes.Green;
            }
            catch (Exception ex)
            {
                lblStatus.Content = $"Ошибка: {ex.Message}";
                lblStatus.Foreground = System.Windows.Media.Brushes.Red;
            }
            this.Close();
        }
    }
}
