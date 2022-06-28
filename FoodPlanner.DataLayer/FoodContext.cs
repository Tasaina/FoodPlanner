using Microsoft.EntityFrameworkCore;
using System;

namespace FoodPlanner.DataLayer
{
    public class FoodContext : DbContext
    {
        public FoodContext(DbContextOptions<FoodContext> options)
            : base(options)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            SqliteDatabasePath = System.IO.Path.Join(path, "foodPlanner.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
         => options.UseSqlite($"Data Source={SqliteDatabasePath}");

        public string SqliteDatabasePath { get; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<Tag> TagLibrary { get; set; }
        public DbSet<FoodTag> TagsOnFood { get; set; }
        public DbSet<FoodPlan> FoodPlans { get; set; }
        public DbSet<FoodPlanEntry> FoodPlanEntries { get; set; }
        public DbSet<TagColor> TagColors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FoodPlan>().HasMany(fp => fp.Entries);
            modelBuilder.Entity<Food>().HasMany(f => f.Tags);
            modelBuilder.Entity<FoodTag>().HasOne(f => f.Tag);
            modelBuilder.Entity<Tag>().HasOne(t => t.Color);
        }
    }
}