using Microsoft.EntityFrameworkCore;
using TrackerDesktop.Data.Entities;

namespace TrackerDesktop.Data
{
    public class TrackerContext : DbContext
    {

        public DbSet<User> Users { get; set; }
        public DbSet<Mode> Modes { get; set; }
        public DbSet<Step> Steps { get; set; }

        public TrackerContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=TrackerDesktop.db3");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Mode>()
                .Property(p => p.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<Step>()
                .Property(p => p.Id)
                .ValueGeneratedNever();
                
        }
    }
}
