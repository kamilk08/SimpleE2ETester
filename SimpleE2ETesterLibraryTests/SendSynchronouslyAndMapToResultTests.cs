using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Owin.Testing;
using SimpleE2ETesterLibrary.Extensions.Tester;
using SimpleE2ETesterLibrary.HttpClients;
using SimpleE2ETesterLibrary.Interfaces;
using SimpleE2ETesterLibrary.Models;
using SimpleE2ETesterLibraryTests.Models;
using TestsApi;
using TestsApi.Models;
using Xunit;

namespace SimpleE2ETesterLibraryTests
{
    [Collection("Sequential")]
    public class SendSynchronouslyAndMapToResultTests
    {
        private readonly ISimpleE2ETester _tester;

        public SendSynchronouslyAndMapToResultTests()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            
            var testServer = TestServer.Create<Startup>();
            
            _tester = new SimpleE2ETester(new AspNetHttpClient(testServer.HttpClient));
        }
        
        [Fact]
        public void SendSynchronouslyAndMapToResult_WhenCalledAndRequestIsOk_ThenResponseShouldHave200StatusCode()
        {
            var response = _tester.SendSynchronouslyAndMapToResult(@"http://localhost:5000/api/get/1");

            response.IsOk().Should().BeTrue();
        }

        [Fact]
        public void SendSynchronously_WhenCalledAndRequestIsInvalid_ThenResponseShouldHave400StatusCode()
        {
            var response = _tester.SendSynchronouslyAndMapToResult(@"http://localhost:5000/api/get/-1");

            response.IsBadRequest().Should().BeTrue();
        }
        
        
        [Fact]
        public void SendSynchronouslyAndMapToResult_WhenCalledAndRequestIsNull_ThenResponseShouldHave500StatusCode()
        {
            var response = _tester
                .SendSynchronouslyAndMapToResult(new HttpPostTestRequest(new HttpPostDto
                {
                    Flag = true
                }));

            response.IsServerError().Should().BeTrue();
        }
        
        [Fact] 
        public void SendSynchronouslyAndMapToResult_WhenCalledAndRequestIsInvalid_ThenResponseShouldHave400StatusCode()
        {
            var response = _tester.SendSynchronouslyAndMapToResult(new HttpPostTestRequest(new HttpPostDto
            {
                Collection = new List<int>(),
                Flag = false,
                Guid = Guid.Empty,
                Id = 1
            }));

            response.IsBadRequest().Should().BeTrue();
        }
    }
}