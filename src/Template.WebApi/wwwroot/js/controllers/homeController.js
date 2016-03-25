(function () {
    'use strict';

    angular
        .module('homeCtrl',[])
        .controller('homeController', ['$location', 'TemplateHub', function ($location, TemplateHub) {


          

            var vm = this;


            //var event = new CustomEvent('build', { 'detail': 'zz' });

            //vm.addEventListener('build', function (e) { console.log(e.detail + ' 1') }, false);
            //vm.addEventListener('build', function (e) { console.log(e.detail + ' 2') }, false);
            //vm.addEventListener('build', function (e) { console.log(e.detail + ' 3') }, false);
            //vm.addEventListener('build', function (e) { console.log(e.detail + ' 4') }, false);

            //vm.dispatchEvent(event);

            vm.title = 'homeCtrl';
            vm.data = [
                {
                    a: '1'
                },
                {
                    a: '2'
                }
            ];


            // Free Hub
            vm.freeMessage = "Free client text";
            vm.sendFreeMessage = function () {
                TemplateHub.hub.SendFreeMessage(vm.freeMessage);
            };
            TemplateHub.sendFreeMessage = function (data) {
                vm.freeMessage = data;
            };

            vm.protMessage = "Protected client text";
            vm.sendProtectedMessage = function () {
                var req = TemplateHub.hub.SendProtMessage(vm.protMessage);
            };            
            TemplateHub.sendProtectedMessage = function (data) {
                vm.protMessage = data;
            };

            vm.adminMessage = "Admin client text";
            vm.sendAdminMessage = function (){
                TemplateHub.hub.SendAdminMessage(vm.adminMessage);
            }
            TemplateHub.sendAdminMessage = function (data) {
                vm.adminMessage = data;
            };
            ///////////////////////////////////////////////

            // Prot Hub
            vm.freePrMessage = "Free client text";
            vm.sendPrFreeMessage = function () {
                TemplateHub.protHub.SendFreeMessage(vm.freePrMessage);
            };
            TemplateHub.sendPrFreeMessage = function (data) {
                vm.freePrMessage = data;
            };

            vm.protPrMessage = "Protected client text";
            vm.sendPrProtectedMessage = function () {
                var req = TemplateHub.protHub.SendProtMessage(vm.protPrMessage);
            };
            TemplateHub.sendPrProtectedMessage = function (data) {
                vm.protPrMessage = data;
            };

            vm.adminPrMessage = "Admin client text";
            vm.sendPrAdminMessage = function () {
                TemplateHub.protHub.SendAdminMessage(vm.adminPrMessage);
            }
            TemplateHub.sendPrAdminMessage = function (data) {
                vm.adminPrMessage = data;
            };
            ////////////////////////////////////////////////////


           
            vm.connectHub = function () {
                TemplateHub.hub.connect();
            };
            vm.disconnectHub = function () {
                TemplateHub.hub.disconnect();             
            };

            vm.connectPrHub = function () {
                TemplateHub.protHub.connect();
            };
            vm.disconnectPrHub = function () {
                TemplateHub.protHub.disconnect();
            };

            vm.changeToken = function () {
                TemplateHub.changeQueryToken();
            };
            vm.changeOnUserToken = function () {
                TemplateHub.changeOnUserQueryToken();
            };
            vm.changeOnBadToken = function () {
                TemplateHub.changeOnBadQueryToken();
            };           
            vm.clearQueryToken = function () {
                TemplateHub.clearQueryToken();
                
            };

            vm.changeConnectionToken = function () {
                TemplateHub.changeToken();
            };
            vm.changeOnUserConnectionToken = function () {
                TemplateHub.changeOnUserToken();
            };
            vm.changeOnBadConnectionToken = function () {
                TemplateHub.changeOnBadToken();
            };
            vm.clearConnectionToken = function () {
                TemplateHub.clearToken();
                
            };
            
            //TemplateHub.hub.promise = TemplateHub.hub.connect();
            //TemplateHub.protHub.promise = TemplateHub.protHub.connect();
        }]);  
})();
