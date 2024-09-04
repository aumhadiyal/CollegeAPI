using CollegeApp.Data.Config;
using CollegeApp.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CollegeApp.Data
{
    public class CollegeDBContext : DbContext
    {
        public CollegeDBContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<Student>(new StudentConfig());
        }
    }
}
