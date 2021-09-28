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
    public class DoAsyncTests
    {
        private readonly ISimpleE2ETester _tester;

        public DoAsyncTests()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            var testServer = TestServer.Create<Startup>();

            _tester = new SimpleE2ETester(new AspNetHttpClient(testServer.HttpClient));
            
        }

        [Theory]
        [InlineData(200)]
        [InlineData(50)]
        [InlineData(10)]
        public async Task DoAsync_WhenCalledAndOrderIsSetToInOrder_ThenTasksShouldBeInvokedSequentially(int count)
        {
            for (int i = 0; i < count; i++)
                _tester.AddTask(() => SendRequest(i));

            await _tester
                .DoAsync(Order.InOrder);

            var pendingTasksPositions = _tester.GetPendingTasks().Select(s => s.Position).ToList();
            var completedTasksPositions = _tester.GetCompletedTasks().Select(s => s.Position).ToList();

            pendingTasksPositions.SequenceEqual(completedTasksPositions).Should().BeTrue();
        }

        [Theory]
        [InlineData(200)]
        [InlineData(50)]
        [InlineData(10)]
        public async Task DoAsync_WhenCalledAndOrderIsSetToRandom_ThenTasksShouldFinishedWithoutOrder(int count)
        {
            for (int i = 0; i < count; i++)
                _tester.AddTask(() => SendRequest(i));
            
            await _tester
                .DoAsync(Order.Random);

            var pendingTasksPositions = _tester.GetPendingTasks().Select(s => s.Position).ToList();
            var completedTasksPositions = _tester.GetCompletedTasks()
                .Where(p => p != null)
                .Select(s => s.Position).ToList();

            pendingTasksPositions.SequenceEqual(completedTasksPositions).Should().BeFalse();
        }


        private async Task SendRequest(int id)
        {
            var request = new HttpPostTestRequest(new HttpPostDto
            {
                Id = id,
                Guid = Guid.NewGuid(),
                Collection = new List<int>()
            });

            await this._tester.SendAsync(request);
        }
    }
}