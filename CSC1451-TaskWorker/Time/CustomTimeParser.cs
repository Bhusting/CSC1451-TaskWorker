using System;
using System.Collections.Generic;
using System.Text;

namespace CSC1451_TaskWorker.Time
{
    public static class CustomTimeParser
    {

        public static TimeSpan ParseEndTime(this string time)
        {
            var hours = "";
            var minutes = "";

            var minuteCheck = false;
            for (var x = 0; x < time.Length; x++)
            {
                if (time[x] == ':')
                {
                    minuteCheck = true;
                }
                else
                {
                    if (!minuteCheck)
                    {
                        hours = $"{hours}{time[x]}";
                    }
                    else
                    {

                        minutes = $"{minutes}{time[x]}";
                    }
                }

            }

            var result = new TimeSpan(int.Parse(hours), int.Parse(minutes), 0);

            return result;
        }

    }
}
