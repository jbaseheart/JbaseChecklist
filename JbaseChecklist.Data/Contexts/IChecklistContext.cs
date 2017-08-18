using Microsoft.EntityFrameworkCore;
using JbaseChecklist.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace JbaseChecklist.Data.Contexts
{
    public interface IChecklistContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));

        DbSet<User> Users { get; set; }
        DbSet<Checklist> CheckLists { get; set; }
        DbSet<ChecklistItem> CheckListItems { get; set; }
    }
}
