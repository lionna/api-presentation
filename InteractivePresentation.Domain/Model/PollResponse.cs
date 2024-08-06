using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace InteractivePresentation.Domain.Model
{
    public class PollResponse
    {
        [JsonPropertyName("poll_id")]
        public Guid Id { get; set; }
        [JsonPropertyName("question")]
        public string Question { get; set; }
        [JsonPropertyName("presentation_id")]
        public Guid PresentationId { get; set; }
        [JsonPropertyName("options")]
        public List<OptionResponse> Options { get; set; }
    }
}
