using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using JbaseChecklist.Domain;
using JbaseChecklist.Domain.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JbaseChecklist.API.Controllers
{
    [Route("api/[controller]")]
    public class ChecklistController : Controller
    {
        private readonly IChecklistRepository _checklistRepo;

        public ChecklistController(IChecklistRepository checklistRepo)
        {
            _checklistRepo = checklistRepo;

            CreateDefaultListIfEmpty();
        }

        private void CreateDefaultListIfEmpty()
        {
            var allItems = _checklistRepo.GetAllChecklistItems();

            if (allItems.Count() == 0)
            {
                _checklistRepo.CreateCheckListItem(new ChecklistItem { Description = "Item1" });
            }
        }

        [HttpGet]
        public IEnumerable<ChecklistItem> GetAll()
        {
            return _checklistRepo.GetAllChecklistItems();
        }

        [HttpGet("{id}", Name = "GetCheckListItem")]
        public IActionResult GetById(int id)
        {
            var item = _checklistRepo.GetCheckListItemById(id);
            if (item == null)
            {
                return NotFound();
            }

            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] ChecklistItem item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            var newItem = _checklistRepo.CreateCheckListItem(item);

            return CreatedAtRoute("GetCheckListItem", new { id = newItem.Id }, newItem);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ChecklistItem item)
        {
            if(item == null || item.Id != id)
            {
                return BadRequest();
            }

            var existingItem = _checklistRepo.GetCheckListItemById(id);
            if(existingItem == null)
            {
                return NotFound();
            }

            existingItem.Description = item.Description;
            existingItem.IsComplete = item.IsComplete;

            _checklistRepo.UpdateCheckListItem(existingItem);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = _checklistRepo.GetCheckListItemById(id);
            if (item == null)
            {
                return NotFound();
            }

            _checklistRepo.Delete(item);

            return new NoContentResult();
        }

    }
}
