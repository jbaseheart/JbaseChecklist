using System;
using System.Collections.Generic;
using System.Text;
using JbaseChecklist.Domain.Models;
using System.Threading.Tasks;

namespace JbaseChecklist.Domain
{
    public interface IChecklistRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByUserNameAsync(string userName);
        Task<User> GetUserByUserIdAsync(int userId);
        Task<User> CreateUserAsync(User user);        

        Task<IEnumerable<Checklist>> GetAllChecklistsByUserNameAsync(string userName);
        Task<Checklist> GetChecklistByIdAsync(int checklistId);
        Task<Checklist> CreateCheckListAsync(Checklist checklist);
        Task<Checklist> UpdateCheckListAsync(Checklist checklist);
        Task DeleteChecklistAsync(Checklist checklist);


        Task<IEnumerable<ChecklistItem>> GetAllChecklistItemsByChecklistIdAsync(int checklistId);
        Task<ChecklistItem> GetCheckListItemByIdAsync(int id);
        Task<ChecklistItem> CreateCheckListItemAsync(ChecklistItem item);
        Task<ChecklistItem> UpdateCheckListItemAsync(ChecklistItem item);
        Task DeleteChecklistItemAsync(ChecklistItem item);
        Task DeleteChecklistItemsAsync(IEnumerable<ChecklistItem> items);
    }
}
