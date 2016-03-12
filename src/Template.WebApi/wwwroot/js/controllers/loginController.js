(function () {
    'use strict';

    angular
        .module('loginCtrl', [])
        .controller('loginController', ['$location', 'Auth', 'jwtHelper', '$window', function ($location, Auth, jwtHelper, $window) {
            var vm = this;
            vm.title = 'loginController';

            vm.loginData = {
                username: 'admin',
                password: 'admin',
               
            };

            vm.getToken = function (data)
            {
                //console.log(data);
                //$location.path('/home');
                
                Auth.getToken(data).then(function (res) {
                   
                    var returnUrl = $location.search().returnUrl;
                    if (returnUrl) {
                        $location.url(returnUrl);
                    }
                    else
                        $location.path('/');
                }, function (err) {
                    $window.alert('Incorrect username/password')
                    console.log(err);
                });

            }
            vm.refreshToken = function () {
                //console.log(data);
                //$location.path('/home');

                Auth.refreshToken().then(function (res) {
                    if (res) {
                        vm.token = jwtHelper.decodeToken(res.data.access_token);
                        vm.refresh_token = res.data.refresh_token;
                    }
                }, function (err) {
                    console.log(err);
                });
            }

           

        }]);
})();