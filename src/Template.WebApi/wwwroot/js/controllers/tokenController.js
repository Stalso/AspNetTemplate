(function () {
    'use strict';

    angular
        .module('tokenCtrl', [])
        .controller('tokenController', ['$location', 'Auth', function ($location, Auth) {
            var vm = this;
            vm.title = 'tokenController';
          
        }]);


})();
