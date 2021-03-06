﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ConstellationWebApp.Models;

namespace ConstellationWebApp.Data
{
    public class ConstellationWebAppContext : DbContext
    {
            public ConstellationWebAppContext (DbContextOptions<ConstellationWebAppContext> options)
                : base(options)
            {
            }

         public DbSet<User> User { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<UserProject> UserProjects { get; set; }
        public DbSet<ContactLink> ContactLinks { get; set; }
        public DbSet<ProjectLink> ProjectLinks { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Project>().ToTable("Project");

            modelBuilder.Entity<UserProject>()
       .HasKey(bc => new { bc.UserID, bc.ProjectID });
            modelBuilder.Entity<UserProject>()
                .HasOne(bc => bc.Project)
                .WithMany(b => b.UserProjects)
                .HasForeignKey(bc => bc.UserID);
            modelBuilder.Entity<UserProject>()
                .HasOne(bc => bc.Project)
                .WithMany(c => c.UserProjects)
                .HasForeignKey(bc => bc.ProjectID);

            modelBuilder.Entity<User>()
               .HasMany(c => c.ContactLinks)
               .WithOne(e => e.Users);

            modelBuilder.Entity<Project>()
             .HasMany(c => c.ProjectLinks)
             .WithOne(e => e.Projects);

            /*    modelBuilder.Entity<ProjecLink>()
                 .HasOne<Project>(s => s.Project)
                 .WithMany(g => g.Projects)
                 .HasForeignKey(s => s.ProjectLinkID);*/
        }
    }
    
}
