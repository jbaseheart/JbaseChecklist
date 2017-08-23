﻿(function () {
    'use strict';

    angular
        .module('jbaseChecklist')
        .controller('checklistsController', ['$scope', '$routeParams', '$location', 'checklistRepository', checklistsController]);

    //checklistController.$inject = ['$scope'];

    function checklistsController($scope, $routeParams, $location, checklistRepository) {

        activate();

        function activate() {

            $scope.checklists = [];

            var username = $routeParams.username;
            $scope.username = username;

            $scope.SelectChecklist = function (checklistId) {
                $location.path('/checklists/' + username + '/' + checklistId);
            }

            //get the checklists for the current user
            checklistRepository.GetChecklists(username).then(
                // callback function for successful http request
                function success(response) {
                    $scope.checklists = response.data;
                },
                // callback function for error in http request
                function error(response) {
                    // log errors
                }
            );
        }
    }
})();
