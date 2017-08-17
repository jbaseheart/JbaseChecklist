using JbaseChecklist.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JbaseChecklist.API.ViewModels
{
    public class ChecklistViewModel
    {
        public ChecklistViewModel() { }

        public ChecklistViewModel(Checklist checklist)
        {
            Id = checklist.Id;
            Name = checklist.Name;
            Description = checklist.Description;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }
}
