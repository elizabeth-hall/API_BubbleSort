using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace API_BubbleSort.Data
{
    /// <summary>
    /// Used in deserializing Json response
    /// </summary>
    public class API_JsonRequest
    {
        public API_JsonRequest() { }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "data")]
        public List<Employee> Employees { get; set; }
    }
}