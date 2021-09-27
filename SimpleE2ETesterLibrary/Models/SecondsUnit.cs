namespace SimpleE2ETesterLibrary.Models
{
    public class SecondsUnit : ITimeUnit
    {
        public int Duration { get; }

        private SecondsUnit() { }

        public SecondsUnit(int duration)
        {
            Duration = duration;
        }
    }
}