using System;
using System.Net;
using System.Threading.Tasks;
using SimpleE2ETesterLibrary.Interfaces;
using SimpleE2ETesterLibrary.Models;

namespace SimpleE2ETesterLibrary.Extensions.Responses
{
    public static class ArePrecedingRequestsSuccessfulExtension
    {
        public static ISimpleE2ETester ArePrecedingRequestsSuccessful(this ISimpleE2ETester tester,
            Action<bool> action)
        {
            var completedRequests = tester.GetCompletedRequests();

            var flag = true;

            foreach (var completedRequest in completedRequests)
            {
                if (!StatusCodes.IsSuccessfulResponse((int) completedRequest.ResponseResult.StatusCode))
                {
                    flag = false;
                    break;
                }
            }

            action(flag);

            return tester;
        }

        public static async Task<ISimpleE2ETester> AreAllRequestsSuccessfulAsync(
            this Task<ISimpleE2ETester> testerTask,
            Action<bool> action)
        {
            var tester = await testerTask;

            var completedRequests = tester.GetCompletedRequests();

            var flag = true;

            foreach (var completedRequest in completedRequests)
            {
                if (!StatusCodes.IsSuccessfulResponse((int) completedRequest.ResponseResult.StatusCode))
                {
                    flag = false;
                    break;
                }
            }

            action(flag);

            return tester;
        }
    }
}