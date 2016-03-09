(function () {
    'use strict';

    angular
        .module('rootCtrl',[])
        .controller('rootController', ['$location', function () {
            var vm = this;
            vm.title = 'homeCtrl';
        }]);
})();
