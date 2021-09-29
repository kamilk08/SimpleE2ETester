using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Owin.Testing;
using SimpleE2ETesterLibrary.Extensions.Responses;
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
    public class AreAllRequestsSuccessfulTests
    {
        private readonly ISimpleE2ETester _tester;
        
        public AreAllRequestsSuccessfulTests()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            var testServer = TestServer.Create<Startup>();

            _tester = new SimpleE2ETester(new AspNetHttpClient(testServer.HttpClient));
        }

        [Fact]
        public async Task
            AreAllRequestsSuccessfulAsync_WhenCalledAndAllRequestsAreSuccess_ThenItShouldReturnTrueValue()
        {
            var result = await _tester
                .AddRequest(new HttpPostTestRequest(new HttpPostDto
                {
                    Id = 1,
                    Guid = Guid.NewGuid(),
                    Collection = new List<int>(),
                    Flag = false
                }))
                .AddRequest(new HttpPostTestRequest(new HttpPostDto
                {
                    Id = 2,
                    Guid = Guid.NewGuid(),
                    Flag = false
                }))
                .AddRequest(new HttpPostTestRequest(new HttpPostDto
                {
                    Guid = Guid.NewGuid(),
                    Id = 3,
                    Flag = false
                }))
                .SendPendingRequestsAsync(Order.Sequential)
                .AreAllRequestsSuccessfulAsync((flag) => flag.Should().BeTrue());
        }

        [Fact]
        public async Task AreAllRequestsSuccessfulAsync_WhenCalledAndOneOfTheRequestsWasNotSuccessful_ThenItShouldReturnFalseValue()
        {
            var result = await _tester
                .AddRequest(new HttpPostTestRequest(new HttpPostDto
                {
                    Id = 1,
                    Guid = Guid.NewGuid(),
                    Collection = new List<int>(),
                    Flag = false
                }))
                .AddRequest(new HttpPostTestRequest(new HttpPostDto
                {
                    Id = 2,
                    Guid = Guid.NewGuid(),
                    Flag = false
                }))
                .AddRequest(new HttpPostTestRequest(new HttpPostDto
                {
                    Guid = Guid.Empty,
                    Id = 3,
                    Flag = false
                }))
                .SendPendingRequestsAsync(Order.Sequential)
                .AreAllRequestsSuccessfulAsync((flag) => flag.Should().BeFalse());
        }
    }
}