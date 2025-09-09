using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hamada.models;
using Microsoft.EntityFrameworkCore;

namespace hamada.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
            .HasKey(r => r.Id);

            modelBuilder.Entity<User>()
            .Property(r => r.Id)
            .ValueGeneratedOnAdd();

            base.OnModelCreating(modelBuilder);
        }
    }
}