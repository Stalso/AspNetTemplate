(function () {
    'use strict';

    angular
        .module('SignalRService', ['SignalR'])
        .factory('TemplateHub', ['$http', 'Hub', '$rootScope', 'Auth', function factory($http, Hub, $rootScope, Auth) {
          
            //var token1 = 'eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IjFQREdZV084R0JVOUtKQjBBME04R0dBX0NJSjRBM0cxQ19JTllPVTkifQ.eyJuYmYiOjE0NTg1NDQ2NDgsImV4cCI6MTQ1OTQ0NTI1OCwiaWF0IjoxNDU4NTQ0NjQ4LCJ1bmlxdWVfbmFtZSI6ImFkbWluIiwicm9sZSI6IkFkbWluIiwiQXNwTmV0LklkZW50aXR5LlNlY3VyaXR5U3RhbXAiOiI2ZjllN2ZkYy1mZTcxLTQ3MjYtOTk2ZS03NmE3NWVkMzYxYjciLCJ1c2FnZSI6ImFjY2Vzc190b2tlbiIsImNvbmZpZGVudGlhbCI6dHJ1ZSwic2NvcGUiOlsib2ZmbGluZV9hY2Nlc3MiLCJyb2xlcyIsInByb2ZpbGUiXSwic3ViIjoiMGVlMzMxM2YtMzlmMS00YWQ5LWE4NDYtODRhMDk4ZWQ5OGM3IiwiYXVkIjpbImh0dHA6Ly9sb2NhbGhvc3Q6MTA0NTAvIiwiaHR0cDovL2xvY2FsaG9zdDoxMDM3Ny8iXSwiYXpwIjoibXlQdWJsaWNDbGllbnQiLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjEwNDUwLyJ9.dXeqUQHqC3WsymTCT3Me4BgN4_a8koIyTofq7wk8N2pvefNUty5xkCRslfjRt6p1a2CCauDO5L_zGy5vwdegXe4iHAUq7XrxzXiFPN3kbBC6XiAjAE3EpEYfzkQkRyJDRf6P_y3nedasuAzCdjCfb1tyhnGJwsY-z0oRDQPiKbBh1egJ5Q87yV97SF-CtAJsjUrjAwMLTNHLdR-RTtLdVVKiBu6pdfE2ivOP6uajMK7VNVCcdD9v_1iaeSSRcE3lARVGoQT2msy7g-fg7AI11dxRYOW0IM_4qwLDuFAjjGPJANMCEh4aDEZ8Hkb51QCjA1Hsv6eV2Csr8hCJBCjz8w';
            //var token1 = 'eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IjFQREdZV084R0JVOUtKQjBBME04R0dBX0NJSjRBM0cxQ19JTllPVTkifQ.eyJuYmYiOjE0NTg0NzAyMTEsImV4cCI6MTQ1ODQ3MzgxMSwiaWF0IjoxNDU4NDcwMjExLCJ1bmlxdWVfbmFtZSI6ImFkbWluIiwicm9sZSI6IkFkbWluIiwiQXNwTmV0LklkZW50aXR5LlNlY3VyaXR5U3RhbXAiOiI1NGIxMTk5Mi1hZDgxLTQ4NzAtOWU0Zi0wN2M3MzAyNmUwYzUiLCJ1c2FnZSI6ImFjY2Vzc190b2tlbiIsImNvbmZpZGVudGlhbCI6dHJ1ZSwic2NvcGUiOlsib2ZmbGluZV9hY2Nlc3MiLCJyb2xlcyIsInByb2ZpbGUiXSwic3ViIjoiYTI0NzE0NzktMjQxMy00ZmU2LWJmMzctMjhiMWJhN2I3ODljIiwiYXVkIjpbImh0dHA6Ly9sb2NhbGhvc3Q6MTA0NTAvIiwiaHR0cDovL2xvY2FsaG9zdDoxMDM3Ny8iXSwiYXpwIjoibXlQdWJsaWNDbGllbnQiLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjEwNDUwLyJ9.cvqe41ToM5fAkt-lXQTkjhG__JvRIFp53b045Wf3LJBgyAb_0Q1t4spxrUwc4AJDaox1KNjqoZR7b7eQ_cPfHRwOxUOQACJXghaxCnA5VWCs6avw8u3BFHuqSDQH6gGTvv2BWpvpt_SLV--FpUfOSntRb9KY4XcKFdZMsYMA1Nt05myZusnEZU62kspLk-zdUsb_Hv7Eu8OxlcvuphDrXVRMqWTNXTh-Z_Q9XOzrNZn7O5xFdPpDn4GzOYiCi1cI2r7Q8zuJiqZ6Wz2TMJuBDDhGeoxwD87agbnTkegSc_IHnrYeM8DMOiNsqocs2L64ehNcXTh5b07DIC7l6a2NQQ';
            $.ajaxSetup({
                //beforeSend: function (xhr, settings) {
                //    console.log(xhr);
                //    //settings.url = settings.url + "&access_token=" + token1;
                //    settings.url = settings.url + "&access_token=" + token1;

                //},
                //error: function (xhr, stat, errorThrown) {
                //    console.log('ajax error' + errorThrown);
                //},
                complete: function (jqXHR, textStatus)
                {
                    console.log('ajax complete: ' + textStatus);
                    if (jqXHR.status === 500)
                    {
                        console.log('500 error');
                        setTimeout(function () {
                            changeQueryToken();
                            establishConnection();
                        }, 1);
                    }
                    
                },
                success: function (jqXHR, textStatus, jqXHR1 ) {
                    console.log('ajax success' + jqXHR1);
                },
            });

            var hub = new Hub('msg', {
                
                // Server Side methods
                methods: ['SendFreeMessage', 'SendProtMessage'],
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
                    }
                },
                
                errorHandler: function (error,data) {
                    console.log('CathcedError ' + error);
                     
                },
                queryParams: {

                   //expired
                   //'access_token': 'eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IjFQREdZV084R0JVOUtKQjBBME04R0dBX0NJSjRBM0cxQ19JTllPVTkifQ.eyJuYmYiOjE0NTgyODA4MTIsImV4cCI6MTQ1ODI4MDgxNSwiaWF0IjoxNDU4MjgwODEyLCJ1bmlxdWVfbmFtZSI6ImFkbWluIiwicm9sZSI6IkFkbWluIiwiQXNwTmV0LklkZW50aXR5LlNlY3VyaXR5U3RhbXAiOiIxMzNmNDRkMi03YTkxLTQyYjEtODY0YS00MDcwNzY1NDIwZjMiLCJ1c2FnZSI6ImFjY2Vzc190b2tlbiIsImNvbmZpZGVudGlhbCI6dHJ1ZSwic2NvcGUiOlsib2ZmbGluZV9hY2Nlc3MiLCJyb2xlcyIsInByb2ZpbGUiXSwic3ViIjoiYWYwNjk4NTEtMGNlZi00NGI1LWIyOTctYzQ3NTY5NzFlNzkyIiwiYXVkIjpbImh0dHA6Ly9sb2NhbGhvc3Q6MTA0NTAvIiwiaHR0cDovL2xvY2FsaG9zdDoxMDM3Ny8iXSwiYXpwIjoibXlQdWJsaWNDbGllbnQiLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjEwNDUwLyJ9.GAaaaZkSNKN5f1iAFo2auTXXRU2bukP1rpfmV-NGeHgQYckji-QIOcxv4JyAQsLCRWXuS0TZ3eSq8RrmI1anUTuPsdj15S71PWFs501UpUu5ouyuVEAvl0lMnyF7irj-rQiw5z6gUpQDGDXKJWMyJ4C0MJDyyEa9HQbGUF8YOctBTIyzCLsZZbAQcTHZQ4F7FW04y3CVdwiiGw80GHrAELw9Z-T-AeT57M7ylJL6F6MpaCJT84BWbcC8SgbWysb1-my8BTvVB345ylFbVOaISGyVIXiFn0LAckrKc2gH0oGLtk3Z0h7PXC1mEcsEsAF1y0l_xCx4wEUauV6Tz8m_4A'

                    //valid
                    'access_token': 'eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IjFQREdZV084R0JVOUtKQjBBME04R0dBX0NJSjRBM0cxQ19JTllPVTkifQ.eyJuYmYiOjE0NTg1NDQ2NDgsImV4cCI6MTQ1OTQ0NTI1OCwiaWF0IjoxNDU4NTQ0NjQ4LCJ1bmlxdWVfbmFtZSI6ImFkbWluIiwicm9sZSI6IkFkbWluIiwiQXNwTmV0LklkZW50aXR5LlNlY3VyaXR5U3RhbXAiOiI2ZjllN2ZkYy1mZTcxLTQ3MjYtOTk2ZS03NmE3NWVkMzYxYjciLCJ1c2FnZSI6ImFjY2Vzc190b2tlbiIsImNvbmZpZGVudGlhbCI6dHJ1ZSwic2NvcGUiOlsib2ZmbGluZV9hY2Nlc3MiLCJyb2xlcyIsInByb2ZpbGUiXSwic3ViIjoiMGVlMzMxM2YtMzlmMS00YWQ5LWE4NDYtODRhMDk4ZWQ5OGM3IiwiYXVkIjpbImh0dHA6Ly9sb2NhbGhvc3Q6MTA0NTAvIiwiaHR0cDovL2xvY2FsaG9zdDoxMDM3Ny8iXSwiYXpwIjoibXlQdWJsaWNDbGllbnQiLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjEwNDUwLyJ9.dXeqUQHqC3WsymTCT3Me4BgN4_a8koIyTofq7wk8N2pvefNUty5xkCRslfjRt6p1a2CCauDO5L_zGy5vwdegXe4iHAUq7XrxzXiFPN3kbBC6XiAjAE3EpEYfzkQkRyJDRf6P_y3nedasuAzCdjCfb1tyhnGJwsY-z0oRDQPiKbBh1egJ5Q87yV97SF-CtAJsjUrjAwMLTNHLdR-RTtLdVVKiBu6pdfE2ivOP6uajMK7VNVCcdD9v_1iaeSSRcE3lARVGoQT2msy7g-fg7AI11dxRYOW0IM_4qwLDuFAjjGPJANMCEh4aDEZ8Hkb51QCjA1Hsv6eV2Csr8hCJBCjz8w'
                 },
               

                stateChanged: function (state) {
                    switch (state.newState) {
                        case $.signalR.connectionState.connecting:
                            console.log("connecting");

                            break;
                        case $.signalR.connectionState.connected:
                            console.log("connected");
                            //$rootScope.$apply(function () {
                            //    service.connectionEstablished = true;
                            //});
                          
                            break;
                        case $.signalR.connectionState.reconnecting:
                            console.log("reconnecting");
                            break;
                        case $.signalR.connectionState.disconnected:
                            console.log("disconnected");                          
                            break;
                    }
                },
                transport: 'longPolling'
            });
            //$.connection.hub.logging = true;
            //hub.connection.error = function (error, data) {
            //    console.log('CathcedError1 ' + error);

            //};
           
            var service = {
                connectionEstablished: false,
                hub: hub,
                dropConnection: dropConnection,
                establishConnection: establishConnection,
                sendFreeMessage: function () { throw "No handler assigned for `sendFreeMessage`"; },
                sendProtectedMessage: function () { throw "No handler assigned for `sendProtectedMessage`"; },
                changeToken: changeToken,
                changeOnBadToken: changeOnBadToken,
                changeQueryToken: changeQueryToken,
                changeOnBadQueryToken: changeOnBadQueryToken,
                clearQueryToken: clearQueryToken
            };

           
            return service;
           
            function changeToken() {
                token1 = 'eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IjFQREdZV084R0JVOUtKQjBBME04R0dBX0NJSjRBM0cxQ19JTllPVTkifQ.eyJuYmYiOjE0NTg1NDQ2NDgsImV4cCI6MTQ1OTQ0NTI1OCwiaWF0IjoxNDU4NTQ0NjQ4LCJ1bmlxdWVfbmFtZSI6ImFkbWluIiwicm9sZSI6IkFkbWluIiwiQXNwTmV0LklkZW50aXR5LlNlY3VyaXR5U3RhbXAiOiI2ZjllN2ZkYy1mZTcxLTQ3MjYtOTk2ZS03NmE3NWVkMzYxYjciLCJ1c2FnZSI6ImFjY2Vzc190b2tlbiIsImNvbmZpZGVudGlhbCI6dHJ1ZSwic2NvcGUiOlsib2ZmbGluZV9hY2Nlc3MiLCJyb2xlcyIsInByb2ZpbGUiXSwic3ViIjoiMGVlMzMxM2YtMzlmMS00YWQ5LWE4NDYtODRhMDk4ZWQ5OGM3IiwiYXVkIjpbImh0dHA6Ly9sb2NhbGhvc3Q6MTA0NTAvIiwiaHR0cDovL2xvY2FsaG9zdDoxMDM3Ny8iXSwiYXpwIjoibXlQdWJsaWNDbGllbnQiLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjEwNDUwLyJ9.dXeqUQHqC3WsymTCT3Me4BgN4_a8koIyTofq7wk8N2pvefNUty5xkCRslfjRt6p1a2CCauDO5L_zGy5vwdegXe4iHAUq7XrxzXiFPN3kbBC6XiAjAE3EpEYfzkQkRyJDRf6P_y3nedasuAzCdjCfb1tyhnGJwsY-z0oRDQPiKbBh1egJ5Q87yV97SF-CtAJsjUrjAwMLTNHLdR-RTtLdVVKiBu6pdfE2ivOP6uajMK7VNVCcdD9v_1iaeSSRcE3lARVGoQT2msy7g-fg7AI11dxRYOW0IM_4qwLDuFAjjGPJANMCEh4aDEZ8Hkb51QCjA1Hsv6eV2Csr8hCJBCjz8w';
            };

            function changeOnBadToken() {
                token1 = 'eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IjFQREdZV084R0JVOUtKQjBBME04R0dBX0NJSjRBM0cxQ19JTllPVTkifQ.eyJuYmYiOjE0NTgyODA4MTIsImV4cCI6MTQ1ODI4MDgxNSwiaWF0IjoxNDU4MjgwODEyLCJ1bmlxdWVfbmFtZSI6ImFkbWluIiwicm9sZSI6IkFkbWluIiwiQXNwTmV0LklkZW50aXR5LlNlY3VyaXR5U3RhbXAiOiIxMzNmNDRkMi03YTkxLTQyYjEtODY0YS00MDcwNzY1NDIwZjMiLCJ1c2FnZSI6ImFjY2Vzc190b2tlbiIsImNvbmZpZGVudGlhbCI6dHJ1ZSwic2NvcGUiOlsib2ZmbGluZV9hY2Nlc3MiLCJyb2xlcyIsInByb2ZpbGUiXSwic3ViIjoiYWYwNjk4NTEtMGNlZi00NGI1LWIyOTctYzQ3NTY5NzFlNzkyIiwiYXVkIjpbImh0dHA6Ly9sb2NhbGhvc3Q6MTA0NTAvIiwiaHR0cDovL2xvY2FsaG9zdDoxMDM3Ny8iXSwiYXpwIjoibXlQdWJsaWNDbGllbnQiLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjEwNDUwLyJ9.GAaaaZkSNKN5f1iAFo2auTXXRU2bukP1rpfmV-NGeHgQYckji-QIOcxv4JyAQsLCRWXuS0TZ3eSq8RrmI1anUTuPsdj15S71PWFs501UpUu5ouyuVEAvl0lMnyF7irj-rQiw5z6gUpQDGDXKJWMyJ4C0MJDyyEa9HQbGUF8YOctBTIyzCLsZZbAQcTHZQ4F7FW04y3CVdwiiGw80GHrAELw9Z-T-AeT57M7ylJL6F6MpaCJT84BWbcC8SgbWysb1-my8BTvVB345ylFbVOaISGyVIXiFn0LAckrKc2gH0oGLtk3Z0h7PXC1mEcsEsAF1y0l_xCx4wEUauV6Tz8m_4A';
            };

            function dropConnection() {
                if (service.hub) {
                    hub.disconnect();

                }
            };

            function establishConnection() {
                if (service.hub) {
                    hub.connect();
                }
            };

            function changeQueryToken() {
                if (service.hub) {
                    service.hub.connection.qs = {
                            access_token: 'eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IjFQREdZV084R0JVOUtKQjBBME04R0dBX0NJSjRBM0cxQ19JTllPVTkifQ.eyJuYmYiOjE0NTg1NDQ2NDgsImV4cCI6MTQ1OTQ0NTI1OCwiaWF0IjoxNDU4NTQ0NjQ4LCJ1bmlxdWVfbmFtZSI6ImFkbWluIiwicm9sZSI6IkFkbWluIiwiQXNwTmV0LklkZW50aXR5LlNlY3VyaXR5U3RhbXAiOiI2ZjllN2ZkYy1mZTcxLTQ3MjYtOTk2ZS03NmE3NWVkMzYxYjciLCJ1c2FnZSI6ImFjY2Vzc190b2tlbiIsImNvbmZpZGVudGlhbCI6dHJ1ZSwic2NvcGUiOlsib2ZmbGluZV9hY2Nlc3MiLCJyb2xlcyIsInByb2ZpbGUiXSwic3ViIjoiMGVlMzMxM2YtMzlmMS00YWQ5LWE4NDYtODRhMDk4ZWQ5OGM3IiwiYXVkIjpbImh0dHA6Ly9sb2NhbGhvc3Q6MTA0NTAvIiwiaHR0cDovL2xvY2FsaG9zdDoxMDM3Ny8iXSwiYXpwIjoibXlQdWJsaWNDbGllbnQiLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjEwNDUwLyJ9.dXeqUQHqC3WsymTCT3Me4BgN4_a8koIyTofq7wk8N2pvefNUty5xkCRslfjRt6p1a2CCauDO5L_zGy5vwdegXe4iHAUq7XrxzXiFPN3kbBC6XiAjAE3EpEYfzkQkRyJDRf6P_y3nedasuAzCdjCfb1tyhnGJwsY-z0oRDQPiKbBh1egJ5Q87yV97SF-CtAJsjUrjAwMLTNHLdR-RTtLdVVKiBu6pdfE2ivOP6uajMK7VNVCcdD9v_1iaeSSRcE3lARVGoQT2msy7g-fg7AI11dxRYOW0IM_4qwLDuFAjjGPJANMCEh4aDEZ8Hkb51QCjA1Hsv6eV2Csr8hCJBCjz8w'
                        };
                   
                }
            };

            function changeOnBadQueryToken() {
                if (service.hub) {                   
                    service.hub.connection.qs = {
                        access_token: 'eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IjFQREdZV084R0JVOUtKQjBBME04R0dBX0NJSjRBM0cxQ19JTllPVTkifQ.eyJuYmYiOjE0NTgyODA4MTIsImV4cCI6MTQ1ODI4MDgxNSwiaWF0IjoxNDU4MjgwODEyLCJ1bmlxdWVfbmFtZSI6ImFkbWluIiwicm9sZSI6IkFkbWluIiwiQXNwTmV0LklkZW50aXR5LlNlY3VyaXR5U3RhbXAiOiIxMzNmNDRkMi03YTkxLTQyYjEtODY0YS00MDcwNzY1NDIwZjMiLCJ1c2FnZSI6ImFjY2Vzc190b2tlbiIsImNvbmZpZGVudGlhbCI6dHJ1ZSwic2NvcGUiOlsib2ZmbGluZV9hY2Nlc3MiLCJyb2xlcyIsInByb2ZpbGUiXSwic3ViIjoiYWYwNjk4NTEtMGNlZi00NGI1LWIyOTctYzQ3NTY5NzFlNzkyIiwiYXVkIjpbImh0dHA6Ly9sb2NhbGhvc3Q6MTA0NTAvIiwiaHR0cDovL2xvY2FsaG9zdDoxMDM3Ny8iXSwiYXpwIjoibXlQdWJsaWNDbGllbnQiLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjEwNDUwLyJ9.GAaaaZkSNKN5f1iAFo2auTXXRU2bukP1rpfmV-NGeHgQYckji-QIOcxv4JyAQsLCRWXuS0TZ3eSq8RrmI1anUTuPsdj15S71PWFs501UpUu5ouyuVEAvl0lMnyF7irj-rQiw5z6gUpQDGDXKJWMyJ4C0MJDyyEa9HQbGUF8YOctBTIyzCLsZZbAQcTHZQ4F7FW04y3CVdwiiGw80GHrAELw9Z-T-AeT57M7ylJL6F6MpaCJT84BWbcC8SgbWysb1-my8BTvVB345ylFbVOaISGyVIXiFn0LAckrKc2gH0oGLtk3Z0h7PXC1mEcsEsAF1y0l_xCx4wEUauV6Tz8m_4A'
                    };
                }
            };

            function clearQueryToken() {
                if (service.hub) {
                    service.hub.connection.qs = {};
                }
            };
           
          

        }]);




})();