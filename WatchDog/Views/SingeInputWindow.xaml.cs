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
    /// Логика взаимодействия для SingleInputWindow.xaml
    /// </summary>
    public partial class SingleInputWindow : Window
    {
        public string InputValue { get; private set; }

        public SingleInputWindow(string label)
        {
            InitializeComponent();
            Label.Text = label; // Установка текста метки
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            InputValue = InputTextBox.Text;
            DialogResult = true; // Закрыть окно с возвратом результата
            Close();
        }
    }
}
