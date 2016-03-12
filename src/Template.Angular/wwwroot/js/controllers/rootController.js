(function () {
    'use strict';

    angular
        .module('rootCtrl',[])
        .controller('rootController', ['$location', '$rootScope', 'Auth', 'localStorageService', function ($location, $rootScope, Auth, localStorageService) {
            var vm = this;
            vm.title = 'homeCtrl';

            vm.logoff = function () {
                Auth.logoff();
            };
            
            $rootScope.$watch(function () { 
                return Auth.userData
            },
                function () {
                    vm.userData = Auth.userData;
                });
        }]);
})();
