using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchDog
{
    public class CheckResultsReport
    {
        public string Address { get; set; }
        public int TotalChecks { get; set; }
        public double AvailabilityPercentage { get; set; }
    }
}
