using SimpleE2ETesterLibrary.Interfaces;

namespace SimpleE2ETesterLibrary.Models.Requests
{
    public class SimpleCompletedRequest
    {
        public ISimpleHttpRequest Request { get; }

        public ISimpleHttpResponseResult ResponseResult { get; }

        public SimpleCompletedRequest(ISimpleHttpRequest request, ISimpleHttpResponseResult responseResult)
        {
            Request = request;
            ResponseResult = responseResult;
        }
    }
}