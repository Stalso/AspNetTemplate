(function () {
    'use strict';

    angular
        .module('homeCtrl',[])
        .controller('homeController', ['$location', 'TemplateConnection', function ($location, TemplateConnection) {

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


            // Free Hub
            vm.freeMessage = "Free client text";
            vm.sendFreeMessage = function () {
                TemplateConnection.hub.SendFreeMessage(vm.freeMessage);
            };
            TemplateConnection.sendFreeMessage = function (data) {
                vm.freeMessage = data;
            };

            vm.protMessage = "Protected client text";
            vm.sendProtectedMessage = function () {
                TemplateConnection.hub.SendProtMessage(vm.protMessage);
            };            
            TemplateConnection.sendProtectedMessage = function (data) {
                vm.protMessage = data;
            };

            vm.adminMessage = "Admin client text";
            vm.sendAdminMessage = function (){
                TemplateConnection.hub.SendAdminMessage(vm.adminMessage);
            }
            TemplateConnection.sendAdminMessage = function (data) {
                vm.adminMessage = data;
            };
            ///////////////////////////////////////////////

            // Prot Hub
            vm.freePrMessage = "Free client text";
            vm.sendPrFreeMessage = function () {
                TemplateConnection.protHub.SendFreeMessage(vm.freePrMessage);
            };
            TemplateConnection.sendPrFreeMessage = function (data) {
                vm.freePrMessage = data;
            };

            vm.protPrMessage = "Protected client text";
            vm.sendPrProtectedMessage = function () {
                var req = TemplateConnection.protHub.SendProtMessage(vm.protPrMessage);
            };
            TemplateConnection.sendPrProtectedMessage = function (data) {
                vm.protPrMessage = data;
            };

            vm.adminPrMessage = "Admin client text";
            vm.sendPrAdminMessage = function () {
                TemplateConnection.protHub.SendAdminMessage(vm.adminPrMessage);
            }
            TemplateConnection.sendPrAdminMessage = function (data) {
                vm.adminPrMessage = data;
            };
            ////////////////////////////////////////////////////


            // Free Hub
            vm.connectHub = function () {
                TemplateConnection.hub.connect();
            };
            vm.disconnectHub = function () {
                TemplateConnection.hub.disconnect();
            };

          
            // notificated
            vm.changeToken = function () {
                TemplateConnection.changeToken();
            };
            vm.changeOnUserToken = function () {
                TemplateConnection.changeOnUserToken();
            };
            vm.changeOnBadToken = function () {
                TemplateConnection.changeOnBadToken();
            };           
            vm.clearToken = function () {
                TemplateConnection.clearToken();                
            };
            vm.rejectToken = function () {
                TemplateConnection.rejectToken();
            };

            // not notificated
            vm.changeTokenArt = function () {
                TemplateConnection.changeTokenArt();
            };
            vm.changeOnUserTokenArt = function () {
                TemplateConnection.changeOnUserTokenArt();
            };
            vm.changeOnBadTokenArt = function () {
                TemplateConnection.changeOnBadTokenArt();
            };
            vm.clearTokenArt = function () {
                TemplateConnection.clearTokenArt();                
            };
            vm.rejectTokenArt = function () {
                TemplateConnection.rejectTokenArt();
            };

            // qs
            vm.changeQsToken = function () {
                TemplateConnection.changeQsToken();
            };
            vm.changeQsOnUserToken = function () {
                TemplateConnection.changeQsOnUserToken();
            };
            vm.changeQsOnBadToken = function () {
                TemplateConnection.changeQsOnBadToken();
            };
            vm.clearQsToken = function () {
                TemplateConnection.clearQsToken();
            };


            // protected hub
            vm.connectPrHub = function () {
                TemplateConnection.protHub.connect();
            };
            vm.disconnectPrHub = function () {
                TemplateConnection.protHub.disconnect();
            };
            ///////////////
           
        }]);  
})();
