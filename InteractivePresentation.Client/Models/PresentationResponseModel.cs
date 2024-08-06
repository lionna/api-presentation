using System;
using System.Text.Json.Serialization;

namespace InteractivePresentation.Client.Models
{
    public class PresentationResponseModel
    {
        [JsonPropertyName("presentation_id")]
        public Guid PresentationId { get; set; }
    }
}
