using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurePlease.Helpers
{
    internal class CalculationHelpers
    {

        private static int getTimeSpanInMinutes(DateTime time)
        {
            var currentTime = DateTime.Now;
            return (currentTime.Subtract(time)).Minutes;
        }

        public static int getTimeSpanInMinutes(Dictionary<string, DateTime> dict, string member)
        {
            if(dict.TryGetValue(member, out var currentTime))
            {
                return getTimeSpanInMinutes(currentTime);
            }
            else
            {
                dict[member] = new DateTime(1970, 1, 1, 0, 0, 0);
                return getTimeSpanInMinutes(dict[member]);
            }
           
        }
    }
}
