using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GeocachingToolbelt.Utils;

namespace GeocachingToolbelt.Models
{
    public class Multi
    {
        [Key]
        public int Id { get; set; }

        // For the URL/sharing
        public string GUID { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Waypoint> Waypoints { get; set; }

        public virtual ICollection<Variable> Variables { get; set; }

        public List<char> GetLettersFromWaypoints()
        {
            var solver = new FormulaSolver();
            var letters = new List<char>();
            foreach (var wp in Waypoints)
            {
                letters.AddRange(solver.GetLetters(wp.Coordinate));
            }

            letters.Sort();
            return letters.Distinct().ToList();
        }
    }
}
