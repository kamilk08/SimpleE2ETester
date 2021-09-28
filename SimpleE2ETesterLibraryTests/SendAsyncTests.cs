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
    public class SendAsyncTests
    {
        private readonly ISimpleE2ETester _tester;

        public SendAsyncTests()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            
            var testServer = TestServer.Create<Startup>();

            _tester = new SimpleE2ETester(new AspNetHttpClient(testServer.HttpClient));
            
            _tester.ClearCompletedRequests();
            _tester.ClearPendingRequests();
        }
        
        [Fact]
        public async Task
            SendAsync_WhenCalledAndRequestIsAOkRequest_ThenRequestShouldBeCompletedAndHaveAppropriateResponse()
        {
            var dto = new HttpPostDto
            {
                Guid = Guid.NewGuid(),
                Id = 1,
                Collection = new List<int>(Enumerable.Repeat<int>(1, 2))
            };

            var request = new HttpPostTestRequest(dto);

            await _tester.SendAsync(request);

            var requests = _tester.GetCompletedRequests();

            requests.First().ResponseResult.IsOk().Should().BeTrue();
        }

        [Fact]
        public async Task
            SendAsync_WhenCalledAndRequestIsABadRequest_ThenRequestShouldBeCompletedAndHaveAppropriateResponse()
        {
            var dto = new HttpPostDto
            {
                Guid = Guid.Empty,
                Id = 1,
                Collection = new List<int>(Enumerable.Repeat<int>(1, 2))
            };

            var request = new HttpPostTestRequest(dto);

            await _tester.SendAsync(request);

            var requests = _tester.GetCompletedRequests();

            requests.First().ResponseResult.IsBadRequest().Should().BeTrue();
        }

        [Fact]
        public async Task
            SendAsync_WhenCalledAndServerErrorHasHadHappen_ThenRequestShouldBeCompletedAndHaveAppropriateResponse()
        {
            var request = new HttpPostTestRequest(new HttpPostDto
            {
                Flag = true
            });

            await _tester.SendAsync(request);

            var requests = _tester.GetCompletedRequests();

            requests.First().ResponseResult.IsServerError().Should().BeTrue();
        }

        [Theory]
        [InlineData(5)]
        [InlineData(50)]
        public async Task
            SendAsync_WhenCalledMultipleTimesAndAllRequestsAreOkRequests_ThenEachCompletedRequestShouldHaveAppropriateResponse(
                int count)
        {
            for (int i = 0; i < count; i++)
            {
                var postRequest = new HttpPostTestRequest(new HttpPostDto
                {
                    Collection = new List<int>(),
                    Guid = Guid.NewGuid(),
                    Id = i
                });

                await _tester.SendAsync(postRequest);
            }

            var completedRequests = _tester.GetCompletedRequests();

            foreach (var completedRequest in completedRequests)
                completedRequest.ResponseResult.IsOk().Should().BeTrue();
        }

        [Theory]
        [InlineData(5)]
        [InlineData(50)]
        [InlineData(100)]
        public async Task
            SendAsync_WhenCalledMultipleTimesAndOneOfTheRequestsIsABadRequest_ThenAllCompletedRequestsShouldHaveAppropriateResponses(
                int count)
        {
            for (int i = 0; i < count - 1; i++)
            {
                var postRequest = new HttpPostTestRequest(new HttpPostDto
                {
                    Collection = new List<int>(),
                    Guid = Guid.NewGuid(),
                    Id = i
                });

                await _tester.SendAsync(postRequest);
            }

            var badPostRequest = new HttpPostTestRequest(new HttpPostDto
            {
                Collection = new List<int>(),
                Guid = Guid.Empty,
                Id = 1
            });

            await _tester.SendAsync(badPostRequest);

            var completedRequests = _tester.GetCompletedRequests();
            var requestsWithOkResponses = completedRequests.Take(completedRequests.Count() - 1).ToList();

            foreach (var completedRequest in requestsWithOkResponses)
                completedRequest.ResponseResult.IsOk().Should().BeTrue();

            completedRequests.Last().ResponseResult.IsBadRequest().Should().BeTrue();
        }

        [Fact]
        public async Task
            SendAsync_WhenCalledMultipleTimesInFluentWay_ThenAllCompletedRequestsShouldHaveAppropriateResponses()
        {
            var badPostRequest = new HttpPostTestRequest(new HttpPostDto
            {
                Collection = new List<int>(),
                Guid = Guid.Empty,
                Id = 1
            });

            await _tester
                .SendAsync(badPostRequest)
                .SendAsync(badPostRequest)
                .SendAsync(badPostRequest)
                .SendAsync(badPostRequest)
                .SendAsync(badPostRequest);

            var completedRequests = _tester.GetCompletedRequests();

            foreach (var completedRequest in completedRequests)
                completedRequest.ResponseResult.IsBadRequest().Should().BeTrue();
        }
    }
}