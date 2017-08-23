(function () {
    'use strict';

    angular
        .module('jbaseChecklist')
        .controller('checklistController', ['$scope', '$routeParams', '$location', 'checklistRepository', 'focus', checklistController]);

    //checklistController.$inject = ['$scope'];

    function checklistController($scope, $routeParams, $location, checklistRepository, focus) {

        $scope.checklist = {};
        $scope.checklistItems = [];

        var username = $routeParams.username;
        var checklistId = $routeParams.checklistId;

        $scope.username = username;
        $scope.checklistId = checklistId;


        $scope.AddItem = AddItem;
        $scope.UpdateItem = UpdateItem;
        $scope.DeleteItem = DeleteItem;

        $scope.setFocus = function (id) {
            focus(id);
        }

        function RefreshList() {
            //get the individual items in the checklist
            checklistRepository.GetChecklistItems(username, checklistId).then(
                // callback function for successful http request
                function success(response) {
                    $scope.checklistItems = response.data;
                },
                // callback function for error in http request
                function error(response) {
                    // log errors
                }
            );
        }

        function UpdateItem(item) {
            checklistRepository.UpdateChecklistItem(username, checklistId, item.id, item.description, item.isComplete).then(
                function success(response) {
                    RefreshList();
                },
                function error(response) {
                    // log errors
                }
            );
        }

        function AddItem() {
            //show the item in the list immediately
            $scope.checklistItems.push({ description: $scope.newItemText, isComplete: false });

            //persist to backend
            checklistRepository.AddChecklistItem(username, checklistId, $scope.newItemText, false).then(
                function success(response) {
                    RefreshList();                    
                },
                function error(response) {
                    // log errors
                }
            );

            //clear out the text box
            $scope.newItemText = '';
        }

        function DeleteItem(checklistItemId) {
            checklistRepository.DeleteChecklistItem(username, checklistId, checklistItemId).then(
                function success(response) {
                    RefreshList();
                },
                function error(response) {
                    // log errors
                }
            );
        }

        activate();

        function activate() {

            //get the info for the checklist
            checklistRepository.GetChecklist(username, checklistId).then(
                // callback function for successful http request
                function success(response) {
                    $scope.checklist = response.data;
                },
                // callback function for error in http request
                function error(response) {
                    // log errors
                }
            );

            RefreshList();
        }
    }
})();
