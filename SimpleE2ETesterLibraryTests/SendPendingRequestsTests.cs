using System;
using System.Collections.Generic;
using System.Linq;
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
    public class SendPendingRequestsTests
    {
        private readonly ISimpleE2ETester _tester;

        public SendPendingRequestsTests()
        {
            var testServer = TestServer.Create<Startup>();

            _tester = new SimpleE2ETester(new AspNetHttpClient(testServer.HttpClient));
        }

        [Theory]
        [InlineData(150)]
        [InlineData(50)]
        public async Task SendPendingRequests_WhenCalledAndSendOrderIsInOrder_ThenRequestShouldBeSendSequentially(
            int count)
        {
            CreateTasks(count);

            var pendingRequests = _tester.GetPendingRequests().ToList();

            await _tester.SendPendingRequestsAsync(Order.Sequential);

            var completedRequests = _tester.GetCompletedRequests().ToList();

            var httpRequestsContentsTasks = completedRequests
                .Select(s => s.ResponseResult.GetResponseContentAsync<HttpPostDto>())
                .ToList();

            var httpRequestsContents = await Task.WhenAll(httpRequestsContentsTasks);
            var httpRequestsIds = httpRequestsContents.Select(s => s.Id).ToList();
            var pendingRequestsPositions = pendingRequests.Select(s => s.Position).ToList();

            pendingRequestsPositions.SequenceEqual(httpRequestsIds).Should().BeTrue();
        }

        [Theory]
        [InlineData(150)]
        [InlineData(50)]
        public async Task SendPendingRequests_WhenCalledAndSendOrderIsInOrder_ThenRequestShouldHaveOkResponse(
            int count)
        {
            _tester.ClearPendingRequests();
            _tester.ClearCompletedRequests();

            CreateTasks(count);

            var pendingRequests = _tester.GetPendingRequests().ToList();

            await _tester.SendPendingRequestsAsync(Order.Sequential);

            var completedRequests = _tester.GetCompletedRequests().ToList();

            var results = completedRequests.Select(s => s.ResponseResult.IsOk()).ToList();

            results.All(s => s == true).Should().BeTrue();
        }

        [Theory]
        [InlineData(150)]
        [InlineData(50)]
        public async Task SendPendingRequests_WhenCalledAndSendOrderIsRandom_ThenRequestShouldBeSendWithoutOrder(
            int count)
        {
            CreateTasks(count);

            var pendingRequests = _tester.GetPendingRequests().ToList();

            await _tester.SendPendingRequestsAsync(Order.Random);
            
            var completedRequests = _tester.GetCompletedRequests().ToList();

            var requestTaskContents = completedRequests
                .Select(s => s.ResponseResult.GetResponseContentAsync<HttpPostDto>())
                .ToList();

            var httpRequestsContents = await Task.WhenAll(requestTaskContents);

            var pendingRequestsPositions = pendingRequests.Select(s => s.Position).ToList();
            var httpRequestsIds = httpRequestsContents.Select(s => s.Id).ToList();

            pendingRequestsPositions.SequenceEqual(httpRequestsIds).Should().BeFalse();
        }

        [Theory]
        [InlineData(150)]
        [InlineData(50)]
        public async Task SendPendingRequests_WhenCalledAndSendOrderIsRandom_ThenRequestShouldHaveOkResponse(int count)
        {
            CreateTasks(count);

            var pendingRequests = _tester.GetPendingRequests().ToList();

            await _tester.SendPendingRequestsAsync(Order.Random);

            var completedRequests = _tester.GetCompletedRequests().ToList();

            var requestTaskContents = completedRequests
                .Where(p => p != null)
                .Select(s => s.ResponseResult.GetResponseContentAsync<HttpPostDto>()).ToList();

            var results = completedRequests.Where(p => p != null).Select(s => s.ResponseResult.IsOk()).ToList();

            results.All(s => s == true).Should().BeTrue();
        }


        private void CreateTasks(int count)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            for (int i = 1; i < count; i++)
            {
                _tester.AddRequest(new HttpPostTestRequest(new HttpPostDto
                {
                    Id = i,
                    Guid = Guid.NewGuid(),
                    Collection = new List<int>()
                }));
            }
        }
    }
}