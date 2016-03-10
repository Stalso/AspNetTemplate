﻿(function () {
    'use strict';

    angular
        .module('authService', [])
        .factory('Auth', ['$http', 'localStorageService', function ($http, localStorageService) {

            //var clientId = 'myPublicClient';
            //var data = "grant_type=password&username=admin&password=admin&client_id=myPublicClient&scope=offline_access";

            var service = {
                getToken: getToken,
                refreshToken: refreshToken
            };

          

            return service;

            function getToken(data) {
                var postdata = 'grant_type=password&username=' + data.username + '&password=' + data.password + '&client_id=myPublicClient&scope=offline_access roles profile';

                return $http.post('/token', postdata, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' }, skipAuthorization: true }).then(function (res) {
                    if (res && res.data) {
                        localStorageService.set('access_token', res.data.access_token);
                        localStorageService.set('refresh_token', res.data.refresh_token);
                    }
                    return res;
                }, function (err) {
                    return err;
                });
            };
            function refreshToken() {
                //var access_token = localStorageService.get('access_token', res.data.access_token);
                var refresh_token = localStorageService.get('refresh_token');
                var postdata = 'grant_type=refresh_token&refresh_token=' + refresh_token + '&client_id=myPublicClient';

                return $http.post('/token', postdata, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' }, skipAuthorization: true }).then(function (res) {
                    if (res && res.data) {
                        localStorageService.set('access_token', res.data.access_token);
                        localStorageService.set('refresh_token', res.data.refresh_token);
                    }
                    return res;
                }, function (err) {
                    return err;
                });
            };
        }]);

})();