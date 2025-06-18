using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchDog
{
    public class CheckResult
    {
        public string MethodName { get; set; }
        public string Address { get; set; }
        public bool IsAlive { get; set; }
        public string ResponseTime { get; set; }
        public string ExtraMessage { get; set; }

        public override string ToString()
        {
            return $"Method: {MethodName};\nAddress: {Address};\nIs Alive: {IsAlive};\nResponse Time: {ResponseTime}\nMessage: {ExtraMessage}.";
        }
    }


}
