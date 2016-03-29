(function () {
    'use strict';

    angular
        .module('c-SignalRService', ['c-SignalR'])
        .factory('TemplateConnection', ['$http', 'HubConnection', '$rootScope', 'Auth', '$q', '$timeout', function factory($http, HubConnection, $rootScope, Auth, $q, $timeout) {

            var bad_token = 'eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IjFQREdZV084R0JVOUtKQjBBME04R0dBX0NJSjRBM0cxQ19JTllPVTkifQ.eyJuYmYiOjE0NTgyODA4MTIsImV4cCI6MTQ1ODI4MDgxNSwiaWF0IjoxNDU4MjgwODEyLCJ1bmlxdWVfbmFtZSI6ImFkbWluIiwicm9sZSI6IkFkbWluIiwiQXNwTmV0LklkZW50aXR5LlNlY3VyaXR5U3RhbXAiOiIxMzNmNDRkMi03YTkxLTQyYjEtODY0YS00MDcwNzY1NDIwZjMiLCJ1c2FnZSI6ImFjY2Vzc190b2tlbiIsImNvbmZpZGVudGlhbCI6dHJ1ZSwic2NvcGUiOlsib2ZmbGluZV9hY2Nlc3MiLCJyb2xlcyIsInByb2ZpbGUiXSwic3ViIjoiYWYwNjk4NTEtMGNlZi00NGI1LWIyOTctYzQ3NTY5NzFlNzkyIiwiYXVkIjpbImh0dHA6Ly9sb2NhbGhvc3Q6MTA0NTAvIiwiaHR0cDovL2xvY2FsaG9zdDoxMDM3Ny8iXSwiYXpwIjoibXlQdWJsaWNDbGllbnQiLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjEwNDUwLyJ9.GAaaaZkSNKN5f1iAFo2auTXXRU2bukP1rpfmV-NGeHgQYckji-QIOcxv4JyAQsLCRWXuS0TZ3eSq8RrmI1anUTuPsdj15S71PWFs501UpUu5ouyuVEAvl0lMnyF7irj-rQiw5z6gUpQDGDXKJWMyJ4C0MJDyyEa9HQbGUF8YOctBTIyzCLsZZbAQcTHZQ4F7FW04y3CVdwiiGw80GHrAELw9Z-T-AeT57M7ylJL6F6MpaCJT84BWbcC8SgbWysb1-my8BTvVB345ylFbVOaISGyVIXiFn0LAckrKc2gH0oGLtk3Z0h7PXC1mEcsEsAF1y0l_xCx4wEUauV6Tz8m_4A';
            var admin_token = 'eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IjFQREdZV084R0JVOUtKQjBBME04R0dBX0NJSjRBM0cxQ19JTllPVTkifQ.eyJuYmYiOjE0NTg1NDQ2NDgsImV4cCI6MTQ1OTQ0NTI1OCwiaWF0IjoxNDU4NTQ0NjQ4LCJ1bmlxdWVfbmFtZSI6ImFkbWluIiwicm9sZSI6IkFkbWluIiwiQXNwTmV0LklkZW50aXR5LlNlY3VyaXR5U3RhbXAiOiI2ZjllN2ZkYy1mZTcxLTQ3MjYtOTk2ZS03NmE3NWVkMzYxYjciLCJ1c2FnZSI6ImFjY2Vzc190b2tlbiIsImNvbmZpZGVudGlhbCI6dHJ1ZSwic2NvcGUiOlsib2ZmbGluZV9hY2Nlc3MiLCJyb2xlcyIsInByb2ZpbGUiXSwic3ViIjoiMGVlMzMxM2YtMzlmMS00YWQ5LWE4NDYtODRhMDk4ZWQ5OGM3IiwiYXVkIjpbImh0dHA6Ly9sb2NhbGhvc3Q6MTA0NTAvIiwiaHR0cDovL2xvY2FsaG9zdDoxMDM3Ny8iXSwiYXpwIjoibXlQdWJsaWNDbGllbnQiLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjEwNDUwLyJ9.dXeqUQHqC3WsymTCT3Me4BgN4_a8koIyTofq7wk8N2pvefNUty5xkCRslfjRt6p1a2CCauDO5L_zGy5vwdegXe4iHAUq7XrxzXiFPN3kbBC6XiAjAE3EpEYfzkQkRyJDRf6P_y3nedasuAzCdjCfb1tyhnGJwsY-z0oRDQPiKbBh1egJ5Q87yV97SF-CtAJsjUrjAwMLTNHLdR-RTtLdVVKiBu6pdfE2ivOP6uajMK7VNVCcdD9v_1iaeSSRcE3lARVGoQT2msy7g-fg7AI11dxRYOW0IM_4qwLDuFAjjGPJANMCEh4aDEZ8Hkb51QCjA1Hsv6eV2Csr8hCJBCjz8w';
            var user_token = 'eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IjFQREdZV084R0JVOUtKQjBBME04R0dBX0NJSjRBM0cxQ19JTllPVTkifQ.eyJuYmYiOjE0NTg4Mjc0NzQsImV4cCI6MTQ1OTcyODA4NCwiaWF0IjoxNDU4ODI3NDc0LCJ1bmlxdWVfbmFtZSI6Im5ld1VzZXIiLCJBc3BOZXQuSWRlbnRpdHkuU2VjdXJpdHlTdGFtcCI6ImFmNzljYzFjLWRkZGMtNDg3YS1iNTQ1LTM0ZDViZGNkNDBmZiIsInVzYWdlIjoiYWNjZXNzX3Rva2VuIiwiY29uZmlkZW50aWFsIjp0cnVlLCJzY29wZSI6WyJvZmZsaW5lX2FjY2VzcyIsInJvbGVzIiwicHJvZmlsZSJdLCJzdWIiOiIyZTQzNDc4Yi0zMTQxLTRjZmMtYmM2NS02ODhjZmMyNDRkYjEiLCJhdWQiOlsiaHR0cDovL2xvY2FsaG9zdDoxMDQ1MC8iLCJodHRwOi8vbG9jYWxob3N0OjEwMzc3LyJdLCJhenAiOiJteVB1YmxpY0NsaWVudCIsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6MTA0NTAvIn0.owPRw395LTqa_SgQzSmK7DARLBnPYUDCCezg2tZD8ifBVVMZBoxHjkJykumfRQ35xo32lVbO90DZYX6RrCtt92DSBehVkpn8sGqJhSUI6YlFTKyjab_lf-nAs5xugx1O3_UdFxED4EXZ0vXjbZZIDTmzAmUhNURXnoc2NUrJuN6IMUuvrTsfLUl1-OWCCBDeDLBLTAob1-N4dM8pzYH9HbfNZgu7OJmyWSO2ZvRZWAD4R7YDEGCXfoLsRq4fiJSeb1ZJjJTKO8Mn2PD3LFNWv5atL5pa-jX9GIGStbh2_wbMpv3SnPeWimml4VmcKX4EpwFa7pdsnuT1XdsnybH4KA';

            var hubConnection = new HubConnection({

                autoConnect: false,



                errorHandler: function (error, data) {
                    if (error.context.status === 500) {
                        console.log(error.context.status + ' error from connection');
                        if (getHub500Reconnect()) {
                            setHub500Reconnect(false);
                            //real
                            this.getToken();

                            //good
                            //changeToken();

                            //bad
                            //changeOnBadToken();

                        }
                        else {
                            setHub500Reconnect(true);
                        }

                    }
                },
                queryParams: {
                    'testQueryParam': 'testQueryParamValue'
                },

                stateChanged: function (state) {
                    switch (state.newState) {
                        case $.signalR.connectionState.connecting:
                            console.log("connecting from connection");
                            break;
                        case $.signalR.connectionState.connected:
                            console.log("connected  from connection");
                            setHub500Reconnect(true);
                            break;
                        case $.signalR.connectionState.reconnecting:
                            console.log("reconnecting from connection");
                            break;
                        case $.signalR.connectionState.disconnected:
                            console.log("disconnected from connection");
                            break;
                    }
                },

                securityTokenOptions: {

                    getToken: function () {
                        // real
                        var res = Auth.getAccessTokenWithRefresh().then(function (token) {
                            return token
                        }, function (err) {
                            return null;
                        });

                        //expired
                        //return $q.when(bad_token);

                        //valid admin
                        //var res = $q.when(admin_token);

                        //valid user
                        //var res = $q.when(user_token);

                        return res;
                    },

                    getConnectionToken: function () {
                        if (this.qs) {
                            return this.qs.access_token;
                        }
                        else
                            return null;
                    },

                    setConnectionToken: function (token) {
                        if (this.qs) {
                            if (token)
                                this.qs.access_token = token;
                            else {
                                delete this.qs.access_token;
                            }
                        }
                        else {
                            if (token) {
                                this.qs = {
                                    access_token: token
                                };
                            }
                        }
                    },

                    unAuthHandler: function () {
                        console.log('401 from connection');
                        $rootScope.$broadcast('unauthenticated');
                    },

                    forbiddenHandler: function () {
                        console.log('403 from connection');
                        $rootScope.$broadcast('forbidden');
                    },

                    tokenChangedLogic: function () {

                        return reestablishConnection();
                    },

                },

                hubs: [{
                    hubName: 'msg',
                    options: {
                        // Server Side methods
                        methods: ['SendFreeMessage', 'SendProtMessage', 'SendAdminMessage'],

                        // Methods, which can be called from the server
                        listeners: {
                            'sendFreeMessage': function (data) {
                                $rootScope.$apply(function () {
                                    service.sendFreeMessage(data);
                                });
                            },
                            'sendProtectedMessage': function (data) {
                                $rootScope.$apply(function () {
                                    service.sendProtectedMessage(data);
                                });
                            },
                            'sendAdminMessage': function (data) {
                                $rootScope.$apply(function () {
                                    service.sendAdminMessage(data);
                                });
                            },
                        },

                        stateChanged: function (state) {
                            switch (state.newState) {
                                case $.signalR.connectionState.connecting:
                                    console.log("msg connecting");
                                    break;
                                case $.signalR.connectionState.connected:
                                    console.log("msg connected");
                                    break;
                                case $.signalR.connectionState.reconnecting:
                                    console.log("msg reconnecting");
                                    break;
                                case $.signalR.connectionState.disconnected:
                                    console.log("msg disconnected");
                                    break;
                            }
                        },

                    }
                }, {
                    hubName: 'prot',
                    options: {
                        methods: ['SendFreeMessage', 'SendProtMessage', 'SendAdminMessage'],

                        listeners: {
                            'sendFreeMessage': function (data) {
                                $rootScope.$apply(function () {
                                    service.sendPrFreeMessage(data);
                                });
                            },
                            'sendProtectedMessage': function (data) {
                                $rootScope.$apply(function () {
                                    service.sendPrProtectedMessage(data);
                                });
                            },
                            'sendAdminMessage': function (data) {
                                $rootScope.$apply(function () {
                                    service.sendPrAdminMessage(data);
                                });
                            },
                        },

                        stateChanged: function (state) {
                            switch (state.newState) {
                                case $.signalR.connectionState.connecting:
                                    console.log("prot connecting");
                                    break;
                                case $.signalR.connectionState.connected:
                                    console.log("prot connected");
                                    break;
                                case $.signalR.connectionState.reconnecting:
                                    console.log("prot reconnecting");
                                    break;
                                case $.signalR.connectionState.disconnected:
                                    console.log("prot disconnected");
                                    break;
                            }
                        }
                    }
                }, {
                    hubName: 'admin',
                    options: {
                        methods: ['SendFreeMessage', 'SendProtMessage', 'SendAdminMessage'],

                        listeners: {
                            'sendFreeMessage': function (data) {
                                $rootScope.$apply(function () {
                                    service.sendAdFreeMessage(data);
                                });
                            },
                            'sendProtectedMessage': function (data) {
                                $rootScope.$apply(function () {
                                    service.sendAdProtectedMessage(data);
                                });
                            },
                            'sendAdminMessage': function (data) {
                                $rootScope.$apply(function () {
                                    service.sendAdAdminMessage(data);
                                });
                            },
                        },

                        stateChanged: function (state) {
                            switch (state.newState) {
                                case $.signalR.connectionState.connecting:
                                    console.log("admin connecting");
                                    break;
                                case $.signalR.connectionState.connected:
                                    console.log("admin connected");
                                    break;
                                case $.signalR.connectionState.reconnecting:
                                    console.log("admin reconnecting");
                                    break;
                                case $.signalR.connectionState.disconnected:
                                    console.log("admin disconnected");
                                    break;
                            }
                        }
                    }
                }]
            });

            //$rootScope.$on("test_access_token changed", function (userInfo) {
            //    console.log('test_access_token changed event');
            //    hubConnection.tokenChangedEventHandler();

            //});
            $rootScope.$on("access_token changed", function (userInfo) {
                console.log('access_token changed event');
                if(hubConnection.state() === 1)
                    hubConnection.tokenChangedEventHandler();

            });
            hubConnection.reconnectOn500 = true;
            var hub = hubConnection.hubs['msg'];
            var protHub = hubConnection.hubs['prot'];
            var adminHub = hubConnection.hubs['admin'];

            function getHub500Reconnect() {
                return hubConnection.reconnectOn500;
            }
            function setHub500Reconnect(value) {
                hubConnection.reconnectOn500 = value;
            }

            var service = {

                connection: hubConnection,

                hub: hub,
                protHub: protHub,
                adminHub: adminHub,

                //dropConnection: dropConnection,
                //establishConnection: establishConnection,

                sendFreeMessage: function () { throw "No handler assigned for `sendFreeMessage`"; },
                sendProtectedMessage: function () { throw "No handler assigned for `sendProtectedMessage`"; },
                sendAdminMessage: function () { throw "No handler assigned for `sendAdminMessage`"; },

                sendPrFreeMessage: function () { throw "No handler assigned for `sendPrFreeMessage`"; },
                sendPrProtectedMessage: function () { throw "No handler assigned for `sendPrProtectedMessage`"; },
                sendPrAdminMessage: function () { throw "No handler assigned for `sendPrAdminMessage`"; },

                sendAdFreeMessage: function () { throw "No handler assigned for `sendAdFreeMessage`"; },
                sendAdProtectedMessage: function () { throw "No handler assigned for `sendAdProtectedMessage`"; },
                sendAdAdminMessage: function () { throw "No handler assigned for `sendAdAdminMessage`"; },

                changeToken: changeToken,
                changeOnUserToken: changeOnUserToken,
                changeOnBadToken: changeOnBadToken,
                clearToken: clearToken,
                rejectToken: rejectToken,

                changeTokenArt: changeTokenArt,
                changeOnUserTokenArt: changeOnUserTokenArt,
                changeOnBadTokenArt: changeOnBadTokenArt,
                clearTokenArt: clearTokenArt,
                rejectTokenArt: rejectTokenArt,

                changeQsToken: changeQsToken,
                changeQsOnUserToken: changeQsOnUserToken,
                changeQsOnBadToken: changeQsOnBadToken,
                clearQsToken: clearQsToken,

            };

            return service;




            function reestablishConnection() {
                service.connection.disconnect();
                return $timeout(service.connection.connect(), 1);
            }



            ////////////// changeToken with notification
            function changeToken(skipNotification) {

                hubConnection.connection.getToken = function () {
                    return $q.when(admin_token);
                }
                console.log('qood token set');
                if (!skipNotification)
                    $rootScope.$broadcast('test_access_token changed');
            };

            function changeOnUserToken(skipNotification) {

                hubConnection.connection.getToken = function () {
                    return $q.when(user_token);
                }
                console.log('user token set');
                if (!skipNotification)
                    $rootScope.$broadcast('test_access_token changed');
            };

            function changeOnBadToken(skipNotification) {

                hubConnection.connection.getToken = function () {
                    return $q.when(bad_token);
                }
                console.log('bad token set');
                if (!skipNotification)
                    $rootScope.$broadcast('test_access_token changed');
            };

            function clearToken(skipNotification) {

                hubConnection.connection.getToken = function () {
                    return $q.when('');
                }
                console.log('no query token set');
                if (!skipNotification)
                    $rootScope.$broadcast('test_access_token changed');
            };
            function rejectToken(skipNotification) {

                hubConnection.connection.getToken = function () {
                    var deffered = $q.defer();
                    deffered.reject(null);
                    return deffered.promise;
                }
                console.log('reject token set');
                if (!skipNotification)
                    $rootScope.$broadcast('test_access_token changed');

            }
            ////////////////////////////////////////////////

            ////////////// changeToken without notification
            function changeTokenArt() {

                console.log('good token without notification set');
                return changeToken(true);
            };
            function changeOnUserTokenArt() {

                console.log('user token without notification set');
                return changeOnUserToken(true);
            };
            function changeOnBadTokenArt() {

                console.log('bad token without notification set');
                return changeOnBadToken(true);
            };

            function clearTokenArt() {

                console.log('clear token without notification set');
                return clearToken(true);
            };
            function rejectTokenArt() {
               
                console.log('reject token without notification set');
                return rejectToken(true);
            }
            ///////////////////////////////////////////////

            ////////////// changeToken in qs
            function changeQsToken() {

                console.log('good token in qs set');
                hubConnection.connection.setConnectionToken(admin_token);

            };
            function changeQsOnUserToken() {

                console.log('user token in qs set');
                hubConnection.connection.setConnectionToken(user_token);
            };
            function changeQsOnBadToken() {

                console.log('bad token in qs set');
                hubConnection.connection.setConnectionToken(bad_token);
            };
            function clearQsToken() {

                console.log('clear token in qs set');
                hubConnection.connection.setConnectionToken('');
            };

        }]);
})();