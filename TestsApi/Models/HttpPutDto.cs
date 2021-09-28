﻿using System;

 namespace TestsApi.Models
{
    public class HttpPutDto
    {
        public Guid Guid { get; set; }
        
        public int Id { get; set; }
        
        public bool Flag { get; set; }
    }
}