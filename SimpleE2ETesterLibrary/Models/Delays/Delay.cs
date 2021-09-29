using System;
using SimpleE2ETesterLibrary.Interfaces;

namespace SimpleE2ETesterLibrary.Models.Delays
{
    public class Delay
    {
        internal ITimeUnit TimeUnit { get; private set; }

        internal ISimpleE2ETester Tester { get; private set; }

        private Delay()
        {
        }

        internal Delay(ISimpleE2ETester tester)
        {
            Tester = tester;
        }

        internal Delay SetTimeUnit(ITimeUnit timeUnit)
        {
            TimeUnit = timeUnit ?? throw new InvalidOperationException($"Time unit cannot be null");
            TimeUnit = timeUnit.Duration <= 0 ? throw new InvalidOperationException($"Time unit has to greater then 0") : timeUnit;

            return this;
        }
    }
}