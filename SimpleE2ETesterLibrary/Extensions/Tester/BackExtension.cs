using System.Threading.Tasks;
using SimpleE2ETesterLibrary.Interfaces;
using SimpleE2ETesterLibrary.Models;

namespace SimpleE2ETesterLibrary.Extensions.Tester
{
    public static class BackExtension
    {
        public static async Task<ISimpleE2ETester> Back<T>(this Task<SimpleWork<T>> workTask)
        {
            var tester = await workTask;

            var testerTask = tester.GetTestHelper();

            return await testerTask;
        }
    }
}