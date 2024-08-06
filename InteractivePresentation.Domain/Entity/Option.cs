using System;
using System.ComponentModel.DataAnnotations;

namespace InteractivePresentation.Domain.Entity
{
    public class Option
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
