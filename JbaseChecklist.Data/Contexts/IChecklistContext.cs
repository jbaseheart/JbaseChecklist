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
        DbSet<ChecklistItem> CheckListItems { get; set; }
    }
}
