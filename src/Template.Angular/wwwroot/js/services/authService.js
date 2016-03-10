(function () {
    'use strict';

    angular
        .module('authService', [])
        .factory('Auth', ['$http', function ($http) {
            var serviceBase = 'http://localhost:10450/';
            var clientId = 'myPublicClient';
            var data = "grant_type=password&username=admin&password=admin&client_id=myPublicClient&scope=offline_access roles profile";

            var service = {
                getToken: getToken
            };

            return service;

            function getToken() {
                return $http.post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } });
            }
        }]);
})();