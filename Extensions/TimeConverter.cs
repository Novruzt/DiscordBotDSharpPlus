using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIrstDiscordBotC_.Extensions
{
    public static class TimeConverter
    {
        public static bool TryParse(string input, out TimeSpan result)
        {
            result = TimeSpan.Zero;

            int hoursIndex = input.IndexOf("h");
            int minutesIndex = input.IndexOf("m");
            int secondsIndex = input.IndexOf("s");

            if (hoursIndex != -1)
            {
                if (int.TryParse(input.Substring(0, hoursIndex), out int hours))
                {
                    result = TimeSpan.FromHours(hours);
                }
            }

            if (minutesIndex != -1)
            {
                if (int.TryParse(
                    input.Substring(hoursIndex + 1, (minutesIndex - hoursIndex - 1 == -1) ? input.Length - hoursIndex - 1 : minutesIndex - hoursIndex - 1),
                    out int minutes))
                {
                    result = result.Add(TimeSpan.FromMinutes(minutes));
                }
            }

            if (secondsIndex != -1)
            {
                if (int.TryParse(
                    input.Substring(minutesIndex + 1, (secondsIndex - minutesIndex - 1 == -1) ? input.Length - minutesIndex - 1 : secondsIndex - minutesIndex - 1),
                    out int seconds))
                {
                    result = result.Add(TimeSpan.FromSeconds(seconds));
                }
            }

            return hoursIndex != -1 || minutesIndex != -1 || secondsIndex != -1;
        }
    
    }
}
