using System;
using System.Net;
using System.Threading.Tasks;
using SimpleE2ETesterLibrary.Models;
using SimpleE2ETesterLibrary.Models.Responses;

namespace SimpleE2ETesterLibrary.Interfaces
{
    public interface ISimpleHttpResponseResult
    {
        HttpStatusCode StatusCode { get; }
        HttpResponse HttpResponse { get; }

        bool IsOk();
        bool IsAccepted();
        bool IsCreated();
        bool IsForbidden();
        bool IsUnAuthorized();
        bool IsBadRequest();
        bool IsConflict();
        bool IsServerError();
        
        ISimpleHttpResponseResult Tap(Action<ISimpleHttpResponseResult> action);
        Task<ISimpleHttpResponseResult> Tap(Func<ISimpleHttpResponseResult, Task> func);
        
        ISimpleE2ETester GetTester();
        Task<T> GetResponseContentAsync<T>();
        T GetRawResponse<T>();
    }
    
}