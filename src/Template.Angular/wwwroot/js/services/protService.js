(function () {
    'use strict';

    angular
        .module('protService',[])
        .factory('Prot', ['$http', function ($http) {
            var serviceBase = 'http://localhost:10450/'

            var service = {
                getData: getData
            };
            
            var config = {
                headers: {
                    Authorization: 'Bearer ' + 'eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IjFQREdZV084R0JVOUtKQjBBME04R0dBX0NJSjRBM0cxQ19JTllPVTkifQ.eyJuYmYiOjE0NTcwMTAyNTksImV4cCI6MTQ1NzAxMzg1OSwiaWF0IjoxNDU3MDEwMjU5LCJ1c2FnZSI6ImFjY2Vzc190b2tlbiIsImNvbmZpZGVudGlhbCI6dHJ1ZSwic2NvcGUiOiJvZmZsaW5lX2FjY2VzcyIsInN1YiI6IjU5NmFjMzY2LTYxNmQtNGRmZi1iYjgyLWM1ZDdiYzFiZDU1OSIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6MTA0NTAvIiwiYXpwIjoibXlQdWJsaWNDbGllbnQiLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjEwNDUwLyJ9.QZ6H6oOcbtuwoBtS7nEi5iTrg2FfqCDrldB-qYL_c-HzphwHdRLRbe81wyJZtfNKU0RdMAomBPUCyRSeSkHUaZ6JO4zRd1YXS6epNP6GFzg3Ty1iut_304HJNUusdLetHPnrUDRx9WjO1sUA5F_shJLlMwsbFNgA0dkA9fQdGgxTWvm8H07zFxRdy1qxCCL3lvTvOWZIrbBXB5i7slhYPfuwqmmS9qSipmbBftYlMGISGcccgzdzIutkrolEP6isCPutn3CF61i5_Gz0IhogZaWVftZWIauqx5fcC0xDLDakfvIZjDr9AL-yJZhx4xzplHaZxFbp0h3xJwBzBGl45w'
                }
            };
            return service;

            function getData() {
                //$http.defaults.headers.common['Authorization'] = 'Bearer ' + 'eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IjFQREdZV084R0JVOUtKQjBBME04R0dBX0NJSjRBM0cxQ19JTllPVTkifQ.eyJuYmYiOjE0NTcwMTAyNTksImV4cCI6MTQ1NzAxMzg1OSwiaWF0IjoxNDU3MDEwMjU5LCJ1c2FnZSI6ImFjY2Vzc190b2tlbiIsImNvbmZpZGVudGlhbCI6dHJ1ZSwic2NvcGUiOiJvZmZsaW5lX2FjY2VzcyIsInN1YiI6IjU5NmFjMzY2LTYxNmQtNGRmZi1iYjgyLWM1ZDdiYzFiZDU1OSIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6MTA0NTAvIiwiYXpwIjoibXlQdWJsaWNDbGllbnQiLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjEwNDUwLyJ9.QZ6H6oOcbtuwoBtS7nEi5iTrg2FfqCDrldB-qYL_c-HzphwHdRLRbe81wyJZtfNKU0RdMAomBPUCyRSeSkHUaZ6JO4zRd1YXS6epNP6GFzg3Ty1iut_304HJNUusdLetHPnrUDRx9WjO1sUA5F_shJLlMwsbFNgA0dkA9fQdGgxTWvm8H07zFxRdy1qxCCL3lvTvOWZIrbBXB5i7slhYPfuwqmmS9qSipmbBftYlMGISGcccgzdzIutkrolEP6isCPutn3CF61i5_Gz0IhogZaWVftZWIauqx5fcC0xDLDakfvIZjDr9AL-yJZhx4xzplHaZxFbp0h3xJwBzBGl45w'
                return $http.get(serviceBase + 'api/prvalues',config);
                //return $http.get(serviceBase + 'api/prvalues');
            }
        }]);

    protService.$inject = ['$http'];

    function protService($http) {
       
    }
})();