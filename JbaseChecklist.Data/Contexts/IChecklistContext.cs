using Microsoft.EntityFrameworkCore;
using JbaseChecklist.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace JbaseChecklist.Data.Contexts
{
    public interface IChecklistContext
    {
        int SaveChanges();
        DbSet<User> Users { get; set; }
        DbSet<Checklist> CheckLists { get; set; }
        DbSet<ChecklistItem> CheckListItems { get; set; }
    }
}
