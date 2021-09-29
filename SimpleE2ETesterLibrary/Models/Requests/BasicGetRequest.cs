using System;
using System.Net.Http;
using SimpleE2ETesterLibrary.Interfaces;

namespace SimpleE2ETesterLibrary.Models.Requests
{
    public class BasicGetRequest : ISimpleHttpRequest
    {
        private readonly string _uri;

        public BasicGetRequest(string uri)
        {
            _uri = uri;
        }
        
        public HttpRequestMessage ToHttpRequest()
        {
            return new HttpRequestMessage(HttpMethod.Get, new Uri(_uri));
        }
    }
}