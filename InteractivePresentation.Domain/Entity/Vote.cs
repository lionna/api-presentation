using InteractivePresentation.Domain.Entity.Abstract;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InteractivePresentation.Domain.Entity
{
    public class Vote : ICommonEntity<Guid>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ScaffoldColumn(false)]
        [Column("VoteId")]
        [JsonPropertyName("vote_id")]
        public Guid Id { get; set; }
        [JsonPropertyName("key")]
        public string Key { get; set; }
        [JsonPropertyName("client_id")]
        public Guid ClientId { get; set; }
        [JsonPropertyName("poll_id")]
        public Guid PollId { get; set; }
    }
}
