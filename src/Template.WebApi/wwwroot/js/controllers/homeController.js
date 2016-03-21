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
                    console.log('Controller log' + err);
                });
            };
            
            TemplateHub.sendProtectedMessage = function (data) {
                vm.protMessage = data;
            };

            vm.disconnectHub = function () {
                TemplateHub.hub.disconnect();
                //TemplateHub.hub.connect();

            };
            vm.connectHub = function () {
                //TemplateHub.changeToken();
                TemplateHub.changeQueryToken();
                TemplateHub.hub.connect();
            };
            vm.makeBadConnection = function () {
                TemplateHub.changeOnBadQueryToken();
                TemplateHub.hub.connect();
            };
            vm.changeOnBadToken = function () {
                TemplateHub.changeOnBadQueryToken();
            };
            vm.changeToken = function () {
                TemplateHub.changeQueryToken();
            };
            vm.clearQueryToken = function () {
                TemplateHub.clearQueryToken();
                TemplateHub.hub.connect();
            };
            
            
        }]);  
})();
