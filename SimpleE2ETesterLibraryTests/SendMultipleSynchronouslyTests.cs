using System;
using System.Collections.Generic;
using System.Linq;
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
    public class SendMultipleSynchronouslyTests
    {
        private readonly ISimpleE2ETester _tester;

        public SendMultipleSynchronouslyTests()
        {
            var testServer = TestServer.Create<Startup>();
            
            _tester = new SimpleE2ETester(new AspNetHttpClient(testServer.HttpClient));
        }
        
        [Fact]
        public void SendMultipleSynchronously_WhenCalled_ShouldSendRequests()
        {
            var dto = new HttpPostDto
            {
                Guid = Guid.NewGuid(),
                Id = 1,
                Collection = new List<int>(Enumerable.Repeat<int>(1, 2))
            };
            
            var postRequest = new HttpPostTestRequest(dto);
            var getRequest = new HttpGetTestRequest(1);

            _tester.SendMultipleSynchronously(postRequest, getRequest);

            var completedRequests = _tester.GetCompletedRequests();
            completedRequests.Should().NotBeNull();
            completedRequests.Count().Should().Be(2);
        }

        [Fact]
        public void SendMultipleSynchronously_WhenCalledAndOneOfThemIsABadRequest_ShouldSendRequestsAndReturnAppropriateResults()
        {
            var dto = new HttpPostDto
            {
                Guid = Guid.NewGuid(),
                Id = 1,
                Collection = new List<int>(Enumerable.Repeat<int>(1, 2))
            };
            
            var postRequest = new HttpPostTestRequest(dto);
            var getRequest = new HttpGetTestRequest(-1);

            _tester.SendMultipleSynchronously(postRequest, getRequest);
            
            var completedRequests = _tester.GetCompletedRequests();
            completedRequests.Should().NotBeNull();
            completedRequests.Count().Should().Be(2);

            completedRequests.Any(a=>a.ResponseResult.IsBadRequest()).Should().BeTrue();
        }

    }
}