using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zwindowscore.Utils
{
    public static class Timer
    {
        private static Dictionary<string, Stopwatch> _timers = new Dictionary<string, Stopwatch>();

        public static void StartTimer(string message = null, string key = "default")
        {
            _timers[key] = Stopwatch.StartNew();
            Console.WriteLine("Start {0} ({1})", message, key);
        }

        public static void StopTimer(string message = null, string key = "default")
        {
            if(_timers.ContainsKey(key))
            {
                var _timer = _timers[key];
                _timer.Stop();
                var results = _timer.ElapsedMilliseconds;
                Console.WriteLine("{0} in {1}s ({2})", message, _timer.ElapsedMilliseconds / 1000.000f, key);
                _timers.Remove(key);
            }
            else
            {
                Console.WriteLine("{0} timer not found", key);
            }
        }
    }
}
