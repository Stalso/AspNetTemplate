(function () {
    'use strict';

    angular
        .module('homeCtrl',[])
        .controller('homeController', ['$location', function () {
            var vm = this;
            vm.title = 'homeCtrl';
            vm.data = [
                {
                    a:'1'
                },
                {
                    a: '2'
                }
            ]
        }]);  
})();
