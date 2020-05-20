using System;
using System.Collections.Generic;
using System.Text;

namespace CSC1451_TaskWorker.Time
{
    public static class CustomTimeParser
    {

        public static TimeSpan ParseEndTime(this string time)
        {

            var hours = $"{time[0]}{time[1]}";
            var minutes = $"{time[3]}{time[4]}";

            var result = new TimeSpan(int.Parse(hours), int.Parse(minutes), 0);

            return result;
        }

    }
}
