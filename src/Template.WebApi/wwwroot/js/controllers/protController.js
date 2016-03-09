(function () {
    'use strict';

    angular
        .module('protCtrl',[])
        .controller('protController', ['$location', 'Prot', function ($location, Prot) {
            var vm = this;
            vm.title = 'protController';
          
        }]);

   
})();
