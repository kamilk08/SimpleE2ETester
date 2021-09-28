﻿using System;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using Newtonsoft.Json;
using SimpleE2ETesterLibrary.Interfaces;
using SimpleE2ETesterLibraryTests.Helpers;
using TestsApi.Models;

namespace SimpleE2ETesterLibraryTests.Models
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
            var uri = new Uri(UrlHelper.Post());
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(_dto), Encoding.UTF8, "application/json")
            };

            return httpRequest;
        }
    }
}