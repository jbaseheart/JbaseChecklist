(function () {
    'use strict';

    angular
        .module('jbaseChecklist')
        .controller('newChecklistController', ['$scope', '$routeParams', '$location', 'checklistRepository', newChecklistController]);

    function newChecklistController($scope, $routeParams, $location, checklistRepository) {

        $scope.name = '';
        $scope.description = '';
        $scope.valid = true;

        var username = $routeParams.username;
        var checklistId = $routeParams.checklistId;

        $scope.ValidateAndSubmit = function () {
            $scope.valid = $scope.name.length > 0 && $scope.description.length > 0;

            if ($scope.valid) {
                checklistRepository.AddChecklist(username, $scope.name, $scope.description).then(
                    function success(response) {
                        $location.path('/checklists/' + username + '/' + response.data.id);
                    },
                    function error(respsonse) {
                        if (console) {
                            console.error('Error while attempting %s to %s \n Status: %s \n Message: %s',
                                response.config.method,
                                response.config.url,
                                response.status,
                                response.statusText);
                        }
                    }
                );
            }
        }

    }
})();
