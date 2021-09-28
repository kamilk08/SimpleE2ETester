using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ArePrecedingRequestsSuccessfulTests
    {
        private readonly ISimpleE2ETester _tester;

        public ArePrecedingRequestsSuccessfulTests()
        {
            var testServer = TestServer.Create<Startup>();

            _tester = new SimpleE2ETester(new AspNetHttpClient(testServer.HttpClient));
        }

        [Fact]
        public async Task ArePrecedingRequestsSuccessful_WhenCalledAndAllPrecedingRequestsAreValid_ShouldReturnTrue()
        {
            var dto = new HttpPostDto
            {
                Guid = Guid.NewGuid(),
                Id = 1,
                Collection = new List<int>(Enumerable.Repeat<int>(1, 2))
            };

            var postRequest = new HttpPostTestRequest(dto);

            await _tester
                .SendSynchronously(postRequest)
                .ArePrecedingRequestsSuccessful((flag) => flag.Should().BeTrue())
                .SendAsync(new HttpGetTestRequest(1));
        }

        [Fact]
        public async Task ArePrecedingRequestsSuccessful_WhenCalledAndSomeOfTheRequestsAreNotValid_ShouldReturnFalse()
        {
            var dto = new HttpPostDto
            {
                Guid = Guid.NewGuid(),
                Id = 1,
                Collection = new List<int>(),
                Flag = true
            };

            var postRequest = new HttpPostTestRequest(dto);

            await _tester
                .SendSynchronously(postRequest)
                .ArePrecedingRequestsSuccessful((flag) => flag.Should().BeFalse())
                .SendAsync(new HttpGetTestRequest(1));
        }
    }
}