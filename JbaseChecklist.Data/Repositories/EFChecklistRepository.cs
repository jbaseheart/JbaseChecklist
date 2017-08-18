using JbaseChecklist.Data.Contexts;
using JbaseChecklist.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using JbaseChecklist.Domain.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace JbaseChecklist.Data.Repositories
{
    public class EFChecklistRepository : IChecklistRepository
    {
        private readonly IChecklistContext _context;

        public EFChecklistRepository(IChecklistContext context)
        {
            _context = context;
        }

        #region Users

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(u => u.CheckLists)
                .ToListAsync();
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            return await _context.Users
                .Include(u => u.CheckLists)
                .FirstOrDefaultAsync(u => u.Username == userName);
        }
        
        public async Task<User> GetUserByUserIdAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.CheckLists)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        #endregion

        #region Checklists
        
        public async Task<IEnumerable<Checklist>> GetAllChecklistsByUserNameAsync(string userName)
        {
            return await _context.CheckLists
                .Include(cl => cl.User)
                .Include(cl => cl.ChecklistItems)
                .Where(cl => cl.User.Username == userName)
                .ToListAsync();
        }

        public async Task<Checklist> GetChecklistByIdAsync(int checklistId)
        {
            return await _context.CheckLists
                .Include(cl => cl.User)
                .Include(cl => cl.ChecklistItems)
                .Where(cl => cl.Id == checklistId)
                .FirstOrDefaultAsync();
        }

        public async Task<Checklist> CreateCheckListAsync(Checklist checklist)
        {
            await _context.CheckLists.AddAsync(checklist);
            await _context.SaveChangesAsync();

            return checklist;
        }

        public async Task<Checklist> UpdateCheckListAsync(Checklist checklist)
        {
            _context.CheckLists.Update(checklist);
            await _context.SaveChangesAsync();

            return checklist;
        }

        public async Task DeleteChecklistAsync(Checklist checklist)
        {
            _context.CheckLists.Remove(checklist);
            await _context.SaveChangesAsync();
        }

        #endregion

        #region ChecklistItems
                
        public async Task<IEnumerable<ChecklistItem>> GetAllChecklistItemsByChecklistIdAsync(int checklistId)
        {
            return await _context.CheckListItems.Where(cli => cli.ChecklistId == checklistId).ToListAsync();
        }

        public async Task<ChecklistItem> GetCheckListItemByIdAsync(int id)
        {
            return await _context.CheckListItems
                .Include(cli => cli.Checklist)
                    .ThenInclude(cl => cl.User)
                .Where(cli => cli.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<ChecklistItem> CreateCheckListItemAsync(ChecklistItem item)
        {
            await _context.CheckListItems.AddAsync(item);
            await _context.SaveChangesAsync();

            return item;
        }

        public async Task<ChecklistItem> UpdateCheckListItemAsync(ChecklistItem item)
        {
            _context.CheckListItems.Update(item);
            await _context.SaveChangesAsync();

            return item;
        }

        public async Task DeleteChecklistItemAsync(ChecklistItem item)
        {
            _context.CheckListItems.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteChecklistItemsAsync(IEnumerable<ChecklistItem> items)
        {
            _context.CheckListItems.RemoveRange(items);
            await _context.SaveChangesAsync();
        }

        #endregion

    }
}
