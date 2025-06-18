using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchDog
{
    public class StatusMonitor : INotifyPropertyChanged
    {
        private System.Timers.Timer RunTimer { get; set; }  

        private double interval;
        public double Interval
        {
            get => interval;
            set
            {
                if (interval != value)
                {
                    interval = value;
                    OnPropertyChanged(nameof(Interval));
                }
            }
        }

        private string? address;
        public string? Address
        {
            get => address;
            set
            {
                if (address != value)
                {
                    address = value;
                    OnPropertyChanged(nameof(Address));
                }
            }
        }

        public CheckBase? CheckMethod { get; set; }

        private bool toggle;
        public bool Toggle
        {
            get => toggle;
            set
            {
                if (toggle != value)
                {
                    toggle = value;
                    OnPropertyChanged(nameof(Toggle));
                }
            }
        }

        private bool status;
        public bool Status
        {
            get => status;
            set
            {
                if (status != value)
                {
                    status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }
        public CheckRepository Repository { get; set; }
        public NotifyBot Notifier { get; set; } 

        public StatusMonitor()
        {
            RunTimer = new System.Timers.Timer();

            Address = null;
            CheckMethod = null;

            Toggle = false;
        }

        public int Run()
        {
            if (interval < 0) 
                Interval = 10000;
            Toggle = true;
            if (CheckMethod == null)
                return -1;

            if (Address == null)
                return -2;

            if (RunTimer.Enabled)
                return -3;

            if(Repository == null)
                return -4;

            RunTimer = new System.Timers.Timer();
            RunTimer.Interval = Interval;

            Toggle = true;

            RunTimer.Elapsed += async (sender, e) => await Check();

            RunTimer.AutoReset = true; 
            RunTimer.Enabled = true;

            return 0;
        }

        public async Task<int> Check()
        {
            if (CheckMethod == null)
                return -1;

            var result = await CheckMethod.CheckAsync(Address);
            Repository.SaveAsync(result);

            if (result.IsAlive == false && Notifier != null)
            {
                await Notifier.SendMessageAsync("Ошибка получения доступа. Детали сообщения:\n" + result);
                Status = false;
            }
            else
            {
                Status = true;
            }

            return 0;
        }

        public int Stop()
        {
            RunTimer.Close();
            Toggle = false;

            return 0;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
