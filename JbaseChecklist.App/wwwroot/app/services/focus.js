(function () {
    'use strict';

    angular
        .module('jbaseChecklist')
        .factory('focus', function ($timeout, $window) {
            return function (id) {
                $timeout(function () {
                    var element = $window.document.getElementById(id);
                    if (element)
                        element.focus();
                });
            };
        });
})();