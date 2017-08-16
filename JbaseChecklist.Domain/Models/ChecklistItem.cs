using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JbaseChecklist.Domain.Models
{
    public class ChecklistItem
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsComplete { get; set; }

    }
}
