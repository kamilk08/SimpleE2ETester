namespace SimpleE2ETesterLibrary.Models
{
    public class ThrottleOptions
    {
        public int InitialCount { get; }

        public int MaxCount { get; }

        public ThrottleOptions(int initialCount, int maxCount)
        {
            InitialCount = initialCount;
            MaxCount = maxCount;
        }

        public static ThrottleOptions Default()
        {
            return new ThrottleOptions(10, 100);
        }
    }
}