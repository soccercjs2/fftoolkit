using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Entity;
using fftoolkit.DB.Model;
using System.Data.Entity.ModelConfiguration.Conventions;
using fftoolkit.DB.Models;

namespace fftoolkit.DB.Context
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<League> Leagues { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<UnmatchedPlayer> UnmatchedPlayers { get; set; }

        public DbSet<TeamMapping> TeamMappings { get; set; }
        public DbSet<NameMapping> NameMappings { get; set; }

        public DataContext() : base("DataContext") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
