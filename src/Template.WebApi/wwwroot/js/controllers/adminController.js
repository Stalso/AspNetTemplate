(function () {
    'use strict';

    angular
        .module('adminCtrl', [])
        .controller('adminController', ['$location', 'Admin', function ($location, Admin) {
            var vm = this;
            vm.title = 'adminController';
            Admin.getData().then(function (res) {
                vm.values = res.data;
            }, function (err) {
                console.log(err);
            });
        }]);
})();
