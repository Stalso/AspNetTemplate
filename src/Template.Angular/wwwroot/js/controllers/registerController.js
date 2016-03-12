(function () {
    'use strict';

    angular
        .module('registerCtrl', [])
        .controller('registerController', ['$location', 'Auth',  function ($location, Auth) {
            var vm = this;
            vm.title = 'registerController';

            vm.loginData = {
                username: 'newUser',
                password: 'admin',
                confirmpassword: 'admin',
            };

            vm.register = function (data) {
                Auth.register(data).then(function (res) {
                    $location.path('/login');
                }, function (err) {
                    console.log(err);
                });
            }



        }]);
})();