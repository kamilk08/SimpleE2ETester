using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleE2ETesterLibrary.Interfaces;
using SimpleE2ETesterLibrary.Models;

namespace SimpleE2ETesterLibrary.Extensions.Tester
{
    public static class SendMultipleSynchronouslyExtension
    {
        public static IEnumerable<ISimpleHttpResponseResult> SendMultipleSynchronously(this ISimpleE2ETester tester,
            params ISimpleHttpRequest[] requests)
        {
            var lst = new List<ISimpleHttpResponseResult>();
            
            foreach (var httpRequest in requests)
            {
                var httpResponse = tester.Client.SendAsync(httpRequest).GetAwaiter().GetResult();

                tester.AddCompletedRequest(httpRequest, httpResponse);

                lst.Add(new SimpleHttpResponseResult(tester, httpResponse));
            }

            return lst;
        }
        
        public static Task<IEnumerable<ISimpleHttpResponseResult>> SendMultipleSynchronously(this
            Task<ISimpleE2ETester> testerTask, params ISimpleHttpRequest[] requests)
        {
            var tester = testerTask.GetAwaiter().GetResult();
            var lst = new List<ISimpleHttpResponseResult>();

            foreach (var httpRequest in requests)
            {
                var httpResponse = tester.Client.SendAsync(httpRequest).GetAwaiter().GetResult();

                tester.AddCompletedRequest(httpRequest, httpResponse);

                lst.Add(new SimpleHttpResponseResult(tester, httpResponse));
            }

            return Task.FromResult(lst.AsEnumerable());
        }
    }
}