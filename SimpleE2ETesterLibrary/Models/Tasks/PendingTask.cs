using System.Threading.Tasks;

namespace SimpleE2ETesterLibrary.Models.Tasks
{
    public class PendingTask
    {
        public Task Task { get; }
        
        public int Position { get; }

        public PendingTask(Task task, int position)
        {
            Task = task;
            Position = position;
        }
    }
}