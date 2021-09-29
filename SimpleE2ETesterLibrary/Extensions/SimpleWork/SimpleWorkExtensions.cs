using System;
using System.Threading.Tasks;
using SimpleE2ETesterLibrary.Interfaces;
using SimpleE2ETesterLibrary.Models;
using SimpleE2ETesterLibrary.Models.Tasks;

namespace SimpleE2ETesterLibrary.Extensions.SimpleWork
{
    public static class SimpleWorkExtensions
    {
        public static Task<SimpleWork<K>> Map<K>(this Task<ISimpleE2ETester> tester, Func<K> func)
        {
            var work = new SimpleWork<K>(func(), tester);
            return Task.FromResult(work);
        }

        public static async Task<SimpleWork<K>> Map<T, K>(this Task<SimpleWork<T>> work, Func<T, K> func)
        {
            var awaitedWork = await work;

            return new SimpleWork<K>(func(awaitedWork.Value),awaitedWork.GetTestHelper());
        }

        public static async Task<SimpleWork<K>> Tap<K>(this Task<SimpleWork<K>> work, Func<K, Task> func)
        {
            var awaitedWork = await work;
            await func(awaitedWork.Value);

            return new SimpleWork<K>(awaitedWork.Value, awaitedWork.GetTestHelper());
        }
        
    }
}