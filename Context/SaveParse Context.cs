using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using StellarisSaveParser.Context.Models;

namespace StellarisSaveParser
{
    public class SaveParserContext : DbContext
    {
        public DbSet<GalacticObject> GalacticObjects { get; set; }
        public DbSet<Planet> Planets { get; set; }
        public DbSet<Pop> Pops { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<District> Districts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(
                @"Server=(localdb)\mssqllocaldb;Database=Blogging;Integrated Security=True");
            options.EnableSensitiveDataLogging();
        }
           
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            
            
        }
    }
}
