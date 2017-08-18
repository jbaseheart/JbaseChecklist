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
        }

        /// <summary>
        /// Lists all the users
        /// </summary>
        /// <returns></returns>
        // GET api/Users
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _checklistRepo.GetAllUsersAsync()
                .ContinueWith(t => t.Result.Select(u => new UserViewModel(u)));

            return new ObjectResult(users);
        }

    }
}