using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchDog
{
    public abstract class CheckBase
    {
        public string GUID { get; set; }
        public string Name { get; set; }
        public abstract Task<CheckResult> CheckAsync(string url);
    }
}
