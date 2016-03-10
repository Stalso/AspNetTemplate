(function () {
    'use strict';

    angular
        .module('protService',[])
        .factory('Prot', ['$http', function ($http) {
            var serviceBase = 'http://localhost:10450/'

            var service = {
                getData: getData
            };
            
          
            return service;

            function getData() {
                return $http.get("/api/prvalues");
            }
        }]);

  
})();