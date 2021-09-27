using System;
using System.Threading.Tasks;
using SimpleE2ETesterLibrary.Interfaces;
using SimpleE2ETesterLibrary.Models;

namespace SimpleE2ETesterLibrary.Extensions.Tester
{
    public static class SendAsyncExtension
    {
        public static async Task<ISimpleE2ETester> SendAsync(this ISimpleE2ETester tester,
            Func<ISimpleHttpRequest> func)    
        {
            var response = await tester.Client.SendAsync(func());

            tester.AddCompletedRequest(func(), response);

            return tester;
        }

        public static async Task<ISimpleE2ETester> SendAsync(this ISimpleE2ETester tester, ISimpleHttpRequest request)
        {
            var response = await tester.Client.SendAsync(request);

            tester.AddCompletedRequest(request, response);

            return tester;
        }

        public static async Task<ISimpleE2ETester> SendAsync(this Task<ISimpleE2ETester> testerTask,
            ISimpleHttpRequest request)
        {
            var tester = await testerTask;
            
            var response = await tester.Client.SendAsync(request);

            tester.AddCompletedRequest(request, response);

            return tester;
        }

        public static async Task<ISimpleE2ETester> SendAsync(this Task<ISimpleE2ETester> testerTask,
            Func<ISimpleHttpRequest> func)
        {
            var tester = await testerTask;

            var response = await tester.Client.SendAsync(func());

            tester.AddCompletedRequest(func(), response);

            return tester;
        }
    }
}