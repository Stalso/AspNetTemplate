(function () {
    'use strict';

    angular.module('angular-jwt.interceptor', [
        // Angular modules 


        // Custom modules 

        // 3rd Party Modules

    ]).provider('jwtInterceptor', function () {
        //this.$get = angular.noop;

        this.urlParam = null;
        this.authHeader = 'Authorization';
        this.authPrefix = 'Bearer ';
        this.tokenGetter = function () {
            return null;
        }

        var config = this;

        this.$get = function ($q, $injector, $rootScope) {
            return {
                request: function (request) {
                    if (request.skipAuthorization) {
                        return request;
                    }

                    if (config.urlParam) {
                        request.params = request.params || {};
                        // Already has the token in the url itself
                        if (request.params[config.urlParam]) {
                            return request;
                        }
                    } else {
                        request.headers = request.headers || {};
                        // Already has an Authorization header
                        if (request.headers[config.authHeader]) {
                            return request;
                        }
                    }
                    var tokenPromise = $q.when($injector.invoke(config.tokenGetter, this, {
                        config: request
                    }));

                    return tokenPromise.then(function (token) {
                        console.log('token promise');
                        if (token) {
                            if (config.urlParam) {
                                request.params[config.urlParam] = token;
                            } else {
                                request.headers[config.authHeader] = config.authPrefix + token;
                            }
                        }
                        console.log('token');
                        return request;
                    });
                },
                responseError: function (rejection) {
                    // handle the case where the user is not authenticated
                    if (rejection.status === 401) {
                        $rootScope.$broadcast('unauthenticated', rejection);
                    }
                    return $q.reject(rejection);
                }
            };
        };
    });
})();