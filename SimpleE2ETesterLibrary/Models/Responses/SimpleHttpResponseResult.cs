using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SimpleE2ETesterLibrary.Interfaces;

namespace SimpleE2ETesterLibrary.Models.Responses
{
    public class SimpleHttpResponseResult : BaseHttpResponseResultResult,ISimpleHttpResponseResult
    {
        public HttpResponse HttpResponse { get; }
        
        public override HttpStatusCode StatusCode => HttpResponse?.StatusCode ?? default;
        
        public HttpContent HttpContent => HttpResponse.HttpContent;

        public SimpleHttpResponseResult(ISimpleE2ETester tester,HttpResponse httpResponse) : base(tester)
        {
            HttpResponse = httpResponse;
        }

        public override async Task<T> GetResponseContentAsync<T>() => JsonConvert.DeserializeObject<T>( await HttpResponse.HttpContent.ReadAsStringAsync());
        
        public override T GetRawResponse<T>() => (T)HttpResponse.RawResponse;

        public override ISimpleHttpResponseResult Tap(Action<ISimpleHttpResponseResult> action)
        {
            action(this);
        
            return this;
        }

        public override async Task<ISimpleHttpResponseResult> Tap(Func<ISimpleHttpResponseResult, Task> func)
        {
           var task= func(this);
           await task;

           return this;
        }
    }
}