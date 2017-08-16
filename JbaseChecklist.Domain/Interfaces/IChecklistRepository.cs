using System;
using System.Collections.Generic;
using System.Text;
using JbaseChecklist.Domain.Models;

namespace JbaseChecklist.Domain
{
    public interface IChecklistRepository
    {
        IEnumerable<ChecklistItem> GetAllChecklistItems();
        ChecklistItem GetCheckListItemById(int id);
        ChecklistItem CreateCheckListItem(ChecklistItem item);
        ChecklistItem UpdateCheckListItem(ChecklistItem item);
        void Delete(ChecklistItem item);
    }
}
