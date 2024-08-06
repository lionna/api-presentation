using System;
using System.Text.Json.Serialization;

namespace InteractivePresentation.Domain.Model
{
    public class VoteRequest
    {
        [JsonPropertyName("client_id")]
        public Guid ClientId { get; set; }
        [JsonPropertyName("poll_id")]
        public Guid PollId { get; set; }
        [JsonPropertyName("key")]
        public string Key { get; set; }
    }
}
