using System;
using System.Threading.Tasks;
using SimpleE2ETesterLibrary.Interfaces;
using SimpleE2ETesterLibrary.Models;

namespace SimpleE2ETesterLibrary.Extensions.Tester
{
    public static class ActAsyncExtension
    {
        public static async Task<ISimpleE2ETester> ActAsync(this ISimpleE2ETester tester, Func<Task> func)
        {
            await func();
    
            return tester;
        }

        public static async Task<ISimpleE2ETester> ActAsync(this Task<ISimpleE2ETester> task, Func<Task> func)
        {
            await func();

            return await task;
        }
    }
}