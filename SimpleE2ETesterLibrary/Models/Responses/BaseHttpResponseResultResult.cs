using System;
using System.Net;
using System.Threading.Tasks;
using SimpleE2ETesterLibrary.Interfaces;

namespace SimpleE2ETesterLibrary.Models.Responses
{
    public abstract class BaseHttpResponseResultResult
    {
        protected readonly ISimpleE2ETester Tester;
        
        protected BaseHttpResponseResultResult(ISimpleE2ETester tester)
        {
            Tester = tester;
        }

        public abstract HttpStatusCode StatusCode { get; }

        public abstract ISimpleHttpResponseResult Tap(Action<ISimpleHttpResponseResult> action);
        public abstract Task<ISimpleHttpResponseResult> TapAsync(Func<ISimpleHttpResponseResult, Task> func);
        
        public bool IsOk() => this.StatusCode == HttpStatusCode.OK;
        public bool IsAccepted() => this.StatusCode == HttpStatusCode.Accepted;
        public bool IsCreated() => this.StatusCode == HttpStatusCode.Created;

        public bool IsForbidden() => this.StatusCode == HttpStatusCode.Forbidden;
        public bool IsUnAuthorized() => this.StatusCode == HttpStatusCode.Unauthorized;
        public bool IsBadRequest() => this.StatusCode == HttpStatusCode.BadRequest;

        public bool IsConflict() => this.StatusCode == HttpStatusCode.Conflict;
        public bool IsServerError() => this.StatusCode == HttpStatusCode.InternalServerError;
        
        public abstract Task<T> GetResponseContentAsync<T>();
        public abstract T GetRawResponse<T>();
        
        public Task<ISimpleE2ETester> GetTester() => Task.FromResult(Tester);
    }
}