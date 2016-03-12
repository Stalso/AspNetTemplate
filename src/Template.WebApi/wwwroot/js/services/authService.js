(function () {
    'use strict';

    angular
        .module('authService', [])
        .factory('Auth', ['$http', '$location', '$route', 'localStorageService','jwtHelper','$q', function ($http, $location, $route, localStorageService,jwtHelper,$q) {           

            var service = {
                //userData:{},
                getToken: getToken,
                refreshToken: refreshToken,
                register: register,
                logoff: logoff,
                isAuthorized: isAuthorized,
                getAuthData: getAuthData,
                getAccessToken: getAccessToken,
                getAccessTokenWithRefresh: getAccessTokenWithRefresh,
                setAuthData: setAuthData,
                clearAuthData: clearAuthData,
                changeAuthData: changeAuthData,
                getAuthDataWithRefresh: getAuthDataWithRefresh

            };          

            return service;

            // login action. Gets access_token
            function getToken(data) {
                console.log("login getToken enetered")
                clearAuthData();

                var postdata = 'grant_type=password&username=' + data.username + '&password=' + data.password + '&client_id=myPublicClient&scope=offline_access roles profile';
                
                return $http.post('/token', postdata, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' }, skipAuthorization: true }).then(function (res) {
                    console.log('login getToken server response' + res.data);
                    if (res && res.data) {
                        setAuthData(res.data);
                    }
                    return res;
                }, function (err) {
                    console.log(err);
                    return $q.reject(err);
                });

            };

            // gets access token from refresh token
            function refreshToken() {
                
                
                var data = getAuthData();
                if (data && data.refresh_token) {
                    var postdata = 'grant_type=refresh_token&refresh_token=' + data.refresh_token + '&client_id=myPublicClient';

                    return $http.post('/token', postdata, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' }, skipAuthorization: true }).then(function (res) {
                        if (res && res.data) {
                            console.log('refresh setted');
                            setAuthData(res.data);

                        }
                        else {
                            clearAuthData();
                        }
                        console.log('refreshToken finished');
                        return res;
                    }, function (err) {
                        clearAuthData();
                        return err;
                    });
                }
                else
                    //return $q.reject('data is empty or no token');
                    return data;
            };

            // registers NewUser
            function register(postdata) {
                return $http.post('/api/account', postdata, { skipAuthorization: true }).then(function (res) {                  
                    return res;
                }, function (err) {
                    return err;
                });
            };

            // logoff action
            function logoff() {
               
                if (localStorageService.get('authData')) {
                    clearAuthData();                    
                    $route.reload()

                }
            };
            function changeAuthData(data)
            {
                //service.userData = getAuthDataWithRefresh();
                getAuthDataWithRefresh().then(function (res) {
                    service.userData = res;
                }, function (err) {
                    //service.userdata = {};
                    service.userData = null;
                });
            }
            // Check Auth
            function isAuthorized() {
                
              
                if (getAccessTokenWithRefresh()) {
                    return true;
                }
                else {
                    return false;
                }
              
            };
            /// jwtSection

            // Get auth data from local storage
            function getAuthData() {
                return localStorageService.get('authData');
            };

            // Get auth data from local storage with refresh
            function getAuthDataWithRefresh() {
                
              
                var deffered = $q.defer();
                var data = getAuthData();
                if (data) {
                    if (data.refresh_token) {
                        var access_token = data.access_token;
                        if (access_token && !jwtHelper.isTokenExpired(access_token)) {
                            deffered.resolve(data);
                            console.log('getAuthDataWithRefresh: good access token' + data.access_token);
                            return deffered.promise;
                        }
                        else {
                            //var data = res.data;

                            return refreshToken().then(function (res) {
                                //return res.data;
                                console.log('getAuthDataWithRefresh return');
                                var auth = getAuthData();
                                console.log('getAuthDataWithRefresh: good access token after token refresh' + auth);
                                return auth;

                            }, function (err) {
                                console.log('getAuthDataWithRefresh: bad access token after token refresh' + err);
                                return err;
                            });
                        }
                    }

                    else {
                        deffered.reject('No refresh token');
                        console.log('getAuthDataWithRefresh error: No refresh token');
                        return deffered.promise;
                    }
                    //    data.refresh_token;
                }
                else {
                    deffered.reject('No auth data token');
                    console.log('getAuthDataWithRefresh error: No auth data');
                    return deffered.promise;
                }
                //    data;
              
            };

            // Get access_token from local storage
            function getAccessToken() {
                var data = getAuthData();
                if (data) {
                    return data.access_token;
                }
                else
                    return data;
            };

            // Get access_token from local storage and refreshes it if needed
            function getAccessTokenWithRefresh() {
                console.log('getAccessTokenWithRefresh started: ');
                return getAuthDataWithRefresh().then(function (data) {
                    console.log('getAccessTokenWithRefresh success: ' + data.access_token)
                    return data.access_token;
                }, function (err) {
                    console.log('getAccessTokenWithRefresh error: ' + err)
                    return $q.reject(err);
                   //return err;
               });            
            };

            // Set auth data to local storage
            function setAuthData(data) {

                if (data && data.access_token && data.refresh_token) {
                    var tokenInfo = jwtHelper.decodeToken(data.access_token);
                    var userdata = {
                        access_token: data.access_token,
                        refresh_token: data.refresh_token,
                        username: tokenInfo.unique_name,

                    };
                    console.log('setAuthData: present data set' + userdata);
                    localStorageService.set('authData', userdata);
                }
                else {
                    console.log('setAuthData: No data to set' + userdata);
                    clearAuthData();
                }
            };

            // Clear auth data from local storage
            function clearAuthData() {

                console.log('clearAuthData enetered');
                localStorageService.remove('authData');
                console.log('clearAuthData finished');
                
            };           

        }]);

})();