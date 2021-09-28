using System;
using System.Collections.Generic;

namespace TestsApi.Models
{
    public class HttpPostDto
    {
        public Guid Guid { get; set; }

        public bool Flag { get; set; }

        public int Id { get; set; }

        public IEnumerable<int> Collection { get; set; } = new List<int>(){1};
    }
}