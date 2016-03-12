(function () {
    'use strict';

    angular
        .module('protService',[])
        .factory('Prot', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {
           

            var service = {
                getData: getData
            };
            
          
            return service;

            function getData() {
                return $http.get(ngAuthSettings.apiServiceBaseUri + "/api/prvalues");
            }
        }]);

  
})();