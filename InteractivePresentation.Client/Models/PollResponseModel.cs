using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace InteractivePresentation.Client.Models
{
    public class PollResponseModel
    {
        [JsonPropertyName("poll_id")]
        public Guid PollId { get; set; }
        [JsonPropertyName("question")]
        public string Question { get; set; }
        public List<OptionModel> Options { get; set; }
    }
}
