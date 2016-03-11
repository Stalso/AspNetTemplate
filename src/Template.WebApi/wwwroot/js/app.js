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
         'adminCtrl',
         'adminService',
         'registerCtrl',
         'angularJwt',
        // 3rd Party Modules
        'LocalStorageModule',
    ]).config(['$httpProvider', 'jwtInterceptorProvider', 'localStorageServiceProvider', function ($httpProvider, jwtInterceptorProvider, localStorageServiceProvider) {
        localStorageServiceProvider.setNotify(true, true);
        jwtInterceptorProvider.tokenGetter = ['jwtHelper', '$http', 'localStorageService', 'Auth', function (jwtHelper, $http, localStorageService, Auth) {
            console.log('tokenGetter');
            var token = Auth.getAccessTokenWithRefresh();
            return token;
        }];

        $httpProvider.interceptors.push('jwtInterceptor');
    }]).run(["$rootScope", "$location", 'Auth', function ($rootScope, $location,Auth) {
        $rootScope.$on("unauthenticated", function (userInfo) {
            
            var url = $location.url();
            $location.path("/login").search({ 'returnUrl': url });
        });
       
        $rootScope.$on('LocalStorageModule.notification.setitem', function (scope,data) {
            if(data.key == 'authData')
                Auth.changeAuthData();
            console.log('authdataset');
        });
        $rootScope.$on('LocalStorageModule.notification.removeitem', function (scope,data) {
            if (data.key == 'authData')
                Auth.changeAuthData();
            console.log('authdataremove');
        });
        $rootScope.$on('$routeChangeStart', function (event, next, current) {
            var nextPath = 'undef';
            var prevPath = 'undef';
            if (next)
                nextPath = next.originalPath;
            if (current)
                prevPath = current.originalPath;
            console.log('route changed next: ' + nextPath + ' current: ' + prevPath);
        });

    }]);
})();