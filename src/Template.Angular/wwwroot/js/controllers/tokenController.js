(function () {
    'use strict';

    angular
        .module('tokenCtrl', [])
        .controller('tokenController', ['$location', 'Auth', function ($location, Auth) {
            var vm = this;
            vm.title = 'tokenController';
            Auth.getToken().then(function (res) {
                vm.data = res;
            }, function (err) {
                console.log(err);
            })
        }]);


})();
