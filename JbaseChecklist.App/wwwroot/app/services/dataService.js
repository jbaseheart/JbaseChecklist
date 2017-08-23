(function () {
    'use strict';

    angular
        .module('jbaseChecklist')
        .factory('dataService', ['$http', dataService]);

    //dataService.$inject = ['$http'];

    function dataService($http) {

        var baseUrl = 'http://jbasechecklistapi.azurewebsites.net/api';

        var service = {
            Get: Get,
            Post: Post,
            Delete: Delete
        };

        return service;

        function Get(url) {
            return $http.get(baseUrl + url);
        }

        function Post(url, body) {
            return $http.post(baseUrl + url, body);
        }

        function Delete(url) {
            return $http.delete(baseUrl + url);
        }
    }
})();