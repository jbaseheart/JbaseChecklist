using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using JbaseChecklist.Domain;
using JbaseChecklist.Domain.Models;
using JbaseChecklist.API.ViewModels;

namespace JbaseChecklist.API.Controllers
{
    [Route("api/[controller]")]
    public class ChecklistController : Controller
    {
        private readonly IChecklistRepository _checklistRepo;

        public ChecklistController(IChecklistRepository checklistRepo)
        {
            _checklistRepo = checklistRepo;
        }


        /// <summary>
        /// Gets all the checklists for a given user
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        // GET api/Checklist/{username}
        [HttpGet("{username}")]
        public IActionResult GetAllChecklists(string username)
        {
            var checklists = _checklistRepo.GetAllChecklistsByUserName(username)
                .Select(cl => new ChecklistViewModel(cl));

            return new ObjectResult(checklists);
        }


        /// <summary>
        /// Gets a specific checklist with the given checklistId
        /// </summary>
        /// <param name="username"></param>
        /// <param name="checklistId"></param>
        /// <returns></returns>
        // GET api/Checklist/{username}/{checklistId}
        [HttpGet("{username}/{checklistId:int}", Name = "GetChecklist")]
        public IActionResult GetChecklist(string username, int checklistId)
        {
            var checklist = _checklistRepo.GetChecklistById(checklistId);

            if (checklist == null || checklist?.User?.Username != username)
                return NotFound();

            return new ObjectResult(new ChecklistViewModel(checklist));
        }


        /// <summary>
        /// Creates a new checklist
        /// </summary>
        /// <param name="username"></param>
        /// <param name="checklist"></param>
        /// <remarks>Supplying an Id in the body is unnecessary since it will be provided after the checklist is created.
        /// A valid user is required. Otherwise, a 400 is returned
        /// </remarks>
        /// <returns></returns>
        // POST api/Checklist/{username}
        [HttpPost("{username}")]
        public IActionResult CreateChecklist(string username, [FromBody] ChecklistViewModel checklist)
        {
            var user = _checklistRepo.GetUserByUserName(username);

            if (checklist == null || user == null)
                return BadRequest();

            var newCheckList = _checklistRepo.CreateCheckList(new Checklist() {
                UserId = user.Id,
                Name = checklist.Name,
                Description = checklist.Description,                
            });

            return CreatedAtRoute("GetChecklist", new
            {
                username = username,
                checklistId = newCheckList.Id
            }, new ChecklistViewModel(newCheckList));
        }


        /// <summary>
        /// Updates an existing checklist with the given checklistId
        /// </summary>
        /// <param name="username"></param>
        /// <param name="checklistId"></param>
        /// <param name="checklist"></param>
        /// <returns></returns>
        [HttpPut("{username}/{checklistId:int}")]
        public IActionResult UpdateChecklist(string username, int checklistId, [FromBody] ChecklistViewModel checklist)
        {
            //make sure we have the required params
            if (checklist == null || checklist.Id == 0 || checklistId != checklist.Id)
            {
                return BadRequest();
            }

            //find the checklist
            var checklistToUpdate = _checklistRepo.GetChecklistById(checklist.Id);

            if (checklistToUpdate == null)
                return NotFound();

            //make sure we have access to it
            if (checklistToUpdate?.User?.Username != username)
                return Unauthorized();

            checklistToUpdate.Name = checklist.Name;
            checklistToUpdate.Description = checklist.Description;            

            _checklistRepo.UpdateCheckList(checklistToUpdate);

            return NoContent();
        }


        /// <summary>
        /// Deletes a checklist with the given checklistId
        /// </summary>
        /// <param name="username"></param>
        /// <param name="checklistId"></param>
        /// <returns></returns>
        /// <remarks>This will also delete all of the checklistItems in the list before deleting the list</remarks>
        [HttpDelete("{username}/{checklistId:int}")]
        public IActionResult DeleteChecklist(string username, int checklistId)
        {
            var checklist = _checklistRepo.GetChecklistById(checklistId);
            if (checklist == null)
                return NotFound();

            //make sure we have access to it
            if (checklist?.User?.Username != username)
                return Unauthorized();

            //delete the items in the list first
            var itemsToDelete = _checklistRepo.GetAllChecklistItemsByChecklistId(checklist.Id);
            _checklistRepo.DeleteChecklistItems(itemsToDelete);

            //delete the list
            _checklistRepo.DeleteChecklist(checklist);

            return new NoContentResult();
        }


