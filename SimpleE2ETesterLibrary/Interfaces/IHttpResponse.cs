using System.Net;
using System.Net.Http;

namespace SimpleE2ETesterLibrary.Interfaces
{

    public interface IHttpResponse
    {
        object RawResponse { get; }
    
        HttpStatusCode StatusCode { get; }
        
        HttpContent HttpContent { get; }
    }
}