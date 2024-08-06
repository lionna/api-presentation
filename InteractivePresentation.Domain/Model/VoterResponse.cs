using System;
using System.Text.Json.Serialization;

namespace InteractivePresentation.Domain.Model
{
    public class VoterResponse
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }
        [JsonPropertyName("client_id")]
        public Guid ClientId { get; set; }
    }
}
