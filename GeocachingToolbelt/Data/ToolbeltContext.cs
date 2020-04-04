using System;
using GeocachingToolbelt.Models;
using Microsoft.EntityFrameworkCore;

namespace GeocachingToolbelt.Data
{
    public class ToolbeltContext : DbContext
    {
        public DbSet<Multi> Multi { get; set; }
    }
}
