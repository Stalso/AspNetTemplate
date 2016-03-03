(function () {
    'use strict';

    angular
        .module('protCtrl',[])
        .controller('protController', ['$location', 'Prot', function ($location, Prot) {
            var vm = this;
            vm.title = 'protController';
            Prot.getData().then(function (res) {
                vm.data = res;
            }, function (err) {
                console.log(err);
            })
        }]);

   
})();
