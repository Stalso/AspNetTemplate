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
         'forbiddenCtrl',
         'angularJwt',
         'io.utils',
         'sampleEntsListDirective',
         'SignalRService',
        // 3rd Party Modules
        'LocalStorageModule',
        'SignalR'
    ]).config(['$httpProvider', 'jwtInterceptorProvider', 'localStorageServiceProvider', function ($httpProvider, jwtInterceptorProvider, localStorageServiceProvider) {
        localStorageServiceProvider.setNotify(true, true);
        jwtInterceptorProvider.tokenGetter = ['jwtHelper', '$http', 'localStorageService', 'Auth', '$q', function (jwtHelper, $http, localStorageService, Auth, $q) {
           console.log('tokenGetter started');
           return Auth.getAccessTokenWithRefresh().then(function (token) {
                console.log('tokenGetter success: ' + token);
                return token;
            }, function (err) {
                console.log('tokenGetter error: ' + err);
                return $q.reject(err);
            });
        }];

        $httpProvider.interceptors.push('jwtInterceptor');
    }]).constant('ngAuthSettings', {
        apiServiceBaseUri: '',
        clientId: 'myPublicClient', 
    }).run(["$rootScope", "$location", 'Auth', function ($rootScope, $location,Auth) {
        $rootScope.$on("unauthenticated", function (userInfo) {
            
            var url = $location.url();
            $location.path("/login").search({ 'returnUrl': url });
        });
        $rootScope.$on("forbidden", function (userInfo) {

            var url = $location.url();
            $location.path("/forbidden");
        });
       
        $rootScope.$on('LocalStorageModule.notification.setitem', function (scope,data) {
            if(data.key == 'authData')
                Auth.changeAuthData();
            
        });
        $rootScope.$on('LocalStorageModule.notification.removeitem', function (scope,data) {
            if (data.key == 'authData')
                Auth.changeAuthData();
            
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