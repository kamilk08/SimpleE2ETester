using System.Linq;
using System.Threading.Tasks;
using SimpleE2ETesterLibrary.Interfaces;
using SimpleE2ETesterLibrary.Models;

namespace SimpleE2ETesterLibrary.Extensions.Tester
{
    public static class DoAsyncExtension
    {
        public static async Task<ISimpleE2ETester> DoAsync(this ISimpleE2ETester tester, Order order = Order.InOrder)
        {
            if (order == Order.InOrder)
                await RunPendingTasksInOrderInternal(tester);
            else
                await RunPendingTasksInternal(tester);

            return tester;
        }

        public static async Task<ISimpleE2ETester> DoAsync(this Task<ISimpleE2ETester> testerTask,
            Order order = Order.InOrder)
        {
            var tester = await testerTask;

            if (order == Order.InOrder)
                await RunPendingTasksInOrderInternal(tester);
            else
                await RunPendingTasksInternal(tester);

            return tester;
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
    }
}