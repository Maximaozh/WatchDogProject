using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using WatchDog.Views;

namespace WatchDog
{


    public partial class MainWindow : Window
    {
        public ObservableCollection<StatusMonitor> Monitors { get; set; }
        private NotifyBot bot {  get; set; }
        private CheckRepository rep { get; set; }
        private MonitorLoader ml { get;set; }
        private AppConfig Config { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            Config = new AppConfig();
            Config.Load("Config.cfg");

            bot = new NotifyBot();
            bot.Token = Config.BOT_TOKEN;
            bot.ChatId = Config.CHAT_ID;

            rep = new CheckRepository();
            rep.ConnectionString = Config.CONNECTION_STRING;
            rep.CreateDatabase();

            ml = new MonitorLoader();
            ml.Repository = rep;
            ml.Notifier = bot;
            ml.CheckMethods.Add(new CheckPing());
            ml.CheckMethods.Add(new CheckHttp());

            Monitors = new ObservableCollection<StatusMonitor>(ml.Load(Config.FILE_SAVE_NAME));
            foreach (var monitor in Monitors)
                if (monitor.Toggle == true)
                    monitor.Run();
            this.DataContext = this;

        }


        private void RunAllButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var monitor in Monitors)
                    monitor.Run();
        }

        private void StopAllButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var monitor in Monitors)
                monitor.Stop();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ml.Save(Monitors.ToList<StatusMonitor>(), Config.FILE_SAVE_NAME);
            Config.Save("config.cfg");
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var statusMonitorWindow = new StatusMonitorWindow(ml.CheckMethods);
            if (statusMonitorWindow.ShowDialog() == true) 
            {
                statusMonitorWindow.NewMonitor.Repository = ml.Repository;
                statusMonitorWindow.NewMonitor.Notifier = ml.Notifier;

                Monitors.Add(statusMonitorWindow.NewMonitor); 
            }
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            var monitor = (StatusMonitor)MonitorsListView.SelectedItem;
            if (monitor == null)
                return;
            var statusMonitorWindow = new StatusMonitorWindow(ml.CheckMethods, monitor);
            statusMonitorWindow.ShowDialog();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Вы уверены, что хотите удалить выбранный монитор?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                var monitor = (StatusMonitor)MonitorsListView.SelectedItem;
                if (monitor == null)
                    return;

                monitor.Stop();
                Monitors.Remove(monitor);
            }
        }

        private void BotApiMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SingleInputWindow("Введите API для Телеграм-бота:");
            if (dialog.ShowDialog() == true)
            {
                Config.BOT_TOKEN = dialog.InputValue; // Сохранение в статическое поле
                bot.Token = Config.BOT_TOKEN;
            }
        }

        private void ChatIdMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SingleInputWindow("Введите ID чата Телеграм-бота:");
            if (dialog.ShowDialog() == true)
            {
                Config.CHAT_ID = dialog.InputValue; // Сохранение в статическое поле
                bot.ChatId = Config.CONNECTION_STRING;
            }
        }

        private void ConnectionStringMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SingleInputWindow("Введите строку подключения к БД:");
            if (dialog.ShowDialog() == true)
            {
                Config.CONNECTION_STRING = dialog.InputValue; // Сохранение в статическое поле
                rep.ConnectionString = Config.CONNECTION_STRING;
            }
        }

        private void FileSaveNameMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SingleInputWindow("Введите имя файла списка мониторов");
            if (dialog.ShowDialog() == true)
            {
                Config.FILE_SAVE_NAME = dialog.InputValue; // Сохранение в статическое поле
            }
        }

        private void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ml.Save(Monitors.ToList<StatusMonitor>(), Config.FILE_SAVE_NAME);
            Config.Save("config.cfg");
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog(); 
        }

        private void ReportButton_Click(object sender, RoutedEventArgs e)
        {
            ReportWindow reportWindow = new ReportWindow(rep);
            reportWindow.ShowDialog();
        }

        private void AccessTimeButton_Click(object sender, RoutedEventArgs e)
        {
            var monitor = (StatusMonitor)MonitorsListView.SelectedItem;
            if (monitor == null)
                return;
            AccessTimeWindow acw = new AccessTimeWindow(rep,new List<StatusMonitor>(){monitor});
            acw.ShowDialog();
        }

        private void AccessTimeReportButton_Click(object sender, RoutedEventArgs e)
        {
            AccessTimeWindow acw = new AccessTimeWindow(rep, Monitors.ToList(),true);
            acw.ShowDialog();
        }
    }
}