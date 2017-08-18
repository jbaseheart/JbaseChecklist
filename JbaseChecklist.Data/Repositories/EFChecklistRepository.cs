using JbaseChecklist.Data.Contexts;
using JbaseChecklist.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using JbaseChecklist.Domain.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users
                .Include(u => u.CheckLists)
                .ToList();
        }

        public User GetUserByUserName(string userName)
        {
            return _context.Users
                .Include(u => u.CheckLists)
                .FirstOrDefault(u => u.Username == userName);
        }
        
        public User GetUserByUserId(int userId)
        {
            return _context.Users
                .Include(u => u.CheckLists)
                .FirstOrDefault(u => u.Id == userId);
        }

        public User CreateUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        #endregion

        #region Checklists
        
        public IEnumerable<Checklist> GetAllChecklistsByUserName(string userName)
        {
            return _context.CheckLists
                .Include(cl => cl.User)
                .Include(cl => cl.ChecklistItems)
                .Where(cl => cl.User.Username == userName);
        }

        public Checklist GetChecklistById(int checklistId)
        {
            return _context.CheckLists
                .Include(cl => cl.User)
                .Include(cl => cl.ChecklistItems)
                .Where(cl => cl.Id == checklistId)
                .FirstOrDefault();
        }

        public Checklist CreateCheckList(Checklist checklist)
        {
            _context.CheckLists.Add(checklist);
            _context.SaveChanges();

            return checklist;
        }

        public Checklist UpdateCheckList(Checklist checklist)
        {
            _context.CheckLists.Update(checklist);
            _context.SaveChanges();

            return checklist;
        }

        public void DeleteChecklist(Checklist checklist)
        {
            _context.CheckLists.Remove(checklist);
            _context.SaveChanges();
        }

        #endregion

        #region ChecklistItems
                
        public IEnumerable<ChecklistItem> GetAllChecklistItemsByChecklistId(int checklistId)
        {
            return _context.CheckListItems.Where(cli => cli.ChecklistId == checklistId).ToList();
        }

        public ChecklistItem GetCheckListItemById(int id)
        {
            return _context.CheckListItems
                .Include(cli => cli.Checklist)
                    .ThenInclude(cl => cl.User)
                .Where(cli => cli.Id == id)
                .FirstOrDefault();
        }

        public ChecklistItem CreateCheckListItem(ChecklistItem item)
        {
            _context.CheckListItems.Add(item);
            _context.SaveChanges();

            return item;
        }

        public ChecklistItem UpdateCheckListItem(ChecklistItem item)
        {
            _context.CheckListItems.Update(item);
            _context.SaveChanges();

            return item;
        }

        public void DeleteChecklistItem(ChecklistItem item)
        {
            _context.CheckListItems.Remove(item);
            _context.SaveChanges();
        }

        public void DeleteChecklistItems(IEnumerable<ChecklistItem> items)
        {
            _context.CheckListItems.RemoveRange(items);
            _context.SaveChanges();
        }

        #endregion

    }
}
