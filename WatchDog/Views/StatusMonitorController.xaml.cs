using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace WatchDog
{
    /// <summary>
    /// Логика взаимодействия для StatusMonitorController.xaml
    /// </summary>
    public partial class StatusMonitorController : UserControl
    {
        private StatusMonitor Monitor { get; set; }

        public StatusMonitorController()
        {
            InitializeComponent();
            Monitor = (StatusMonitor)this.DataContext;
        }

        private void StatusCheckBox_Click(object sender, RoutedEventArgs e)
        {
            Monitor = (StatusMonitor)this.DataContext;
            if (Monitor.Toggle == true)
                Monitor.Run();
            else
                Monitor.Stop();
        }


    }
}
