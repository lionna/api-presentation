using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace InteractivePresentation.Client.Models
{
    public class PresentationResponse
    {
        [JsonPropertyName("current_poll_index")]
        public int CurrentPollIndex { get; set; }
        [JsonPropertyName("polls")]
        public List<PollResponseModel> Polls { get; set; }
    }
}
