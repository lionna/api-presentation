using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace InteractivePresentation.Client.Models
{
    public class PollModelRequestModel
    {
        [JsonPropertyName("question")]
        public string Question { get; set; }
        [JsonPropertyName("options")]
        public List<OptionModel> Options { get; set; }
    }
}
