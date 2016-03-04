(function () {
    'use strict';

    angular.module('routes', [
        // Angular modules 
        'ngRoute',

        // Custom modules 
        
        // 3rd Party Modules
        
    ]).config([
        // Angular modules 
        '$routeProvider',
        '$locationProvider',

        function ($routeProvider, $locationProvider) {
            $routeProvider
            // home page
            .when('/', {
                templateUrl: 'views/home.html',
                controller: 'homeController as controller'
            })
            .when('/home', {
                templateUrl: 'views/home.html',
                controller: 'homeController as controller'
            })
            .when('/free', {
                templateUrl: 'views/free.html',
                controller: 'freeController as controller'
            })
            .when('/prot', {
                templateUrl: 'views/prot.html',
                controller: 'protController as controller',
            }).when('/token', {
                templateUrl: 'views/token.html',
                controller: 'tokenController as controller',
            }).otherwise({ redirectTo: "/home" });
           
            $locationProvider.html5Mode(true);
        }
    ]);
})();