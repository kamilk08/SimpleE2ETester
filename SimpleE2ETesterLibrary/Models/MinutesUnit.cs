namespace SimpleE2ETesterLibrary.Models
{
    public class MinutesUnit : ITimeUnit
    {
        public int Duration { get; }

        private MinutesUnit()
        {
            
        }
        
        public MinutesUnit(int duration)
        {
            Duration = duration;
        }
    }
}