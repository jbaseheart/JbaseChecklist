using JbaseChecklist.Data.Contexts;
using JbaseChecklist.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using JbaseChecklist.Domain.Models;
using System.Linq;

namespace JbaseChecklist.Data.Repositories
{
    public class EFChecklistRepository : IChecklistRepository
    {
        private readonly IChecklistContext _context;

        public EFChecklistRepository(IChecklistContext context)
        {
            _context = context;
        }

        public ChecklistItem CreateCheckListItem(ChecklistItem item)
        {
            _context.CheckListItems.Add(item);
            _context.SaveChanges();

            return item;
        }

        public ChecklistItem GetCheckListItemById(int id)
        {
            return _context.CheckListItems.FirstOrDefault(t => t.Id == id);
        }

        public IEnumerable<ChecklistItem> GetAllChecklistItems()
        {
            return _context.CheckListItems.ToList();
        }

        public ChecklistItem UpdateCheckListItem(ChecklistItem item)
        {
            throw new NotImplementedException();
        }
    }
}
