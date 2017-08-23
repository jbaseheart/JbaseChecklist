(function () {
    'use strict';

    angular
        .module('jbaseChecklist')
        .factory('dataService', ['$http', dataService]);

    function dataService($http) {

        var baseUrl = 'http://jbasechecklistapi.azurewebsites.net/api';

        var service = {
            Get: Get,
            Post: Post,
            Put: Put,
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

        function Put(url, body) {
            return $http.put(baseUrl + url, body);
        }
    }
})();