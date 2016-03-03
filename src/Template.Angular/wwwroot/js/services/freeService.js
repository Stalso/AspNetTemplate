(function () {
    'use strict';

    angular
        .module('freeService',[])
        .factory('Free', ['$http', function ($http) {
            var serviceBase = 'http://localhost:10450/'

            var service = {
                getData: getData
            };

            return service;

            function getData() {
                return $http.get(serviceBase + 'api/values');
            }
        }]);
})();