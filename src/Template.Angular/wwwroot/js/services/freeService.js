(function () {
    'use strict';

    angular
        .module('freeService',[])
        .factory('Free', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {
           

            var service = {
                getData: getData
            };

            return service;

            function getData() {
                return $http.get(ngAuthSettings.apiServiceBaseUri + "/api/values");
            }
        }]);
})();