(function () {
    'use strict';

    angular
        .module('loginCtrl', [])
        .controller('loginController', ['$location', function ($location) {
            var vm = this;
            vm.title = 'loginController';

        }]);
})();