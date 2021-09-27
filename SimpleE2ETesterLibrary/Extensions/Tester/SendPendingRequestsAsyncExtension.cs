using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleE2ETesterLibrary.Interfaces;
using SimpleE2ETesterLibrary.Models;

namespace SimpleE2ETesterLibrary.Extensions.Tester
{
    public static class SendPendingRequestsAsyncExtension
    {
        public static async Task<ISimpleE2ETester> SendPendingRequestsAsync(this ISimpleE2ETester tester,
            Order order = Order.InOrder)
        {
            var pendingRequests = tester.GetPendingRequests().ToList();

            var helper = (SimpleE2ETester) tester;

            if (order == Order.InOrder)
                await SendRequestsInOrderAsync(pendingRequests, helper);
            else
                await SendRequestsInRandomOrderAsync(pendingRequests, helper);

            helper.PendingRequests.Clear();

            return helper;
        }


        public static async Task<ISimpleE2ETester> SendPendingRequestsAsync(this Task<ISimpleE2ETester> testerTask,Order order = Order.InOrder)
        {
            var tester = await testerTask;
        
            await tester.SendPendingRequestsAsync(order);
        
            return tester;
        }

        private static async Task SendRequestsInOrderAsync(List<PendingRequest> pendingRequests, SimpleE2ETester helper)
        {
            for (int i = 0; i < pendingRequests.Count(); i++)
            {
                var pendingRequest = pendingRequests[i];

                var result = await helper.SendAsyncAndMapToResult(pendingRequest.SimpleHttpRequest);
            }
        }

        private static async Task SendRequestsInRandomOrderAsync(List<PendingRequest> pendingRequests,
            ISimpleE2ETester helper)
        {
            var tasks = pendingRequests.Select(s => Task.Run(async () =>
            {
                var response = await helper.SendAsyncAndMapToResult(s.SimpleHttpRequest);

            })).ToList();

            await Task.WhenAll(tasks);
        }
    }
}