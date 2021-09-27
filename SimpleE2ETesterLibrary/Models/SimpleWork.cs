using System.Threading.Tasks;
using SimpleE2ETesterLibrary.Interfaces;

namespace SimpleE2ETesterLibrary.Models
{
    public class SimpleWork<T>
    {
        public T Value { get; }
        private readonly Task<ISimpleE2ETester> _testHelper;

        public SimpleWork(T value, Task<ISimpleE2ETester> testHelper)
        {
            Value = value;
            _testHelper = testHelper;
        }

        public Task<ISimpleE2ETester> GetTestHelper()
        {
            return this._testHelper;
        }
    }
}