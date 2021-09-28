﻿using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using SimpleE2ETesterLibrary.Interfaces;
using SimpleE2ETesterLibraryTests.Helpers;
using TestsApi.Models;

namespace SimpleE2ETesterLibraryTests.Models
{
    public class HttpDeleteTestRequest : ISimpleHttpRequest
    {
        private readonly HttpDeleteDto _dto;

        public HttpDeleteTestRequest(HttpDeleteDto dto)
        {
            _dto = dto;
        }

        public HttpRequestMessage ToHttpRequest()
        {
            var uri = new Uri(UrlHelper.Delete());

            var httpRequest = new HttpRequestMessage(HttpMethod.Delete, uri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(_dto), Encoding.UTF8, "apllication/json")
            };

            return httpRequest;
        }
    }
}