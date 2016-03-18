(function () {
    'use strict';

    angular
        .module('homeCtrl',[])
        .controller('homeController', ['$location', 'TemplateHub', function ($location, TemplateHub) {
            var vm = this;
            vm.title = 'homeCtrl';
            vm.data = [
                {
                    a: '1'
                },
                {
                    a: '2'
                }
            ];

            vm.freeMessage = "Free client text";

            vm.sendFreeMessage = function () {
                TemplateHub.hub.SendFreeMessage(vm.freeMessage);
            };

            TemplateHub.sendFreeMessage = function (data) {
                vm.freeMessage = data;
            };

            vm.protMessage = "Protected client text";

            vm.sendProtectedMessage = function () {
                //TemplateHub.hub.SendProtMessage(vm.protMessage).done(function () {
                //    console.log(arguments);
                //})
                //    .fail(function (r) {
                //        console.log(arguments);
                //    });
                TemplateHub.hub.SendProtMessage(vm.protMessage).then(function (data) {
                    console.log(data);
                }, function (err) {
                    console.log(err);
                });
            };

            TemplateHub.sendProtectedMessage = function (data) {
                vm.protMessage = data;
            };

            
        }]);  
})();
