using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeocachingToolbelt.Models
{
    public class Variable
    {
        [Key]
        public int Id { get; set; }

        public int MultiId { get; set; }

        [ForeignKey(nameof(MultiId))]
        public virtual Multi Multi { get; set; }

        public string Letter { get; set; }

        public string Value { get; set; }
    }
}
