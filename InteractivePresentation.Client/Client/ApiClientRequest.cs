using System.Collections.Generic;

namespace InteractivePresentation.Client.Client
{
    public class ApiClientRequest<T>
        where T : class
    {
        public IReadOnlyDictionary<string, string> Headers { get; set; }
        public string Path { get; set; }
        public T Query { get; set; }
    }
}
