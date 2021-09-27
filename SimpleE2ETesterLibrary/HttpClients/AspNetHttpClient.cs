using System;
using System.Net.Http;
using System.Threading.Tasks;
using SimpleE2ETesterLibrary.Interfaces;
using SimpleE2ETesterLibrary.Models;

namespace SimpleE2ETesterLibrary.HttpClients
{
    public class AspNetHttpClient : IHttpClient
    {
        private readonly HttpClient _httpClient;

        public AspNetHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public AspNetHttpClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<HttpResponse> SendAsync(ISimpleHttpRequest request)
        {
            var httpRequestMessage = request.ToHttpRequest();
            if (httpRequestMessage == null)
                throw new InvalidOperationException($"Cannot send http request.Invalid http request message");

            var response = await _httpClient.SendAsync(httpRequestMessage);

            return new HttpResponse(response.StatusCode, response.Content, response);
        }

        public async Task<HttpResponse> GetAsync(string uri)
        {
            if (string.IsNullOrEmpty(uri) || string.IsNullOrWhiteSpace(uri))
                throw new InvalidOperationException($"Cannot send http request. Invalid uri.");

            var response = await _httpClient.GetAsync(new Uri(uri));

            return new HttpResponse(response.StatusCode, response.Content, response);
        }

        public async Task<HttpResponse> GetAsync(ISimpleHttpRequest request)
        {
            var httpRequestMessage = request.ToHttpRequest();

            if (httpRequestMessage == null)
                throw new InvalidOperationException($"Cannot send http request.Invalid http request message");

            var response = await _httpClient.GetAsync(httpRequestMessage.RequestUri);

            return new HttpResponse(response.StatusCode, response.Content, response);
        }
    }
}