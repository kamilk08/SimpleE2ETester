using System;
using System.Threading.Tasks;
using SimpleE2ETesterLibrary.Interfaces;
using SimpleE2ETesterLibrary.Models;

namespace SimpleE2ETesterLibrary.Extensions.Tester
{
    public static class WaitSynchronouslyExtension
    {
        public static Delay Delay(this ISimpleE2ETester tester)
        {
            return new Delay(tester);
        }

        public static Task<Delay> Delay(this Task<ISimpleE2ETester> testerTask)
        {
            var tester = testerTask.GetAwaiter().GetResult();
            
            return Task.FromResult(new Delay(tester));
        }

        public static ISimpleE2ETester Seconds(this Delay delay, int seconds)
        {
            delay.SetTimeUnit(new SecondsUnit(seconds));

            Task.Delay(TimeSpan.FromSeconds(delay.TimeUnit.Duration)).GetAwaiter().GetResult();
            
            return delay.Tester;
        }

        public static Task<ISimpleE2ETester> Seconds(this Task<Delay> waitTask, int seconds)
        {
            var delay = waitTask.GetAwaiter().GetResult();

            delay.SetTimeUnit(new SecondsUnit(seconds));

            Task.Delay(TimeSpan.FromSeconds(delay.TimeUnit.Duration)).GetAwaiter().GetResult();

            return Task.FromResult(delay.Tester);
        }

        public static ISimpleE2ETester Minutes(this Delay delay, int minutes)
        {
            delay.SetTimeUnit(new MinutesUnit(minutes));
            
            Task.Delay(TimeSpan.FromMinutes(delay.TimeUnit.Duration)).GetAwaiter().GetResult();
            
            return delay.Tester;
        }

        public static Task<ISimpleE2ETester> Minutes(this Task<Delay> waitTask, int minutes)
        {
            var delay = waitTask.GetAwaiter().GetResult();

            delay.SetTimeUnit(new MinutesUnit(minutes));
            
            Task.Delay(TimeSpan.FromMinutes(delay.TimeUnit.Duration)).GetAwaiter().GetResult();

            return Task.FromResult(delay.Tester);
        }
    }
}