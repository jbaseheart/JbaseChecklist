using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using JbaseChecklist.Domain;
using JbaseChecklist.Domain.Models;
using JbaseChecklist.API.ViewModels;
using System.Threading.Tasks;

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
        public async Task<IActionResult> GetAllChecklists(string username)
        {
            var checklists = await _checklistRepo.GetAllChecklistsByUserNameAsync(username)
                .ContinueWith(t => t.Result.Select(cl => new ChecklistViewModel(cl)));
                
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
        public async Task<IActionResult> GetChecklist(string username, int checklistId)
        {
            var checklist = await _checklistRepo.GetChecklistByIdAsync(checklistId);

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
        public async Task<IActionResult> CreateChecklist(string username, [FromBody] ChecklistViewModel checklist)
        {
            var user = await _checklistRepo.GetUserByUserNameAsync(username);

            if (checklist == null || user == null)
                return BadRequest();

            var newCheckList = await _checklistRepo.CreateCheckListAsync(new Checklist() {
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
        public async Task<IActionResult> UpdateChecklist(string username, int checklistId, [FromBody] ChecklistViewModel checklist)
        {
            //make sure we have the required params
            if (checklist == null || checklist.Id == 0 || checklistId != checklist.Id)
            {
                return BadRequest();
            }

            //find the checklist
            var checklistToUpdate = await _checklistRepo.GetChecklistByIdAsync(checklist.Id);

            if (checklistToUpdate == null)
                return NotFound();

            //make sure we have access to it
            if (checklistToUpdate?.User?.Username != username)
                return Unauthorized();

            checklistToUpdate.Name = checklist.Name;
            checklistToUpdate.Description = checklist.Description;            

            await _checklistRepo.UpdateCheckListAsync(checklistToUpdate);

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
        public async Task<IActionResult> DeleteChecklist(string username, int checklistId)
        {
            var checklist = await _checklistRepo.GetChecklistByIdAsync(checklistId);
            if (checklist == null)
                return NotFound();

            //make sure we have access to it
            if (checklist?.User?.Username != username)
                return Unauthorized();

            //delete the items in the list first
            var itemsToDelete = await _checklistRepo.GetAllChecklistItemsByChecklistIdAsync(checklist.Id);
            await _checklistRepo.DeleteChecklistItemsAsync(itemsToDelete);

            //delete the list
            await _checklistRepo.DeleteChecklistAsync(checklist);

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
        public async Task<IActionResult> GetAllChecklistItems(string username, int checklistId)
        {
            var checkList = await _checklistRepo.GetChecklistByIdAsync(checklistId);

            if (checkList == null || checkList?.User?.Username != username)
                return NotFound();
                        
            var checklistItems = await _checklistRepo.GetAllChecklistItemsByChecklistIdAsync(checklistId)
                .ContinueWith(t => t.Result.Select(cli => new ChecklistItemViewModel(cli)));

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
        public async Task<IActionResult> GetCheckListItem(string username, int checklistId, int checklistItemId)
        {
            var checkList = await _checklistRepo.GetChecklistByIdAsync(checklistId);

            if (checkList == null || checkList?.User?.Username != username)
                return NotFound();

            var checklistItem = await _checklistRepo.GetAllChecklistItemsByChecklistIdAsync(checklistId)
                .ContinueWith(t => t.Result
                                    .Where(cli => cli.Id == checklistItemId)
                                    .Select(cli => new ChecklistItemViewModel(cli))
                                    .FirstOrDefault()
                );

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
        public async Task<IActionResult> CreateChecklistItem(string username, int checklistId, [FromBody] ChecklistItemViewModel checklistItem)
        {
            
            var checklist = await _checklistRepo.GetChecklistByIdAsync(checklistId);

            if (checklistItem == null || checklist == null)
            {
                return BadRequest();
            }
            
            //verify that this is our own list we're adding to
            if (checklist?.User?.Username != username)
                return Unauthorized();
            
            var newChecklistItem = await _checklistRepo.CreateCheckListItemAsync(new ChecklistItem() {
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
        public async Task<IActionResult> UpdateChecklistItem(string username, int checklistId, int checklistItemId, [FromBody] ChecklistItemViewModel checklistItem)
        {
            //make sure we have the required params
            if (checklistItem == null || checklistItem.Id == 0 || checklistItemId != checklistItem.Id)
            {
                return BadRequest();
            }

            //find the item
            var checklistItemToUpdate = await _checklistRepo.GetCheckListItemByIdAsync(checklistItem.Id);

            if (checklistItemToUpdate == null)
                return NotFound();

            //make sure we have access to it
            if (checklistItemToUpdate?.Checklist?.User?.Username != username)
                return Unauthorized();

            checklistItemToUpdate.Description = checklistItem.Description;
            checklistItemToUpdate.IsComplete = checklistItem.IsComplete;

            await _checklistRepo.UpdateCheckListItemAsync(checklistItemToUpdate);

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
        public async Task<IActionResult> DeleteChecklistItem(string username, int checklistId, int checklistItemId)
        {
            var checklistItem = await _checklistRepo.GetCheckListItemByIdAsync(checklistItemId);
            if (checklistItem == null)
                return NotFound();

            //make sure we have access to it
            if (checklistItem?.Checklist?.User?.Username != username)
                return Unauthorized();

            await _checklistRepo.DeleteChecklistItemAsync(checklistItem);

            return new NoContentResult();
        }

    }
}
