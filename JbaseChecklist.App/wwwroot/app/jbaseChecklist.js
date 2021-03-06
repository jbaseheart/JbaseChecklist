﻿(function () {
    'use strict';

    var jbaseChecklist = angular.module('jbaseChecklist', [
        // Angular modules 
        'ngRoute'
    ])
    .config(['$routeProvider',
        function ($routeProvider) {
            $routeProvider.
                when('/users', {
                    templateUrl: '/app/views/users.html',
                    controller: 'usersController'
                }).
                when('/checklists/:username/new', {
                    templateUrl: '/app/views/new.html',
                    controller: 'newChecklistController'
                }).
                when('/checklists/:username', {
                    templateUrl: '/app/views/checklists.html',
                    controller: 'checklistsController'
                }).
                when('/checklists/:username/:checklistId', {
                    templateUrl: '/app/views/checklist.html',
                    controller: 'checklistController'
                }).
                otherwise({
                    redirectTo: '/users'
                });
        }
    ]);


})();