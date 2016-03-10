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
        jwtInterceptorProvider.tokenGetter = ['jwtHelper', '$http', 'localStorageService', 'Auth', function (jwtHelper, $http, localStorageService, Auth)
        {
            var accessToken = localStorageService.get('access_token');
            if (jwtHelper.isTokenExpired(accessToken)) {
                Auth.refreshToken().then(function (res) {
                    return res.data;
                });
            }
            else
                return accessToken;
            //var refreshToken = localStorage.getItem('refresh_token');
            //if (jwtHelper.isTokenExpired(accessToken)) {
            //    // This is a promise of a JWT id_token
            //    return $http({
            //        url: '/delegation',
            //        // This makes it so that this request doesn't send the JWT
            //        skipAuthorization: true,
            //        method: 'POST',
            //        data: {
            //            grant_type: 'refresh_token',
            //            refresh_token: refreshToken,

            //        }
            //    }).then(function (response) {
            //        var id_token = response.data.id_token;
            //        localStorage.setItem('id_token', id_token);
            //        return id_token;
            //    });
            //}
            console.log("token Getter works");
        }],
        $httpProvider.interceptors.push('jwtInterceptor');
    }]);
})();