namespace SimpleE2ETesterLibrary.Models
{
    public class ThrottleOptions
    {
        private const int DefaultInitialCount = 10;
        private const int DefaultMaxCount = 100;

        public int InitialCount { get; }

        public int MaxCount { get; }

        public ThrottleOptions(int initialCount, int maxCount)
        {
            InitialCount = initialCount;
            MaxCount = maxCount;
        }

        public static ThrottleOptions Default()
        {
            return new ThrottleOptions(DefaultInitialCount, DefaultMaxCount);
        }
    }
}