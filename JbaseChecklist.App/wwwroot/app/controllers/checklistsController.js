(function () {
    'use strict';

    angular
        .module('jbaseChecklist')
        .controller('checklistsController', ['$scope', '$routeParams', '$location', 'checklistRepository', checklistsController]);

    //checklistController.$inject = ['$scope'];

    function checklistsController($scope, $routeParams, $location, checklistRepository) {

        $scope.checklists = [];
        $scope.NewChecklist = NewChecklist;
        $scope.DeleteChecklist = DeleteChecklist;

        var username = $routeParams.username;
        $scope.username = username;

        $scope.SelectChecklist = function (checklistId) {
            $location.path('/checklists/' + username + '/' + checklistId);
        }

        function RefreshList() {
            //get the checklists for the current user
            checklistRepository.GetChecklists(username).then(
                function success(response) {
                    $scope.checklists = response.data;
                },
                LogError
            );
        }

        function NewChecklist() {
            $location.path('/checklists/' + username + '/new');
        }

        function DeleteChecklist(checklistId) {
            checklistRepository.DeleteChecklist(username, checklistId).then(
                function success(response) {
                    RefreshList();
                },
                LogError
            );
        }

        function LogError(response) {
            if (console) {
                console.error('Error while attempting %s to %s \n Status: %s \n Message: %s',
                    response.config.method,
                    response.config.url,
                    response.status,
                    response.statusText);
            }
        }

        activate();

        function activate() {
            RefreshList();
        }
    }
})();
