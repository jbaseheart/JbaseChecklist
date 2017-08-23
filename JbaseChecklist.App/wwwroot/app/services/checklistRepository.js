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

        //get a specific checklist
        function GetChecklist(username, checklistId) {
            return dataService.Get('/Checklist/' + username + '/' + checklistId);
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

        function DeleteChecklistItem(username, checklistId, checklistItemId) {
            return dataService.Delete('/Checklist/' + username + '/' + checklistId + '/items/' + checklistItemId);
        }

        var service = {
            GetChecklists: GetChecklists,
            GetChecklist: GetChecklist,
            GetChecklistItems: GetChecklistItems,
            AddChecklistItem: AddChecklistItem,
            DeleteChecklistItem: DeleteChecklistItem
        };

        return service;
    }
})();