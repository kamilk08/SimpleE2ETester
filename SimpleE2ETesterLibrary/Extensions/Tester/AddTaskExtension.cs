using System;
using System.Threading.Tasks;
using SimpleE2ETesterLibrary.Interfaces;
using SimpleE2ETesterLibrary.Models;

namespace SimpleE2ETesterLibrary.Extensions.Tester
{
    public static class AddTaskExtension
    {
        public static ISimpleE2ETester AddTask(this ISimpleE2ETester testerTask, Task task)
        {
            testerTask.AddTask(task);

            return testerTask;
        }

        public static ISimpleE2ETester AddTask(this ISimpleE2ETester task, Func<Task> func)
        {
            task.AddTask(func());

            return task;
        }

        public static async Task<ISimpleE2ETester> AddTask(this Task<ISimpleE2ETester> testerTask, Task task)
        {
            var tester = await testerTask;

            tester.AddTask(task);

            return tester;
        }


        public static Task<ISimpleE2ETester> AddTask(this Task<ISimpleE2ETester> testerTask, Func<Task> func)
        {
            var tester = testerTask.AddTask(func());

            return tester;
        }
    }
}