using JbaseChecklist.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JbaseChecklist.API.ViewModels
{
    public class UserViewModel
    {
        public UserViewModel() { }

        public UserViewModel(User user)
        {
            Id = user.Id;
            Username = user.Username;
        }

        public int Id { get; set; }

        public string Username { get; set; }

    }
}
