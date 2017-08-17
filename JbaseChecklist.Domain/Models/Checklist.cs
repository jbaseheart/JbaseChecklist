using System;
using System.Collections.Generic;
using System.Text;

namespace JbaseChecklist.Domain.Models
{
    public class Checklist
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public User User { get; set; }
        public List<ChecklistItem> ChecklistItems { get; set; }
    }
}
