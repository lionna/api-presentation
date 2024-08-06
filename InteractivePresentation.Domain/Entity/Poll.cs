using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using InteractivePresentation.Domain.Entity.Abstract;
using System.Text.Json.Serialization;

namespace InteractivePresentation.Domain.Entity
{
    public class Poll: ICommonEntity<Guid>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ScaffoldColumn(false)]
        [Column("PollId")]
        [JsonPropertyName("poll_id")]
        public Guid Id { get; set; }
        public string Question { get; set; }
        public List<Option> Options { get; set; }
        [JsonPropertyName("presentation_id")]
        public Guid PresentationId { get; set; }
        public bool IsCurrent { get; set; } = true;
    }
}
