(function () {
    'use strict';

    angular
        .module('rootCtrl',[])
        .controller('rootController', rootController);

    rootController.$inject = ['$location']; 

    function rootController($location) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'rootController';

        activate();

        function activate() { }
    }
})();
