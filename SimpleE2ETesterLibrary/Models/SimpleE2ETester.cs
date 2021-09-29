﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using SimpleE2ETesterLibrary.Interfaces;
 using SimpleE2ETesterLibrary.Models.Requests;
 using SimpleE2ETesterLibrary.Models.Responses;
 using SimpleE2ETesterLibrary.Models.Tasks;

 namespace SimpleE2ETesterLibrary.Models
{
    public class SimpleE2ETester : ISimpleE2ETester
    {
        internal readonly List<PendingRequest> PendingRequests;
        internal ConcurrentQueue<SimpleCompletedRequest> CompletedRequests;

        private readonly List<PendingTask> _pendingTasks;
        private ConcurrentQueue<CompletedTask> _completedTasks;
        
        public IHttpClient Client { get; }

        public SimpleE2ETester()
        {
            PendingRequests = new List<PendingRequest>();
            CompletedRequests = new ConcurrentQueue<SimpleCompletedRequest>();
            _pendingTasks = new List<PendingTask>();
            _completedTasks = new ConcurrentQueue<CompletedTask>();
        }

        public SimpleE2ETester(IHttpClient client) : this()
        {
            Client = client;
        }

        public ISimpleE2ETester AddRequest(ISimpleHttpRequest request)
        {
            if (request == null) throw new InvalidOperationException($"Request cannot be null. Invalid request");

            this.PendingRequests.Add(new PendingRequest(request, PendingRequests.Count + 1));

            return this;
        }

        public ISimpleE2ETester AddTask(Task task)
        {
            if (task == null) throw new InvalidOperationException($"Task cannot be null. Invalid operation");

            this._pendingTasks.Add(new PendingTask(task, _pendingTasks.Count + 1));

            return this;
        }

        public ISimpleE2ETester AddCompletedRequest(ISimpleHttpRequest request, HttpResponse response)
        {
            if (request == null)
                throw new InvalidOperationException("Cannot add invalid request message. Request is null.");

            if (response == null)
                throw new InvalidOperationException("Cannot add invalid response message.Response is null.");

            this.CompletedRequests.Enqueue(
                new SimpleCompletedRequest(request, new SimpleHttpResponseResult(this, response)));

            return this;
        }

        public ISimpleE2ETester AddCompletedTask(CompletedTask completedTask)
        {
            if(completedTask==null) throw new InvalidOperationException("Cannot add completed task. Task cannot be null");

            _completedTasks.Enqueue(completedTask);
            
            return this;
        }

        public ISimpleE2ETester ClearCompletedRequests()
        {
            if (this.CompletedRequests == null)
                throw new InvalidOperationException($"Cannot clear requests. Completed requests are not initialized.");

            var newBag = new ConcurrentQueue<SimpleCompletedRequest>();

            Interlocked.Exchange<ConcurrentQueue<SimpleCompletedRequest>>(ref CompletedRequests, newBag);

            return this;
        }

        public ISimpleE2ETester ClearPendingRequests()
        {
            if (this.PendingRequests == null)
                throw new InvalidOperationException(
                    $"Cannot clear pending requests. Pending requests list is not initialized.");
            
            this.PendingRequests.Clear();

            return this;
        }

        public ISimpleE2ETester ClearCompletedTasks()
        {
            if(this._completedTasks==null)
                throw new InvalidOperationException($"Cannot clear completed requests. Completed tasks list is not initialized.");
            
            var newBag = new ConcurrentQueue<CompletedTask>();

            Interlocked.Exchange<ConcurrentQueue<CompletedTask>>(ref _completedTasks, newBag);
            
            return this;
        }

        public IEnumerable<PendingTask> GetPendingTasks() => _pendingTasks.AsEnumerable();
        public IEnumerable<PendingRequest> GetPendingRequests() => PendingRequests.AsEnumerable();
        public IEnumerable<SimpleCompletedRequest> GetCompletedRequests() => CompletedRequests.AsEnumerable();
        public IEnumerable<CompletedTask> GetCompletedTasks() => _completedTasks.AsEnumerable();
    }
}