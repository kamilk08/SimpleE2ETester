using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using SimpleE2ETesterLibrary.Interfaces;
using TestsApi.Models;

namespace SimpleE2ETesterLibraryTests.Models
{
    public class HttpPutTestRequest : ISimpleHttpRequest
    {
        private readonly HttpPutDto _dto;

        public HttpPutTestRequest(HttpPutDto dto)
        {
            _dto = dto;
        }

        public HttpRequestMessage ToHttpRequest()
        {
            var uri = new Uri("http://localhost:5000/api/put/{dto}");

            var httpRequest = new HttpRequestMessage(HttpMethod.Put, uri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(_dto), Encoding.UTF8, "application/json")
            };

            return httpRequest;
        }
    }
}