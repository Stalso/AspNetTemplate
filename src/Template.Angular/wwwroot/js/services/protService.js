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
                
                //$http.defaults.headers.common['Access-Control-Allow-Origin'] = true;
                //$http.defaults.useXDomain = true;
                //return $http.get(serviceBase + 'api/prvalues',config);
                //$http.defaults.headers.common['Authorization'] = 'Bearer ' + 'eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IjFQREdZV084R0JVOUtKQjBBME04R0dBX0NJSjRBM0cxQ19JTllPVTkifQ.eyJuYmYiOjE0NTcwNzM5MTQsImV4cCI6MTQ1NzA3NzUxNCwiaWF0IjoxNDU3MDczOTE0LCJ1c2FnZSI6ImFjY2Vzc190b2tlbiIsImNvbmZpZGVudGlhbCI6dHJ1ZSwic2NvcGUiOiJvZmZsaW5lX2FjY2VzcyIsInN1YiI6Ijk3NzBiOTliLWI1ZDItNGE0MS05YTZmLWQ4MjlkYTMwNWNkNyIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6MTAzNzciLCJhenAiOiJteVB1YmxpY0NsaWVudCIsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6MTA0NTAvIn0.zRX3EFyY5Lb_Ksd-38PxTFCeySLbx8vTi_Xqc66Zuu25xClAqqF4Z89USyWVk7Veqkft_EyXSvgE8Yv9HtjUaQ0NNM1-i2oJNH6w_Z8I6kIfRNzJX4qbS8dhq29IeKdzAu0atN42H2Ljbq5ONbyNtivaHNiNyMlxZ-rp78Mwd4O7riJDEDIwCmuZfL4iXWt-tNqgA8oNf2q0giQo1ESh-U7wV1QV8Mb122M4MvhyOCJf-_s7rcy4y6dloj3oZotw2EjJog79tEIxwWsHJJ7kHudWtn4A2L6BGaa4ltXgsyyCNvYTgf4AZFhuPD4MT0rzrvBItwndezYSm2dQ4O67jA';
               
                return $http.get('http://localhost:10450/' + 'api/prvalues', {

                    headers: {
                        'Authorization': 'Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IjFQREdZV084R0JVOUtKQjBBME04R0dBX0NJSjRBM0cxQ19JTllPVTkifQ.eyJuYmYiOjE0NTcwOTU4MTcsImV4cCI6MTQ1NzA5OTQxNywiaWF0IjoxNDU3MDk1ODE3LCJ1c2FnZSI6ImFjY2Vzc190b2tlbiIsImNvbmZpZGVudGlhbCI6dHJ1ZSwic2NvcGUiOiJvZmZsaW5lX2FjY2VzcyIsInN1YiI6IjRkODlkMTViLTNhMGQtNDYyOS1iY2YxLTMxZDA5YjMzNDA4MSIsImF1ZCI6WyJodHRwOi8vbG9jYWxob3N0OjEwNDUwLyIsImh0dHA6Ly9sb2NhbGhvc3Q6MTAzNzcvIl0sImF6cCI6Im15UHVibGljQ2xpZW50IiwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDoxMDQ1MC8ifQ.y1tk2nKRBmHavO9ZqNPNlzT6pitYFrWFYde-vRNsp8rnhjakAKXjgi63xgOjaPGyr9iHmRGWyeAfbaqwE0oseLa80Ox2cgwykQUusoEotV-OGvaoXfUJnaLNtjI2BmbiUTMsP7Vsoi9EnCJ6O4jWBybL2aIJzMtXyjnXiv0RitMkODZrCHjWstXqFpKcFVSrKpLWm1FWQiaaUfNAF88q9hIh1lxFnXvBzyjDEM8gJB8ZD6ZN7QZyQOs7ER6MjVtAOvbu9va9tc-U4eO38vbgPC9oEo4qxzsGhId4xWj_ppcsU2SfJSU_uCITNxr3ZyDLmeBJgd3MHmsgZU8TenX7lw',
                        'Content-Type': 'application/json'
                    }
                });
            }
        }]);

  
})();