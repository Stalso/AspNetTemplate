(function () {
    'use strict';

    angular
        .module('rootCtrl',[])
        .controller('rootController', ['$location','Auth', function ($location,Auth) {
            var vm = this;
            vm.title = 'homeCtrl';

            vm.logoff = function () {
                Auth.logoff();
            };

        }]);
})();
