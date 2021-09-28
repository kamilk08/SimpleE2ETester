using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
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
    public class SendAsyncAndMapToResultTests
    {
        private readonly ISimpleE2ETester _tester;

        public SendAsyncAndMapToResultTests()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            
            var testServer = TestServer.Create<Startup>();

            _tester = new SimpleE2ETester(new AspNetHttpClient(testServer.HttpClient));
        }
        
        
        [Fact]
        public async Task SendAsyncAndMapToResult_WhenCalledAndRequestIsValid_ThenResponseShouldHave200StatusCode()
        {
            var response = await _tester.SendAsyncAndMapToResult(new HttpPostTestRequest(new HttpPostDto
            {
                Collection = new List<int>(),
                Flag = false,
                Guid = Guid.NewGuid(),
                Id = 1
            }));

            response.IsOk().Should().BeTrue();
        }
        
        [Fact]
        public async Task SendAsyncAndMapToResult_WhenCalledAndRequestIsNull_ThenResponseShouldHave500StatusCode()
        {
            var response = await _tester.SendAsyncAndMapToResult(new HttpPostTestRequest(new HttpPostDto
            {
                Flag = true
            }));

            response.IsServerError().Should().BeTrue();
        }
        
        [Fact]
        public async Task SendAsyncAndMapToResult_WhenCalledAndRequestIsInvalid_ThenResponseShouldHave400StatusCode()
        {
            var response = await _tester.SendAsyncAndMapToResult(new HttpPostTestRequest(new HttpPostDto
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