using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SimpleE2ETesterLibrary.Interfaces;
using SimpleE2ETesterLibrary.Models;
using SimpleE2ETesterLibrary.Models.Tasks;

namespace SimpleE2ETesterLibrary.Extensions.Tester
{
    public static class DoAsyncExtension
    {
        public static async Task<ISimpleE2ETester> DoAsync(this ISimpleE2ETester tester, Order order)
        {
            if (order == Order.Sequential)
                await RunPendingTasksInOrderInternal(tester);
            else
                await RunPendingTasksInternal(tester);

            return tester;
        }

        public static async Task<ISimpleE2ETester> DoAsync(this Task<ISimpleE2ETester> testerTask, Order order)
        {
            var tester = await testerTask;

            return await DoAsync(tester, order);
        }

        public static async Task<ISimpleE2ETester> DoAsync(this ISimpleE2ETester tester)
        {
            return await DoAsync(tester, Order.Sequential);
        }

        public static async Task<ISimpleE2ETester> DoAsync(this Task<ISimpleE2ETester> testerTask)
        {
            var tester = await testerTask;

            return await DoAsync(tester, Order.Sequential);
        }

        public static async Task<ISimpleE2ETester> DoAsyncWithThrottleAsync(ISimpleE2ETester tester,
            ThrottleOptions options)
        {
            if (options == null)
                options = ThrottleOptions.Default();

            await RunPendingTasksWithThrottler(tester, options);

            return tester;
        }

        public static async Task<ISimpleE2ETester> DoAsyncWithThrottleAsync(this Task<ISimpleE2ETester> testerTask,
            ThrottleOptions options)
        {
            var tester = await testerTask;

            return await DoAsyncWithThrottleAsync(tester, options);
        }

        private static async Task RunPendingTasksInOrderInternal(ISimpleE2ETester tester)
        {
            var pendingTasks = tester.GetPendingTasks().ToList();

            for (int i = 0; i < pendingTasks.Count; i++)
            {
                var pendingTask = pendingTasks[i];
                await pendingTask.Task;

                tester.AddCompletedTask(new CompletedTask(pendingTask.Task, pendingTask.Position));
            }
        }

        private static async Task RunPendingTasksInternal(ISimpleE2ETester tester)
        {
            var pendingTasks = tester.GetPendingTasks().Select(s => Task.Run(async () =>
            {
                await s.Task;
                tester.AddCompletedTask(new CompletedTask(s.Task, s.Position));
            }));

            await Task.WhenAll(pendingTasks);
        }

        private static async Task RunPendingTasksWithThrottler(ISimpleE2ETester tester, ThrottleOptions options)
        {
            var throttler = new SemaphoreSlim(options.InitialCount, options.MaxCount);

            var pendingTasks = tester.GetPendingTasks().Select(async s =>
            {
                await throttler.WaitAsync();

                try
                {
                    await s.Task;
                    tester.AddCompletedTask(new CompletedTask(s.Task, s.Position));
                }
                finally
                {
                    throttler.Release();
                }
            });

            await Task.WhenAll(pendingTasks);
        }
    }
}