using System.Net;
using System.Net.Http;

namespace SimpleE2ETesterLibrary.Models.Responses
{
    public class HttpResponse
    {
        public object RawResponse { get; }
        public HttpStatusCode StatusCode { get; }
        public HttpContent HttpContent { get; }

        public HttpResponse(HttpStatusCode statusCode, HttpContent httpContent,object rawResponse)
        {
            StatusCode = statusCode;
            HttpContent = httpContent;
            RawResponse = rawResponse;
        }
    }
}