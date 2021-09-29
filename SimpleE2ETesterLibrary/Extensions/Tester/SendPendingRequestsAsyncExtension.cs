using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SimpleE2ETesterLibrary.Interfaces;
using SimpleE2ETesterLibrary.Models;
using SimpleE2ETesterLibrary.Models.Requests;

namespace SimpleE2ETesterLibrary.Extensions.Tester
{
    public static class SendPendingRequestsAsyncExtension
    {
        public static async Task<ISimpleE2ETester> SendPendingRequestsAsync(this ISimpleE2ETester tester,
            Order order)
        {
            var helper = (SimpleE2ETester) tester;

            if (order == Order.Sequential)
                helper = await SendRequestsInOrderAsync(helper) as SimpleE2ETester;
            else
                helper = await SendRequestsInRandomOrderAsync(helper) as SimpleE2ETester;

            helper.ClearPendingRequests();

            return helper;
        }


        public static async Task<ISimpleE2ETester> SendPendingRequestsAsync(this Task<ISimpleE2ETester> testerTask,
            Order order)
        {
            var tester = await testerTask;

            return await SendPendingRequestsAsync(tester, order);
        }

        public static async Task<ISimpleE2ETester> SendPendingRequestsAsync(this ISimpleE2ETester tester)
        {
            return await SendPendingRequestsAsync(tester, Order.Sequential);
        }

        public static async Task<ISimpleE2ETester> SendPendingRequestsAsync(this Task<ISimpleE2ETester> testerTask)
        {
            var tester = await testerTask;

            return await SendPendingRequestsAsync(tester, Order.Sequential);
        }

        public static async Task<ISimpleE2ETester> SendPendingRequestsWithThrottleAsync(this ISimpleE2ETester tester,
            ThrottleOptions options)
        {
            var helper = (SimpleE2ETester) tester;

            helper = await SendPendingRequestsWithThrottler(helper, options) as SimpleE2ETester;

            helper.ClearPendingRequests();

            return helper;
        }

        public static async Task<ISimpleE2ETester> SendPendingRequestsWithThrottleAsync(
            this Task<ISimpleE2ETester> testerTask, ThrottleOptions options)
        {
            var tester = await testerTask;

            return await SendPendingRequestsWithThrottler(tester, options);
        }

        public static async Task<ISimpleE2ETester> SendPendingRequestsWithThrottleAsync(
            this SimpleE2ETester tester)
        {
            return await SendPendingRequestsWithThrottler(tester, ThrottleOptions.Default());
        }

        public static async Task<ISimpleE2ETester> SendPendingRequestsWithThrottleAsync(
            this Task<ISimpleE2ETester> testerTask)
        {
            var tester = await testerTask;

            return await SendPendingRequestsWithThrottler(tester, ThrottleOptions.Default());
        }

        private static async Task<ISimpleE2ETester> SendRequestsInOrderAsync(SimpleE2ETester tester)
        {
            var pendingRequests = tester.GetPendingRequests().ToList();

            for (int i = 0; i < pendingRequests.Count(); i++)
            {
                var pendingRequest = pendingRequests[i];

                var result = await tester.SendAsyncAndMapToResult(pendingRequest.SimpleHttpRequest);
            }

            return tester;
        }

        private static async Task<ISimpleE2ETester> SendRequestsInRandomOrderAsync(ISimpleE2ETester tester)
        {
            var pendingRequests = tester.GetPendingRequests().ToList();

            var tasks = pendingRequests.Select(s => Task.Run(async () =>
            {
                await tester.SendAsyncAndMapToResult(s.SimpleHttpRequest);
            })).ToList();

            await Task.WhenAll(tasks);

            return tester;
        }

        private static async Task<ISimpleE2ETester> SendPendingRequestsWithThrottler(ISimpleE2ETester tester,
            ThrottleOptions options)
        {
            if (options == null) options = ThrottleOptions.Default();

            var throttler = new SemaphoreSlim(options.InitialCount, options.MaxCount);

            var pendingRequests = tester.GetPendingRequests().ToList();

            var tasks = pendingRequests.Select(s => Task.Run(async () =>
            {
                await throttler.WaitAsync();

                try
                {
                    await tester.SendAsyncAndMapToResult(s.SimpleHttpRequest);
                }
                finally
                {
                    throttler.Release();
                }
            }));

            await Task.WhenAll(tasks);

            return tester;
        }
    }
}