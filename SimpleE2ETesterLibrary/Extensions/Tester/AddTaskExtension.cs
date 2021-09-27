using System;
using System.Threading.Tasks;
using SimpleE2ETesterLibrary.Interfaces;
using SimpleE2ETesterLibrary.Models;

namespace SimpleE2ETesterLibrary.Extensions.Tester
{
    public static class AddTaskExtension
    {
        
        public static ISimpleE2ETester AddTask(this ISimpleE2ETester tester, Func<Task> func)
        {
            tester.AddTask(func());
    
            return tester;
        }

        public static Task<ISimpleE2ETester> AddTask(this Task<ISimpleE2ETester> testerTask, Func<Task> func)
        {
            var tester = testerTask.GetAwaiter().GetResult();

            tester.AddTask(func());

            return Task.FromResult(tester);
        }
        
        public static async Task<ISimpleE2ETester> AddTaskAsync(this Task<ISimpleE2ETester> testerTask, Task task)
        {
            var tester = await testerTask;
            
            tester.AddTask(task);
    
            return tester;
        }
    
        public static Task<ISimpleE2ETester> AddTaskAsync(this Task<ISimpleE2ETester> testerTask, Func<Task> func)
        {
            var tester = testerTask.AddTaskAsync(func());
    
            return tester;
        }
    }
}