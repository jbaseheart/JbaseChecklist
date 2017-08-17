using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using JbaseChecklist.Domain;
using JbaseChecklist.API.ViewModels;
using JbaseChecklist.Domain.Models;

namespace JbaseChecklist.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : Controller
    {
        private readonly IChecklistRepository _checklistRepo;

        public UsersController(IChecklistRepository checklistRepo)
        {
            _checklistRepo = checklistRepo;

            CreateDefaultDataIfEmpty();
        }

        private void CreateDefaultDataIfEmpty()
        {
            var allUsers = _checklistRepo.GetAllUsers();
            if (allUsers.Count() == 0)
            {
                var defaultUser = _checklistRepo.CreateUser(new User() { Username = "jbase" });

                var defaultList = _checklistRepo.CreateCheckList(new Checklist()
                {
                    Name = "Default Checklist",
                    Description = "Standard checklist of todo items",
                    UserId = defaultUser.Id
                });

                _checklistRepo.CreateCheckList(new Checklist()
                {
                    Name = "Additional Checklist",
                    Description = "Second checklist of todo items",
                    UserId = defaultUser.Id
                });

                _checklistRepo.CreateCheckListItem(new ChecklistItem
                {
                    Description = "Item1",
                    ChecklistId = 1
                });

            }

        }

        /// <summary>
        /// Lists all the users
        /// </summary>
        /// <returns></returns>
        // GET api/Users
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _checklistRepo.GetAllUsers().Select(u => new UserViewModel(u));

            return new ObjectResult(users);
        }

    }
}