using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using SimpleE2ETesterLibrary.Models;

namespace SimpleE2ETesterLibrary.Interfaces
{
    public interface ISimpleE2ETester
    {
        IHttpClient Client { get; }

        ISimpleE2ETester AddRequest(ISimpleHttpRequest request);
        ISimpleE2ETester AddTask(Task task);
        ISimpleE2ETester AddCompletedRequest(ISimpleHttpRequest request, HttpResponse response);
        ISimpleE2ETester AddCompletedTask(CompletedTask task);
        
        ISimpleE2ETester ClearCompletedRequests();
        ISimpleE2ETester ClearPendingRequests();
        ISimpleE2ETester ClearCompletedTasks();
        
        IEnumerable<PendingTask> GetPendingTasks();
        IEnumerable<PendingRequest> GetPendingRequests();
        IEnumerable<SimpleCompletedRequest> GetCompletedRequests();
        IEnumerable<CompletedTask> GetCompletedTasks();
    }
}