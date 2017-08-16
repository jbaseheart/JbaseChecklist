using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using JbaseChecklist.Domain.Models;

namespace JbaseChecklist.Data.Contexts
{
    public class InMemoryChecklistContext : DbContext, IChecklistContext
    {
        public InMemoryChecklistContext(DbContextOptions<InMemoryChecklistContext> options) 
            : base(options)
        { }

        public DbSet<ChecklistItem> CheckListItems { get; set; }
    }
}
