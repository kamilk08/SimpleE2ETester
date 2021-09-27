using System;
using System.Threading.Tasks;
using SimpleE2ETesterLibrary.Interfaces;
using SimpleE2ETesterLibrary.Models;


namespace SimpleE2ETesterLibrary.Extensions.Tester
{
    public static class AddPendingRequestExtension
    {
        public static ISimpleE2ETester AddRequest(this ISimpleE2ETester tester, Func<ISimpleHttpRequest> func)
        {
            tester.AddRequest(func());

            return tester;
        }
        
        public static async Task<ISimpleE2ETester> AddRequestAsync(this Task<ISimpleE2ETester> testerTask,
            ISimpleHttpRequest request)
        {
            var tester = await testerTask;

            tester.AddRequest(request);

            return tester;
        }

        public static Task<ISimpleE2ETester> AddRequestAsync(this Task<ISimpleE2ETester> testerTask,
            Func<ISimpleHttpRequest> func)
        {
            var tester =  testerTask.AddRequestAsync(func());

            return tester;
        }


    }
}