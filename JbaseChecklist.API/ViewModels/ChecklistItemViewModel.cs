using JbaseChecklist.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JbaseChecklist.API.ViewModels
{
    public class ChecklistItemViewModel
    {
        public ChecklistItemViewModel() { }
        public ChecklistItemViewModel(ChecklistItem checklistItem)
        {
            Id = checklistItem.Id;
            ChecklistId = checklistItem.ChecklistId;
            Description = checklistItem.Description;
            IsComplete = checklistItem.IsComplete;
        }
        
        public int Id { get; set; }
        public int ChecklistId { get; set; }
        public string Description { get; set; }
        public bool IsComplete { get; set; }

    }
}
