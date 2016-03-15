(function () {
    'use strict';

    angular
        .module('sampleEntsListDirective', [])
        .directive('sampleEntsList', ['$window', function ($window) {
            // Usage:
            //     <sampleEntsList></sampleEntsList>
            // Creates:
            // 
            var directive = {
                link: link,
                restrict: 'AE',
                templateUrl: 'js/directives/sampleEntsList/views/sampleEntsList.html',
                scope: {
                    stockData: '='
                }
                //template: 'hui'
            };
            return directive;

            function link(scope, element, attrs) {
                //scope.text = "textFrom Dir";
            }
        }]);

})();