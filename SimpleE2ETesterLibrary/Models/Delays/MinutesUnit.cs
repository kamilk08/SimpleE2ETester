namespace SimpleE2ETesterLibrary.Models.Delays
{
    public class MinutesUnit : ITimeUnit
    {
        public int Duration { get; }

        private MinutesUnit() { }

        public MinutesUnit(int duration)
        {
            Duration = duration;
        }
    }
}