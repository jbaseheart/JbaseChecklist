(function () {
    'use strict';

    angular
        .module('jbaseChecklist')
        .controller('usersController', ['$scope', '$location', 'usersRepository', usersController]);


    function usersController($scope, $location, usersRepository) {
        $scope.title = 'usersController';

        activate();

        function activate() {
            $scope.users = [];

            //go to the user's checklist when clicking on their username
            $scope.SelectUser = function (username) {
                $location.path('/checklists/' + username);
            }

            usersRepository.GetUsers().then(
                // callback function for successful http request
                function success(response) {
                    $scope.users = response.data;
                },
                // callback function for error in http request
                function error(response) {
                    // log errors
                }
            );
        }
    }
})();
