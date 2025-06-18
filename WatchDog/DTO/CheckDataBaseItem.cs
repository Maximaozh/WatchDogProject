using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchDog
{
    public class CheckDataBaseItem
    {
        public int Id { get; set; }
        public string MethodName { get; set; }
        public string Address { get; set; }
        public bool IsAlive { get; set; }
        public string ResponseTime { get; set; }
        public string ExtraMessage { get; set; }
        public string Timestamp { get; set; } // Новое поле для временной метки
    }

}
