using System;
using System.Threading.Tasks;
using SimpleE2ETesterLibrary.Interfaces;
using SimpleE2ETesterLibrary.Models;

namespace SimpleE2ETesterLibrary.Extensions.Tester
{
    public static class SendSynchronouslyAndMapToResultExtension
    {
        public static ISimpleHttpResponseResult SendSynchronouslyAndMapToResult(this ISimpleE2ETester tester, string uri)
        {
            ISimpleHttpResponseResult result = null;

            var request = new BasicGetRequest(uri);

            var httpResponse = tester.Client.SendAsync(request).GetAwaiter().GetResult();
            result = new SimpleHttpResponseResult(tester, httpResponse);

            tester.AddCompletedRequest(request, httpResponse);

            return result;
        }

        public static Task<ISimpleHttpResponseResult> SendSynchronouslyAndMapToResult(
            this Task<ISimpleE2ETester> task,
            string uri)
        {
            var tester = task.GetAwaiter().GetResult();

            ISimpleHttpResponseResult result = null;

            var request = new BasicGetRequest(uri);

            var httpResponse = tester.Client.SendAsync(request).GetAwaiter().GetResult();
            result = new SimpleHttpResponseResult(tester, httpResponse);

            tester.AddCompletedRequest(request, httpResponse);

            return Task.FromResult(result);
        }

        public static ISimpleHttpResponseResult SendSynchronouslyAndMapToResult(this ISimpleE2ETester tester,
            ISimpleHttpRequest request)
        {
            ISimpleHttpResponseResult result = null;

            var httpResponse = tester.Client.SendAsync(request).GetAwaiter().GetResult();

            tester.AddCompletedRequest(request, httpResponse);

            result = new SimpleHttpResponseResult(tester, httpResponse);

            return result;
        }

        public static ISimpleHttpResponseResult SendSynchronouslyAndMapToResult(this ISimpleE2ETester tester,
            Func<ISimpleHttpRequest> func)
        {
            ISimpleHttpResponseResult result = null;

            var httpResponse = tester.Client.SendAsync(func()).GetAwaiter().GetResult();

            tester.AddCompletedRequest(func(), httpResponse);

            result = new SimpleHttpResponseResult(tester, httpResponse);

            return result;
        }

        public static Task<ISimpleHttpResponseResult> SendSynchronouslyAndMapToResult(this Task<ISimpleE2ETester> task,
            Func<ISimpleHttpRequest> func)
        {
            var tester = task.GetAwaiter().GetResult();

            ISimpleHttpResponseResult result = null;

            var httpResponse = tester.Client.SendAsync(func()).GetAwaiter().GetResult();

            tester.AddCompletedRequest(func(), httpResponse);

            result = new SimpleHttpResponseResult(tester, httpResponse);

            return Task.FromResult(result);
        }

        public static Task<ISimpleHttpResponseResult> SendSynchronouslyAndMapToResult(
            this Task<ISimpleE2ETester> task,
            ISimpleHttpRequest request)
        {
            var tester = task.GetAwaiter().GetResult();

            ISimpleHttpResponseResult result = null;

            var httpResponse = tester.Client.SendAsync(request).GetAwaiter().GetResult();

            tester.AddCompletedRequest(request, httpResponse);

            result = new SimpleHttpResponseResult(tester, httpResponse);

            return Task.FromResult(result);
        }
    }
}