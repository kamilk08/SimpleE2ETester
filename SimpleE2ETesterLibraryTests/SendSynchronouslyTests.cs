using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    public class SendSynchronouslyTests
    {
        private readonly ISimpleE2ETester _tester;

        public SendSynchronouslyTests()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            var testServer = TestServer.Create<Startup>();
            
            _tester = new SimpleE2ETester(new AspNetHttpClient(testServer.HttpClient));
        }
        
        [Fact]
        public void SendSynchronously_WhenCalledAndRequestIsValid_ShouldAddCompletedRequest()
        {
            var dto = new HttpPostDto
            {
                Guid = Guid.NewGuid(),
                Id = 1,
                Collection = new List<int>(Enumerable.Repeat<int>(1, 2))
            };

            _tester.SendSynchronously(new HttpPostTestRequest(dto));
            
            var response = _tester.GetCompletedRequests().FirstOrDefault();
            _tester.GetCompletedRequests().Count().Should().Be(1);
            response.Should().NotBeNull();
            response.ResponseResult.IsOk().Should().BeTrue();
        }

        [Fact]
        public void SendSynchronously_WhenCalledAndRequestIsNotValid_ShouldAddCompletedRequest()
        {
            var dto = new HttpPostDto
            {
                Guid = Guid.Empty,
                Id = 1,
                Collection = new List<int>(Enumerable.Repeat<int>(1, 2))
            };

            _tester.SendSynchronously(new HttpPostTestRequest(dto));
            
            var response = _tester.GetCompletedRequests().FirstOrDefault();
            _tester.GetCompletedRequests().Count().Should().Be(1);
            response.Should().NotBeNull();
            response.ResponseResult.IsBadRequest().Should().BeTrue();
        }
        
    }
}