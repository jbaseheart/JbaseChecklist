using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace JbaseChecklist.Domain.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public List<Checklist> CheckLists { get; set; }
    }
}
