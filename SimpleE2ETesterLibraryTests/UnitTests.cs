using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Owin.Testing;
using Newtonsoft.Json;
using SimpleE2ETesterLibrary.Extensions.Tester;
using SimpleE2ETesterLibrary.HttpClients;
using SimpleE2ETesterLibrary.Interfaces;
using SimpleE2ETesterLibrary.Models;
using SimpleE2ETesterLibrary.Models.Responses;
using SimpleE2ETesterLibrary.Models.Tasks;
using SimpleE2ETesterLibraryTests.Models;
using TestsApi;
using TestsApi.Models;
using Xunit;

namespace SimpleE2ETesterLibraryTests
{
    [Collection("Sequential")]
    public class UnitTests
    {
        private readonly ISimpleE2ETester _tester;

        public UnitTests()
        {
            var testServer = TestServer.Create<Startup>();
            
            _tester = new SimpleE2ETester(new AspNetHttpClient(testServer.HttpClient));
        }

        [Theory]
        [InlineData(100)]
        [InlineData(50)]
        [InlineData(25)]
        public void AddTask_WhenCalled_ItShouldAddNewPendingTask(int count)
        {
            var tasks = CreateTestTasks(count);

            foreach (var task in tasks)
                _tester.AddTask(task);

            var pendingTasks = _tester.GetPendingTasks().ToList();

            for (int i = 0; i < count - 1; i++)
            {
                var pendingTask = pendingTasks.ElementAt(i);
                var nextPendingTask = pendingTasks.ElementAt(i + 1);
                if (nextPendingTask == null) break;

                pendingTask.Should().NotBeNull();
                nextPendingTask.Should().NotBeNull();
                pendingTask.Position.Should().Be(nextPendingTask.Position - 1);
            }
        }

        [Fact]
        public void AddTask_WhenCalledAndTaskIsNull_ThenInvalidOperationExceptionShouldBeThrown()
        {
            Action act = () => _tester.AddTask(null);

            act.Should().Throw<InvalidOperationException>();
        }

        [Theory]
        [InlineData(100)]
        [InlineData(25)]
        [InlineData(1)]
        public void AddRequest_WhenCalled_ShouldAddNewPendingRequest(int count)
        {
            var requests = CreateHttpRequests(count);

            foreach (var request in requests) _tester.AddRequest(request);

            var pendingRequests = _tester.GetPendingRequests().ToList();

            for (int i = 0; i < count - 1; i++)
            {
                var pendingRequest = pendingRequests.ElementAt(i);
                var nextPendingRequest = pendingRequests.ElementAt(i + 1);

                pendingRequest.Should().NotBeNull();
                nextPendingRequest.Should().NotBeNull();
                pendingRequest.Position.Should().Be(nextPendingRequest.Position - 1);
            }
        }

        [Fact]
        public void AddRequest_WhenCalledAndRequestIsNull_ThenInvalidOperationExceptionShouldBeThrown()
        {
            Action act = () => _tester.AddRequest(null);

            act.Should().Throw<InvalidOperationException>();
        }

        [Theory]
        [InlineData(100)]
        [InlineData(10)]
        [InlineData(1)]
        public void AddCompletedRequest_WhenCalled_ItShouldAddNewCompletedRequest(int count)
        {
            var requests = CreateHttpRequests(count);
            var responses = CreateHttpResponses(count);

            for (int i = 0; i < count; i++)
            {
                var request = requests[i];
                var response = responses[i];

                _tester.AddCompletedRequest(request, response);
            }

            var completedRequests = _tester.GetCompletedRequests();
            completedRequests.Count().Should().Be(count);
        }

        [Fact]
        public void AddCompletedRequest_WhenCalledAndRequestIsNull_ThenItShouldThrowInvalidOperationException()
        {
            Action act = () => _tester.AddCompletedRequest(null,
                new HttpResponse(HttpStatusCode.Forbidden, new StringContent(Guid.NewGuid().ToString()),
                    new HttpResponseMessage(HttpStatusCode.Accepted)));

            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void AddCompletedRequest_WhenCalledAndResponseIsNull_ThenItShouldThrowInvalidOperationException()
        {
            var queryValue = 1;

            Action act = () => _tester.AddCompletedRequest(new HttpGetTestRequest(queryValue), null);

            act.Should().Throw<InvalidOperationException>();
        }

        [Theory]
        [InlineData(50)]
        public void AddCompletedTask_WhenCalled_ItShouldAddCompletedTask(int count)
        {
            var tasks = this.CreateTestTasks(count);

            for (int i = 0; i < count; i++)
            {
                var task = tasks.ElementAt(i);
                _tester.AddCompletedTask(new CompletedTask(task, i + 1));
            }

            var completedTasks = _tester.GetCompletedTasks();
            completedTasks.Count().Should().Be(count);
        }

        [Fact]
        public void AddCompletedTask_WhenCalledAndIsNull_MethodShouldThrowInvalidOperationException()
        {
            Action act = () => _tester.AddCompletedTask(null);

            act.Should().Throw<InvalidOperationException>();
        }

        private List<Task> CreateTestTasks(int count)
        {
            var tasksList = new List<Task>();

            for (int i = 0; i < count; i++)
            {
                tasksList.Add(Task.Run(() => _tester.SendSynchronouslyAndMapToResult("http://www.github.com")));
            }

            return tasksList;
        }

        private List<ISimpleHttpRequest> CreateHttpRequests(int count)
        {
            var requests = new List<ISimpleHttpRequest>();

            for (int i = 0; i < count; i++)
            {
                requests.Add(new HttpPostTestRequest(new HttpPostDto
                {
                    Id = 1,
                    Guid = Guid.NewGuid(),
                    Collection = new List<int>(),
                    Flag = false
                }));
            }

            return requests;
        }

        private List<HttpResponse> CreateHttpResponses(int count)
        {
            var responses = new List<HttpResponse>();

            for (int i = 0; i < count; i++)
            {
                var response = new HttpResponse(HttpStatusCode.OK,
                    new StringContent(JsonConvert.SerializeObject(Guid.NewGuid().ToString()), Encoding.UTF8,
                        "application/json"), new HttpResponseMessage(HttpStatusCode.Accepted));

                responses.Add(response);
            }

            return responses;
        }
    }
}