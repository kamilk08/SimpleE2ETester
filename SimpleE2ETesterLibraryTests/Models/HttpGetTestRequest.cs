﻿using System;
using System.Net.Http;
using SimpleE2ETesterLibrary.Interfaces;
using SimpleE2ETesterLibraryTests.Helpers;

namespace SimpleE2ETesterLibraryTests.Models
{
    public class HttpGetTestRequest : ISimpleHttpRequest
    {
        private readonly int _id;

        public HttpGetTestRequest(int id)
        {
            _id = id;
        }
        
        public HttpRequestMessage ToHttpRequest()
        {
            var uri = new Uri(UrlHelper.Get(_id));
            
            var httpRequest = new HttpRequestMessage(HttpMethod.Get,uri);

            return httpRequest;
        }
    }
}