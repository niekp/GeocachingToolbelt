using System;
using GeocachingToolbelt.Models;
using Microsoft.EntityFrameworkCore;

namespace GeocachingToolbelt.Data
{
    public class ToolbeltContext : DbContext
    {
        public DbSet<Multi> Multi { get; set; }
        public DbSet<Waypoint> Waypoint { get; set; }
        public DbSet<Variable> Variable { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=toolbox.db");
    }
}
