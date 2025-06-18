using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace WatchDog
{
    public class MonitorData
    {
        public double Interval { get; set; }
        public string? Address { get; set; }
        public string? CheckMethodGuid { get; set; }
        public bool Toggle { get; set; }
        public bool Status { get; set; }
    }

    public class MonitorLoader
    {
        public List<CheckBase> CheckMethods { get; set; }
        public CheckRepository Repository { get; set; }
        public NotifyBot Notifier { get; set; }

        public MonitorLoader() { 
            CheckMethods = new List<CheckBase>();
        }

        public MonitorData Convert(StatusMonitor monitor)
        {
            var data = new MonitorData();
            data.Interval = monitor.Interval;
            data.Address = monitor.Address;
            data.CheckMethodGuid = monitor.CheckMethod.GUID;
            data.Toggle = monitor.Toggle;
            data.Status = monitor.Status;
            return data;
        }

        public StatusMonitor Convert(MonitorData data)
        {
            var monitor = new StatusMonitor();

            monitor.Interval = data.Interval;
            monitor.Address = data.Address;
            monitor.CheckMethod = CheckMethods.First(x => x.GUID == data.CheckMethodGuid);
            monitor.Toggle = data.Toggle;
            monitor.Status = data.Status;

            monitor.Repository = Repository;
            monitor.Notifier = Notifier;
            return monitor;
        }

        public StatusMonitor[] Load(string filename)
        {
            if (!File.Exists(filename))
            {
                return new StatusMonitor[0];
            }

            var json = File.ReadAllText(filename);
            
            var dataLoaded = JsonSerializer.Deserialize<MonitorData[]>(json);

            var monitors = new List<StatusMonitor>();
            
            foreach(var data in dataLoaded)
            {
                monitors.Add(Convert(data));
            }

            return monitors.ToArray();
        }
        public void Save(List<StatusMonitor> monitors,string FilePath)
        {
            List<MonitorData> data = new List<MonitorData>();
            foreach (var monitor in monitors)
            {
                data.Add(Convert(monitor));
            }
            var json = JsonSerializer.Serialize(data);
            File.WriteAllText(FilePath, json);
        }
    }
}
