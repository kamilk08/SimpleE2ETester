using System;
using System.Threading.Tasks;
using SimpleE2ETesterLibrary.Models;
using SimpleE2ETesterLibrary.Models.Responses;

namespace SimpleE2ETesterLibrary.Interfaces
{
    public interface IHttpClient
    {
        Task<HttpResponse> SendAsync(ISimpleHttpRequest request);
        Task<HttpResponse> GetAsync(string uri);
    }
}