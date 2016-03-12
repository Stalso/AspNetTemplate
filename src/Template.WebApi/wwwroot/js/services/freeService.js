(function () {
    'use strict';

    angular
        .module('freeService',[])
        .factory('Free', ['$http', function ($http) {
           

            var service = {
                getData: getData
            };

            return service;

            function getData() {
                return $http.get("/api/values", { skipAuthorization: true });
            }
        }]);
})();