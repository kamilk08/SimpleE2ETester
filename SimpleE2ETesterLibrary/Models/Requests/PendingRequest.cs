using SimpleE2ETesterLibrary.Interfaces;

namespace SimpleE2ETesterLibrary.Models.Requests
{
    public class PendingRequest
    {
        public ISimpleHttpRequest SimpleHttpRequest { get; }
        
        public int Position { get; }

        public PendingRequest(ISimpleHttpRequest simpleHttpRequest, int position)
        {
            SimpleHttpRequest = simpleHttpRequest;
            Position = position;
        }
    }
}