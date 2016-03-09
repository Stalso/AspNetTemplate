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
         'loginCtrl',
         'authService',
         'angularJwt',
        // 3rd Party Modules
        'LocalStorageModule',
    ]).config(['$httpProvider', 'jwtInterceptorProvider', function ($httpProvider, jwtInterceptorProvider) {
        jwtInterceptorProvider.tokenGetter = function ()
        {
            console.log("token Getter works");
        }
        $httpProvider.interceptors.push('jwtInterceptor');
    }]);
})();