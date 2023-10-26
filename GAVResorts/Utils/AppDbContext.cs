﻿using GAVResorts.Models;
using Microsoft.EntityFrameworkCore;

namespace GAVResorts.Utils
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                        .Property(u => u.Id)
                        .ValueGeneratedOnAdd();

            modelBuilder.Entity<Contact>()
                        .Property(c => c.Id)
                        .ValueGeneratedOnAdd();
        }
    }
}
