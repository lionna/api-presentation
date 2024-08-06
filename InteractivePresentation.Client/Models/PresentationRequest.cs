using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace InteractivePresentation.Client.Models
{
    public class PresentationRequest
    {
        [JsonPropertyName("polls")]
        public List<PollModelRequestModel> Polls { get; set; }
    }
}
