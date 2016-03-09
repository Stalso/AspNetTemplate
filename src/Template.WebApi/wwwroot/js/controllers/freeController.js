(function () {
    'use strict';

    angular
        .module('freeCtrl',[])
        .controller('freeController', ['$location','Free', function ($location,Free) {
            var vm = this;
            vm.title = 'freeController';
            
        }]);
})();
