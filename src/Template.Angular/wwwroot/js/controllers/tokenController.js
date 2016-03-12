(function () {
    'use strict';

    angular
        .module('tokenCtrl', [])
        .controller('tokenController', ['$location', 'Auth', '$rootScope', 'jwtHelper', function ($location, Auth, $rootScope, jwtHelper) {
            var vm = this;
            vm.title = 'tokenController';
            
            $rootScope.$watch(function () {
                return Auth.userData
            },
               function () {
                   if (Auth.userData)
                   {
                       //vm.data = Auth.userData;
                       var decodedToken = jwtHelper.decodeToken(Auth.userData.access_token);
                       vm.data = {
                           User: Auth.userData.username,
                           Roles: decodedToken.role,
                           IsExpired: jwtHelper.isTokenExpired(Auth.userData.access_token),
                           decoded_token: decodedToken,
                           refresh_token: Auth.userData.refresh_token
                       };
                   }
                   else
                       vm.data = { info: 'No token' };
               });
            //vm.data = {
            //    a: 'test',
            //    b: 28
            //};
            //Auth
          
        }]);


})();
