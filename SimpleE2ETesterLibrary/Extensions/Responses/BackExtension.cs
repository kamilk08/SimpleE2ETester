using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleE2ETesterLibrary.Interfaces;
using SimpleE2ETesterLibrary.Models;

namespace SimpleE2ETesterLibrary.Extensions.Responses
{
    public static class BackExtension
    {
        public static ISimpleE2ETester Back(ISimpleHttpResponseResult result)
        {
            return result.GetTester();
        }

        public static async Task<ISimpleE2ETester> Back(this Task<ISimpleHttpResponseResult> task)
        {
            var awaited = await task;
            var helper = awaited.GetTester();

            return helper;
        }

        public static async Task<ISimpleE2ETester> Back(this Task<IEnumerable<ISimpleHttpResponseResult>> task)
        {
            var tasks = await task;

            var tester = tasks.First().GetTester();

            return tester;
        }
    }
}