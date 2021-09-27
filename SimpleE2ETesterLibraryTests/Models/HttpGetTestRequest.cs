﻿using System;
using System.Net.Http;
using SimpleE2ETesterLibrary.Interfaces;
using SimpleE2ETesterLibrary.Models;

namespace SimpleE2ETesterNetTests.TestRequests
{
    public class HttpGetTestRequest : ISimpleHttpRequest
    {
        public HttpRequestMessage ToHttpRequest()
        {
            var uri = new Uri("http://localhost:5000/api/get/1");
            
            var httpRequest = new HttpRequestMessage(HttpMethod.Get,uri);

            return httpRequest;
        }
    }
}