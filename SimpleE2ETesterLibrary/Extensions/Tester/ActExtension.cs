using System;
using System.Threading.Tasks;
using SimpleE2ETesterLibrary.Interfaces;
using SimpleE2ETesterLibrary.Models;

namespace SimpleE2ETesterLibrary.Extensions.Tester
{
    public static class ActExtension
    {
        public static ISimpleE2ETester Act(this ISimpleE2ETester tester, Action action)
        {
            action();

            return tester;
        }

        public static Task<ISimpleE2ETester> Act(this Task<ISimpleE2ETester> testerTask, Action action)
        {
            var tester = testerTask.GetAwaiter().GetResult();

            action();

            return Task.FromResult(tester);
        }
    }
}