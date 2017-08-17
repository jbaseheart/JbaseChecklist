using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using JbaseChecklist.Domain.Models;


namespace JbaseChecklist.Data.Contexts
{
    public class SqlChecklistContext : DbContext, IChecklistContext
    {
        public SqlChecklistContext(DbContextOptions<SqlChecklistContext> options) 
            : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Checklist> CheckLists { get; set; }
        public DbSet<ChecklistItem> CheckListItems { get; set; }


        //This forces EF to use non-plural table names
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Checklist>().ToTable("Checklist");
            modelBuilder.Entity<ChecklistItem>().ToTable("ChecklistItem");
        }
    }
}
