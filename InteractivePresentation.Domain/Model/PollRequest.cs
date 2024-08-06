using InteractivePresentation.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InteractivePresentation.Domain.Model
{
    public class PollRequest
    {
        [JsonPropertyName("poll_id")]
        public Guid Id { get; set; }
        public string Question { get; set; }
        public List<OptionRequest> Options { get; set; }
        [JsonPropertyName("presentation_id")]
        public Guid PresentationId { get; set; }
        public bool IsCurrent { get; set; } = true;
    }

    public class OptionRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
