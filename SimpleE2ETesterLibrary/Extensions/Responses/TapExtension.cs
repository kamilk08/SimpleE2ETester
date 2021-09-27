using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleE2ETesterLibrary.Interfaces;
using SimpleE2ETesterLibrary.Models;

namespace SimpleE2ETesterLibrary.Extensions.Responses
{
    public static class TapExtension
    {
        public static async Task<ISimpleHttpResponseResult> Tap(this Task<ISimpleHttpResponseResult> task,
            Func<ISimpleHttpResponseResult, Task> func)
        {
            var awaited = await task;

            await func(awaited);

            return awaited;
        }

        public static async Task<ISimpleHttpResponseResult> Tap(this Task<ISimpleHttpResponseResult> task,
            Action<ISimpleHttpResponseResult> action)
        {
            var awaited = await task;
            action(awaited);

            return awaited;
        }

        public static async Task<IEnumerable<ISimpleHttpResponseResult>> TapMultiple(
            this Task<IEnumerable<ISimpleHttpResponseResult>> task,
            params Func<ISimpleHttpResponseResult, Task>[] enumerable)
        {
            var responses = task.GetAwaiter().GetResult().ToList();

            for (int i = 0; i < responses.Count; i++)
            {
                var func = enumerable.ElementAt(i);
                await func(responses[i]);
            }

            return responses;
        }
    }
}