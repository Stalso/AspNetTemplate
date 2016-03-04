(function () {
    'use strict';

    angular.module('DemoApp', [
        // Angular modules 
        'ngRoute',

        // Custom modules 
         'routes',
         'homeCtrl',
         'freeCtrl',
         'freeService',
         'protCtrl',
         'protService',
         'rootCtrl',
         'tokenCtrl',
         'authService'
        // 3rd Party Modules
        
    ]);
})();