(function () {
    'use strict';

    angular
        .module('authService', [])
        .factory('Auth', ['$http', function ($http) {

            var clientId = 'myPublicClient';
            var data = "grant_type=password&username=admin&password=admin&client_id=myPublicClient&scope=offline_access";

            var service = {
                getData: getData
            };

            return service;

            function getData() {

            }
        }]);

})();