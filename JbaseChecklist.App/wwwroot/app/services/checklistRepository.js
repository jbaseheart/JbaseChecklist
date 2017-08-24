(function () {
    'use strict';

    angular
        .module('jbaseChecklist')
        .factory('checklistRepository', ['$http', 'dataService', checklistRepository]);


    function checklistRepository($http, dataService) {

        //get all checklsts
        function GetChecklists(username) {            
            return dataService.Get('/Checklist/' + username);
        }

        //adds a new checklist
        function AddChecklist(username, name, description) {
            return dataService.Post('/Checklist/' + username, {
                name: name,
                description: description
            });
        }

        //get a specific checklist
        function GetChecklist(username, checklistId) {
            return dataService.Get('/Checklist/' + username + '/' + checklistId);
        }

        //updates a checklist
        function UpdateChecklist(username, checklistId, name, description) {
            return dataService.Put('/Checklist/' + username + '/' + checklistId, {
                id: checklistId,
                name: name,
                description: description
            });
        }

        //delete a checklist
        function DeleteChecklist(username, checklistId) {
            return dataService.Delete('/Checklist/' + username + '/' + checklistId);
        }

        //gets all the items in a checklist
        function GetChecklistItems(username, checklistId) {
            return dataService.Get('/Checklist/' + username + '/' + checklistId + '/items');
        }

        //adds a new item to the list
        function AddChecklistItem(username, checklistId, description, isComplete) {
            return dataService.Post('/Checklist/' + username + '/' + checklistId + '/items', {
                checklistId: checklistId,
                description: description,
                isComplete: isComplete
            });
        }

        //updates an item in the list
        function UpdateChecklistItem(username, checklistId, checklistItemId, description, isComplete) {
            return dataService.Put('/Checklist/' + username + '/' + checklistId + '/items/' + checklistItemId, {
                id: checklistItemId,
                checklistId: checklistId,
                description: description,
                isComplete: isComplete
            });
        }

        //deletes an item from the list
        function DeleteChecklistItem(username, checklistId, checklistItemId) {
            return dataService.Delete('/Checklist/' + username + '/' + checklistId + '/items/' + checklistItemId);
        }

        var service = {
            GetChecklists: GetChecklists,
            GetChecklist: GetChecklist,
            AddChecklist: AddChecklist,
            UpdateChecklist: UpdateChecklist,
            DeleteChecklist: DeleteChecklist,
            GetChecklistItems: GetChecklistItems,
            AddChecklistItem: AddChecklistItem,
            UpdateChecklistItem: UpdateChecklistItem,
            DeleteChecklistItem: DeleteChecklistItem
        };

        return service;
    }
})();