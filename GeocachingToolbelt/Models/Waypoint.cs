using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeocachingToolbelt.Models
{
    public class Waypoint
    {
        [Key]
        public int Id { get; set; }

        public int MultiId { get; set; }

        [ForeignKey(nameof(MultiId))]
        public virtual Multi Multi { get; set; }

        public int Number { get; set; }

        public string Coordinate { get; set; }

    }
}