        /// <summary>
        /// Gets all the checklistItems for a given checklistId. 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="checklistId"></param>
        /// <returns></returns>
        /// <remarks>The checklistId supplied must be for the given user. Otherwise a 404 is returned</remarks>
        // GET api/Checklist/{username}/{checklistId}/items
        [HttpGet("{username}/{checklistId:int}/items")]
        public IActionResult GetAllChecklistItems(string username, int checklistId)
        {
            var checkList = _checklistRepo.GetChecklistById(checklistId);

            if (checkList == null || checkList?.User?.Username != username)
                return NotFound();
                        
            var checklistItems = _checklistRepo.GetAllChecklistItemsByChecklistId(checklistId)
                .Select(cli => new ChecklistItemViewModel(cli));

            return new ObjectResult(checklistItems);
        }


        /// <summary>
        /// Gets a specific ChecklistItem with the given checklistItemId
        /// </summary>
        /// <param name="username"></param>
        /// <param name="checklistId"></param>
        /// <param name="checklistItemId"></param>
        /// <returns></returns>
        // GET api/Checklist/{username}/{id}/items/{checklistItemId:int}
        [HttpGet("{username}/{checklistId:int}/items/{checklistItemId:int}", Name = "GetCheckListItem")]
        public IActionResult GetCheckListItem(string username, int checklistId, int checklistItemId)
        {
            var checkList = _checklistRepo.GetChecklistById(checklistId);

            if (checkList == null || checkList?.User?.Username != username)
                return NotFound();

            var checklistItem = _checklistRepo.GetAllChecklistItemsByChecklistId(checklistId)
                .Where(cli => cli.Id == checklistItemId)
                .Select(cli => new ChecklistItemViewModel(cli))
                .FirstOrDefault();

            return new ObjectResult(checklistItem);
        }


        /// <summary>
        /// Creates a new checklistItem
        /// </summary>
        /// <param name="username"></param>
        /// <param name="checklistId"></param>
        /// <param name="checklistItem"></param>
        /// <returns></returns>
        /// <remarks>Supplying an Id in the body is unnecessary since it will be provided after the checklist is created.
        /// A valid user is required. Otherwise a 400 is returned.
        /// </remarks>
        [HttpPost("{username}/{checklistId:int}/items")]
        public IActionResult CreateChecklistItem(string username, int checklistId, [FromBody] ChecklistItemViewModel checklistItem)
        {
            
            var checklist = _checklistRepo.GetChecklistById(checklistId);

            if (checklistItem == null || checklist == null)
            {
                return BadRequest();
            }
            
            //verify that this is our own list we're adding to
            if (checklist?.User?.Username != username)
                return Unauthorized();
            
            var newChecklistItem = _checklistRepo.CreateCheckListItem(new ChecklistItem() {
                ChecklistId = checklistId,
                Description = checklistItem.Description,
                IsComplete = checklistItem.IsComplete
            });

            return CreatedAtRoute("GetCheckListItem", new
            {
                username = username,
                checklistId = newChecklistItem.ChecklistId,
                checklistItemId = newChecklistItem.Id
            }, new ChecklistItemViewModel(newChecklistItem));

        }


        /// <summary>
        /// Updates an existing ChecklistItem with the given checklistItemId.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="checklistId"></param>
        /// <param name="checklistItemId"></param>
        /// <param name="checklistItem"></param>
        /// <returns></returns>
        [HttpPut("{username}/{checklistId:int}/items/{checklistItemId:int}")]
        public IActionResult UpdateChecklistItem(string username, int checklistId, int checklistItemId, [FromBody] ChecklistItemViewModel checklistItem)
        {
            //make sure we have the required params
            if (checklistItem == null || checklistItem.Id == 0 || checklistItemId != checklistItem.Id)
            {
                return BadRequest();
            }

            //find the item
            var checklistItemToUpdate = _checklistRepo.GetCheckListItemById(checklistItem.Id);

            if (checklistItemToUpdate == null)
                return NotFound();

            //make sure we have access to it
            if (checklistItemToUpdate?.Checklist?.User?.Username != username)
                return Unauthorized();

            checklistItemToUpdate.Description = checklistItem.Description;
            checklistItemToUpdate.IsComplete = checklistItem.IsComplete;

            _checklistRepo.UpdateCheckListItem(checklistItemToUpdate);

            return NoContent();
        }


        /// <summary>
        /// Deletes an item from a checklist with the given checklistItemId
        /// </summary>
        /// <param name="username"></param>
        /// <param name="checklistId"></param>
        /// <param name="checklistItemId"></param>
        /// <returns></returns>
        [HttpDelete("{username}/{checklistId:int}/items/{checklistItemId:int}")]
        public IActionResult DeleteChecklistItem(string username, int checklistId, int checklistItemId)
        {
            var checklistItem = _checklistRepo.GetCheckListItemById(checklistItemId);
            if (checklistItem == null)
                return NotFound();

            //make sure we have access to it
            if (checklistItem?.Checklist?.User?.Username != username)
                return Unauthorized();

            _checklistRepo.DeleteChecklistItem(checklistItem);

            return new NoContentResult();
        }

    }
}
