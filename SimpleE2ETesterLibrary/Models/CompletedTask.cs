using System.Threading.Tasks;

namespace SimpleE2ETesterLibrary.Models
{
    public class CompletedTask
    {
        public Task Task { get; }
        
        public int Position { get; }

        public CompletedTask(Task task, int position)
        {
            Task = task;
            Position = position;
        }
    }
}