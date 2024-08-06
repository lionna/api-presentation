using System.Text.Json.Serialization;

namespace InteractivePresentation.Client.Models
{
    public class OptionModel
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}
