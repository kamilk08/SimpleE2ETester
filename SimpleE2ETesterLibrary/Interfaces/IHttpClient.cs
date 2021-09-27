using System;
using System.Threading.Tasks;
using SimpleE2ETesterLibrary.Models;

namespace SimpleE2ETesterLibrary.Interfaces
{
    public interface IHttpClient
    {
        Task<HttpResponse> SendAsync(ISimpleHttpRequest request);
        Task<HttpResponse> GetAsync(string uri);
    }
}