using System;
using System.ComponentModel.DataAnnotations;

namespace GeocachingToolbelt.Models
{
    public class Multi
    {
        public Multi()
        {
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
