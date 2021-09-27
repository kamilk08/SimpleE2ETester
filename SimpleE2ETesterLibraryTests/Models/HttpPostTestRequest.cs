﻿using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using SimpleE2ETesterLibrary.Interfaces;
using SimpleE2ETesterLibrary.Models;
using SimpleE2ETesterWebApi.Models;

namespace SimpleE2ETesterNetTests.TestRequests
{
    public class HttpPostTestRequest : ISimpleHttpRequest
    {
        private readonly HttpPostDto _dto;

        public HttpPostTestRequest(HttpPostDto dto)
        {
            _dto = dto;
        }

        public HttpRequestMessage ToHttpRequest()
        {
            var uri = new Uri("http://localhost:5000/api/post/{dto}");
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(_dto), Encoding.UTF8, "application/json")
            };

            return httpRequest;
        }
    }
}