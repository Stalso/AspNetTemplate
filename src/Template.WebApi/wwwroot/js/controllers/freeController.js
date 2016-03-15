    (function () {
        'use strict';

        angular
            .module('freeCtrl',[])
            .controller('freeController', ['$location','Free', function ($location,Free) {
                var vm = this;
                vm.title = 'freeController';
                Free.getData().then(function (res) {
                    vm.values = res.data;
                }, function (err) {
                    console.log(err);
                });

                vm.select = function () {
                    if (vm.values)
                    {
                        var selecteditems = vm.values.filter(function (item) {
                            return item.isSelected;
                        });
                        if (selecteditems && ClientDataCollector)
                        {
                            var json = angular.toJson(selecteditems, false);
                            ClientDataCollector.addMany(json);
                        }
                    }
                }
            }]);
    })();
