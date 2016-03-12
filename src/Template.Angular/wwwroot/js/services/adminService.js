(function () {
    'use strict';

    angular
        .module('adminService', [])
        .factory('Admin', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {
            var serviceBase = 'http://localhost:10450/'

            var service = {
                getData: getData
            };


            return service;

            function getData() {
                return $http.get(ngAuthSettings.apiServiceBaseUri + "/api/admin");
            }
        }]);


})();