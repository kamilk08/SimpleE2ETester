using System;
using System.Threading.Tasks;
using SimpleE2ETesterLibrary.Interfaces;
using SimpleE2ETesterLibrary.Models;

namespace SimpleE2ETesterLibrary.Extensions.Tester
{
    public static class ActExtension
    {
        public static ISimpleE2ETester Act(this ISimpleE2ETester task, Action action)
        {
            action();

            return task;
        }

        public static async Task<ISimpleE2ETester> Act(this ISimpleE2ETester task, Func<Task> func)
        {
            await func();

            return task;
        }


        public static async Task<ISimpleE2ETester> Act(this Task<ISimpleE2ETester> task, Func<Task> func)
        {
            await func();

            return await task;
        }

        public static async Task<ISimpleE2ETester> Act(this Task<ISimpleE2ETester> task, Action action)
        {
            var tester = await task;

            action();

            return tester;
        }



    }
}