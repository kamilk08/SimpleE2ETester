using System;
using System.Threading.Tasks;
using SimpleE2ETesterLibrary.Interfaces;

namespace SimpleE2ETesterLibrary.Extensions.Tester
{
    public static class SendSynchronouslyExtension
    {
        public static ISimpleE2ETester SendSynchronously(this ISimpleE2ETester tester, ISimpleHttpRequest request)
        {
            var response = tester.Client.SendAsync(request).GetAwaiter().GetResult();

            tester.AddCompletedRequest(request, response);

            return tester;
        }

        public static ISimpleE2ETester SendSynchronously(this ISimpleE2ETester tester, Func<ISimpleHttpRequest> func)
        {
            tester.SendSynchronously(func());

            return tester;
        }

        public static Task<ISimpleE2ETester> SendSynchronously(this Task<ISimpleE2ETester> task,
            ISimpleHttpRequest request)
        {
            var tester = task.GetAwaiter().GetResult();

            var response = tester.Client.SendAsync(request).GetAwaiter().GetResult();

            tester.AddCompletedRequest(request, response);

            return Task.FromResult(tester);
        }

        public static Task<ISimpleE2ETester> SendSynchronously(this Task<ISimpleE2ETester> task, Func<ISimpleHttpRequest> func)
        {
            var request = func();

            var tester = task.GetAwaiter().GetResult();

            var response = tester.Client.SendAsync(request).GetAwaiter().GetResult();

            tester.AddCompletedRequest(request, response);

            return Task.FromResult(tester);
        }
    }
}