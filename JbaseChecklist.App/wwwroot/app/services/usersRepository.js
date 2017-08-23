(function () {
    'use strict';

    angular
        .module('jbaseChecklist')
        .factory('usersRepository', ['$http', 'dataService', usersRepository]);


    function usersRepository($http, dataService) {
        function GetUsers() {
            
            return dataService.Get('/Users');
        }

        var service = {
            GetUsers: GetUsers
        };

        return service;
    }
})();