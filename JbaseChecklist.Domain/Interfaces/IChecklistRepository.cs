using System;
using System.Collections.Generic;
using System.Text;
using JbaseChecklist.Domain.Models;

namespace JbaseChecklist.Domain
{
    public interface IChecklistRepository
    {
        IEnumerable<User> GetAllUsers();
        User GetUserByUserName(string userName);
        User GetUserByUserId(int userId);
        User CreateUser(User user);        

        IEnumerable<Checklist> GetAllChecklistsByUserName(string userName);
        Checklist GetChecklistById(int checklistId);
        Checklist CreateCheckList(Checklist checklist);
        Checklist UpdateCheckList(Checklist checklist);
        void DeleteChecklist(Checklist checklist);


        IEnumerable<ChecklistItem> GetAllChecklistItemsByChecklistId(int checklistId);
        ChecklistItem GetCheckListItemById(int id);
        ChecklistItem CreateCheckListItem(ChecklistItem item);
        ChecklistItem UpdateCheckListItem(ChecklistItem item);
        void DeleteChecklistItem(ChecklistItem item);
    }
}
