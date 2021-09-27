using System;
using System.Threading.Tasks;
using SimpleE2ETesterLibrary.Interfaces;
using SimpleE2ETesterLibrary.Models;

namespace SimpleE2ETesterLibrary.Extensions.Tester
{
    public static class SendAsyncAndMapToResultExtension
    {
        public static async Task<ISimpleHttpResponseResult> SendAsyncAndMapToResult(this ISimpleE2ETester tester,
            ISimpleHttpRequest request)
        {
            var response = await tester.Client.SendAsync(request);

            tester.AddCompletedRequest(request, response);
            
            return new SimpleHttpResponseResult(tester, response);
        }

        public static async Task<ISimpleHttpResponseResult> SendAsyncAndMapToResult(this ISimpleE2ETester tester,
            Func<ISimpleHttpRequest> func)
        {
            var response = await tester.Client.SendAsync(func());

            tester.AddCompletedRequest(func(), response);
            
            return new SimpleHttpResponseResult(tester, response);
        }

        public static async Task<ISimpleHttpResponseResult> SendAsyncAndMapToResult(
            this Task<ISimpleE2ETester> testerTask, Func<ISimpleHttpRequest> func)
        {
            var tester = await testerTask;

            var response = await tester.Client.SendAsync(func());

            return new SimpleHttpResponseResult(tester, response);
        }

        public static async Task<ISimpleHttpResponseResult> SendAsyncAndMapToResult(
            this Task<ISimpleE2ETester> testerTask,
            ISimpleHttpRequest request)
        {
            var tester = await testerTask;

            var response = await tester.Client.SendAsync(request);

            return new SimpleHttpResponseResult(tester, response);
        }
    }
}