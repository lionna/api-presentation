using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace InteractivePresentation.Client.Client
{
    public class ApiClientResponse<T>
        where T : class
    {
        public string Content { get; set; }
        public T Data { get; set; }
        public IReadOnlyDictionary<string, string> Headers { get; set; }
        public HttpMethod Method { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public Uri Uri { get; set; }
    }
}
