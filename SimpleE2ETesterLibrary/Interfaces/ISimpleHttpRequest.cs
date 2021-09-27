using System.Collections.Generic;
using System.Net.Http;

namespace SimpleE2ETesterLibrary.Interfaces
{
    public interface ISimpleHttpRequest
    {
        HttpRequestMessage ToHttpRequest();
    }
}