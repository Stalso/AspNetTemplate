(function () {
    'use strict';

    angular
        .module('SignalRService', ['SignalR'])
        .factory('TemplateHub', ['$http', 'Hub', '$rootScope', 'Auth', function factory($http, Hub, $rootScope, Auth) {
          
            var token1 = 'eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IjFQREdZV084R0JVOUtKQjBBME04R0dBX0NJSjRBM0cxQ19JTllPVTkifQ.eyJuYmYiOjE0NTg0ODIzNDYsImV4cCI6MTQ1ODQ4NTk0NiwiaWF0IjoxNDU4NDgyMzQ2LCJ1bmlxdWVfbmFtZSI6ImFkbWluIiwicm9sZSI6IkFkbWluIiwiQXNwTmV0LklkZW50aXR5LlNlY3VyaXR5U3RhbXAiOiI1NGIxMTk5Mi1hZDgxLTQ4NzAtOWU0Zi0wN2M3MzAyNmUwYzUiLCJ1c2FnZSI6ImFjY2Vzc190b2tlbiIsImNvbmZpZGVudGlhbCI6dHJ1ZSwic2NvcGUiOlsib2ZmbGluZV9hY2Nlc3MiLCJyb2xlcyIsInByb2ZpbGUiXSwic3ViIjoiYTI0NzE0NzktMjQxMy00ZmU2LWJmMzctMjhiMWJhN2I3ODljIiwiYXVkIjpbImh0dHA6Ly9sb2NhbGhvc3Q6MTA0NTAvIiwiaHR0cDovL2xvY2FsaG9zdDoxMDM3Ny8iXSwiYXpwIjoibXlQdWJsaWNDbGllbnQiLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjEwNDUwLyJ9.zfg8nZcA9UZwK4XOAZtyTsO-G0ZjIgOzfF8BJNDIZlNWGVm581j1z1MxRzVM6qVTscWEXX7R7gPW6OQ1hRfcOHNZHmf0X4i30AeHC4qudWFfJ2KDL6WOKpnAVBPDfxl9id3HPzzQF3iWaT4Mt__nCWcbBlFPCBbc938kev9idm5TMJAGSiomXZuz4R4EZylxGwjIwTJzfqBO_l0TP-5zJ1vN8thZvDKh_eTpIhnaGEM6O4HRwWwu_H4aEqPU44_5Ja6jXDUmfEtYSLqAEzeiwlYCpMg8aZBZFKH8BnOhCZHjSMe7Q1beglpKrQVchMXzWw3qBy6xK1uyKjhA-wzO3g';
            //var token1 = 'eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IjFQREdZV084R0JVOUtKQjBBME04R0dBX0NJSjRBM0cxQ19JTllPVTkifQ.eyJuYmYiOjE0NTg0NzAyMTEsImV4cCI6MTQ1ODQ3MzgxMSwiaWF0IjoxNDU4NDcwMjExLCJ1bmlxdWVfbmFtZSI6ImFkbWluIiwicm9sZSI6IkFkbWluIiwiQXNwTmV0LklkZW50aXR5LlNlY3VyaXR5U3RhbXAiOiI1NGIxMTk5Mi1hZDgxLTQ4NzAtOWU0Zi0wN2M3MzAyNmUwYzUiLCJ1c2FnZSI6ImFjY2Vzc190b2tlbiIsImNvbmZpZGVudGlhbCI6dHJ1ZSwic2NvcGUiOlsib2ZmbGluZV9hY2Nlc3MiLCJyb2xlcyIsInByb2ZpbGUiXSwic3ViIjoiYTI0NzE0NzktMjQxMy00ZmU2LWJmMzctMjhiMWJhN2I3ODljIiwiYXVkIjpbImh0dHA6Ly9sb2NhbGhvc3Q6MTA0NTAvIiwiaHR0cDovL2xvY2FsaG9zdDoxMDM3Ny8iXSwiYXpwIjoibXlQdWJsaWNDbGllbnQiLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjEwNDUwLyJ9.cvqe41ToM5fAkt-lXQTkjhG__JvRIFp53b045Wf3LJBgyAb_0Q1t4spxrUwc4AJDaox1KNjqoZR7b7eQ_cPfHRwOxUOQACJXghaxCnA5VWCs6avw8u3BFHuqSDQH6gGTvv2BWpvpt_SLV--FpUfOSntRb9KY4XcKFdZMsYMA1Nt05myZusnEZU62kspLk-zdUsb_Hv7Eu8OxlcvuphDrXVRMqWTNXTh-Z_Q9XOzrNZn7O5xFdPpDn4GzOYiCi1cI2r7Q8zuJiqZ6Wz2TMJuBDDhGeoxwD87agbnTkegSc_IHnrYeM8DMOiNsqocs2L64ehNcXTh5b07DIC7l6a2NQQ';
            $.ajaxSetup({
                beforeSend: function (xhr, settings) {
                    console.log(xhr);
                    //settings.url = settings.url + "&access_token=" + token1;
                    settings.url = settings.url + "&access_token=" + token1;

                }
            });
            var hub = new Hub('msg', {
                ver : '1',
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
                useSharedConnection: false,
                errorHandler: function (error,data) {
                    console.log(error);
                     
                },
                //queryParams: {

                //   //expired
                //   //'access_token': 'eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IjFQREdZV084R0JVOUtKQjBBME04R0dBX0NJSjRBM0cxQ19JTllPVTkifQ.eyJuYmYiOjE0NTgyODA4MTIsImV4cCI6MTQ1ODI4MDgxNSwiaWF0IjoxNDU4MjgwODEyLCJ1bmlxdWVfbmFtZSI6ImFkbWluIiwicm9sZSI6IkFkbWluIiwiQXNwTmV0LklkZW50aXR5LlNlY3VyaXR5U3RhbXAiOiIxMzNmNDRkMi03YTkxLTQyYjEtODY0YS00MDcwNzY1NDIwZjMiLCJ1c2FnZSI6ImFjY2Vzc190b2tlbiIsImNvbmZpZGVudGlhbCI6dHJ1ZSwic2NvcGUiOlsib2ZmbGluZV9hY2Nlc3MiLCJyb2xlcyIsInByb2ZpbGUiXSwic3ViIjoiYWYwNjk4NTEtMGNlZi00NGI1LWIyOTctYzQ3NTY5NzFlNzkyIiwiYXVkIjpbImh0dHA6Ly9sb2NhbGhvc3Q6MTA0NTAvIiwiaHR0cDovL2xvY2FsaG9zdDoxMDM3Ny8iXSwiYXpwIjoibXlQdWJsaWNDbGllbnQiLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjEwNDUwLyJ9.GAaaaZkSNKN5f1iAFo2auTXXRU2bukP1rpfmV-NGeHgQYckji-QIOcxv4JyAQsLCRWXuS0TZ3eSq8RrmI1anUTuPsdj15S71PWFs501UpUu5ouyuVEAvl0lMnyF7irj-rQiw5z6gUpQDGDXKJWMyJ4C0MJDyyEa9HQbGUF8YOctBTIyzCLsZZbAQcTHZQ4F7FW04y3CVdwiiGw80GHrAELw9Z-T-AeT57M7ylJL6F6MpaCJT84BWbcC8SgbWysb1-my8BTvVB345ylFbVOaISGyVIXiFn0LAckrKc2gH0oGLtk3Z0h7PXC1mEcsEsAF1y0l_xCx4wEUauV6Tz8m_4A'

                //    //valid
                //    //'access_token': 'eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IlBPMU5YSFhPMUI4UTZBQ1VMTldFSEZHV0dKWFRWWlhFVEowM0hLVFoifQ.eyJuYmYiOjE0NTgzODUyODgsImV4cCI6MTQ1ODM4ODg4OCwiaWF0IjoxNDU4Mzg1Mjg4LCJ1bmlxdWVfbmFtZSI6ImFkbWluIiwicm9sZSI6IkFkbWluIiwiQXNwTmV0LklkZW50aXR5LlNlY3VyaXR5U3RhbXAiOiJiODFmZWI1YS0wZWI2LTRkYzAtOTFmMy1mYTg1N2E0NjE1NjAiLCJ1c2FnZSI6ImFjY2Vzc190b2tlbiIsImNvbmZpZGVudGlhbCI6dHJ1ZSwic2NvcGUiOlsib2ZmbGluZV9hY2Nlc3MiLCJyb2xlcyIsInByb2ZpbGUiXSwic3ViIjoiYTUwODA4ZGYtNWU5MS00OTgwLTkwYTQtMWE1YWU3ODY5MGFlIiwiYXVkIjpbImh0dHA6Ly9sb2NhbGhvc3Q6MTA0NTAvIiwiaHR0cDovL2xvY2FsaG9zdDoxMDM3Ny8iXSwiYXpwIjoibXlQdWJsaWNDbGllbnQiLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjEwNDUwLyJ9.ArZic2sisMTe7oQzy50cBxxAHyyM5Ymz9Fp0E5go3hHyTiq3XOkKuMvzwUeM9SdzwKII87uL1kAUZGM_LwfdqCt9xG2mQlzVyzC37yGaZUjZtaVsbGmu-zysi3i3_WWiZUoh2KID3ZzWZ0X5N_vy3hRKgn6jBKfUbg9Hr7jVmM6z69cp0Z3Ut9d4x1CjA5HsY6Vcodt1HpzBIOysbQBuOTYR4OI2WmjPLKPt3v24JIVwLhN9_bTala-yCiiyTzC-t5qbTnRd4b3QQ2NghyjNSZu_G845gLxXzjfzBBbfj5d6Y2CHHw4SLjVJacHl03qa18LvcaWEyt0oe8qDAjPBFg'
                // },
               

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
                            //changeToken();
                            //changeToken();
                            //establishConnection();

                                setTimeout(function () {
                                    changeToken();
                                    establishConnection();
                                }, 1);
                            //$rootScope.$apply(function () {
                            //    service.connectionEstablished = false;
                            //});
                            //token1 = 'eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IjFQREdZV084R0JVOUtKQjBBME04R0dBX0NJSjRBM0cxQ19JTllPVTkifQ.eyJuYmYiOjE0NTgzMTI1NTEsImV4cCI6MTQ1ODMxNjE1MSwiaWF0IjoxNDU4MzEyNTUxLCJ1bmlxdWVfbmFtZSI6ImFkbWluIiwicm9sZSI6IkFkbWluIiwiQXNwTmV0LklkZW50aXR5LlNlY3VyaXR5U3RhbXAiOiJkNzc2ZjJjNS01NTdhLTRmYTAtOGE5Zi05MGZiMzhhNjcwODUiLCJ1c2FnZSI6ImFjY2Vzc190b2tlbiIsImNvbmZpZGVudGlhbCI6dHJ1ZSwic2NvcGUiOlsib2ZmbGluZV9hY2Nlc3MiLCJyb2xlcyIsInByb2ZpbGUiXSwic3ViIjoiZjUwOTQzYmItN2FkMS00MmNhLTliMzgtMzkwMDJiYTY4MGYxIiwiYXVkIjpbImh0dHA6Ly9sb2NhbGhvc3Q6MTA0NTAvIiwiaHR0cDovL2xvY2FsaG9zdDoxMDM3Ny8iXSwiYXpwIjoibXlQdWJsaWNDbGllbnQiLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjEwNDUwLyJ9.wPcnZNs8OjMglCYCR7QMIEM2-9CpxOdX5cYmfIlT7iAj_ICJHdt31IvOUUW8yvERQYIM6SoqD1fEbbWFnW_LYX3f_KZ9MWlpr0-QVRljSEMXgVzub4gee5YWCwLCG1xTg62AolO1FCfDXd9yD9qH1D3fkf-lUQg0AwoNcozNZzIfT230O8EFQgzgh50wwxZRtbZKOAelglfw3EwbjglgVMp7aejvBS2ccDGZpSE2OYzLCX7tmqsigqm9ShId4yLlVY9yKq-acF3Q-B_89l2giKUlhnHunOAJbw8JI9gbywf8TPnwDYtl_j2To-4YYgT-3YTo6BODyEYWiY_vG6jFSQ';
                            //establishConnection();
                            break;
                    }
                },
                transport: 'longPolling'
            });
            $.connection.hub.logging = true;           
            var service = {
                connectionEstablished: false,
                hub: hub,
                dropConnection: dropConnection,
                establishConnection: establishConnection,
                sendFreeMessage: function () { throw "No handler assigned for `sendFreeMessage`"; },
                sendProtectedMessage: function () { throw "No handler assigned for `sendProtectedMessage`"; },
                //atoken: token1,
                changeToken: changeToken,
                changeOnBadToken: changeOnBadToken
            };

           
            return service;
           
            function changeToken()
            {
                token1 = 'eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IjFQREdZV084R0JVOUtKQjBBME04R0dBX0NJSjRBM0cxQ19JTllPVTkifQ.eyJuYmYiOjE0NTg0ODIzNDYsImV4cCI6MTQ1ODQ4NTk0NiwiaWF0IjoxNDU4NDgyMzQ2LCJ1bmlxdWVfbmFtZSI6ImFkbWluIiwicm9sZSI6IkFkbWluIiwiQXNwTmV0LklkZW50aXR5LlNlY3VyaXR5U3RhbXAiOiI1NGIxMTk5Mi1hZDgxLTQ4NzAtOWU0Zi0wN2M3MzAyNmUwYzUiLCJ1c2FnZSI6ImFjY2Vzc190b2tlbiIsImNvbmZpZGVudGlhbCI6dHJ1ZSwic2NvcGUiOlsib2ZmbGluZV9hY2Nlc3MiLCJyb2xlcyIsInByb2ZpbGUiXSwic3ViIjoiYTI0NzE0NzktMjQxMy00ZmU2LWJmMzctMjhiMWJhN2I3ODljIiwiYXVkIjpbImh0dHA6Ly9sb2NhbGhvc3Q6MTA0NTAvIiwiaHR0cDovL2xvY2FsaG9zdDoxMDM3Ny8iXSwiYXpwIjoibXlQdWJsaWNDbGllbnQiLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjEwNDUwLyJ9.zfg8nZcA9UZwK4XOAZtyTsO-G0ZjIgOzfF8BJNDIZlNWGVm581j1z1MxRzVM6qVTscWEXX7R7gPW6OQ1hRfcOHNZHmf0X4i30AeHC4qudWFfJ2KDL6WOKpnAVBPDfxl9id3HPzzQF3iWaT4Mt__nCWcbBlFPCBbc938kev9idm5TMJAGSiomXZuz4R4EZylxGwjIwTJzfqBO_l0TP-5zJ1vN8thZvDKh_eTpIhnaGEM6O4HRwWwu_H4aEqPU44_5Ja6jXDUmfEtYSLqAEzeiwlYCpMg8aZBZFKH8BnOhCZHjSMe7Q1beglpKrQVchMXzWw3qBy6xK1uyKjhA-wzO3g';
            }
            function changeOnBadToken() {
                token1 = 'eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IjFQREdZV084R0JVOUtKQjBBME04R0dBX0NJSjRBM0cxQ19JTllPVTkifQ.eyJuYmYiOjE0NTgyODA4MTIsImV4cCI6MTQ1ODI4MDgxNSwiaWF0IjoxNDU4MjgwODEyLCJ1bmlxdWVfbmFtZSI6ImFkbWluIiwicm9sZSI6IkFkbWluIiwiQXNwTmV0LklkZW50aXR5LlNlY3VyaXR5U3RhbXAiOiIxMzNmNDRkMi03YTkxLTQyYjEtODY0YS00MDcwNzY1NDIwZjMiLCJ1c2FnZSI6ImFjY2Vzc190b2tlbiIsImNvbmZpZGVudGlhbCI6dHJ1ZSwic2NvcGUiOlsib2ZmbGluZV9hY2Nlc3MiLCJyb2xlcyIsInByb2ZpbGUiXSwic3ViIjoiYWYwNjk4NTEtMGNlZi00NGI1LWIyOTctYzQ3NTY5NzFlNzkyIiwiYXVkIjpbImh0dHA6Ly9sb2NhbGhvc3Q6MTA0NTAvIiwiaHR0cDovL2xvY2FsaG9zdDoxMDM3Ny8iXSwiYXpwIjoibXlQdWJsaWNDbGllbnQiLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjEwNDUwLyJ9.GAaaaZkSNKN5f1iAFo2auTXXRU2bukP1rpfmV-NGeHgQYckji-QIOcxv4JyAQsLCRWXuS0TZ3eSq8RrmI1anUTuPsdj15S71PWFs501UpUu5ouyuVEAvl0lMnyF7irj-rQiw5z6gUpQDGDXKJWMyJ4C0MJDyyEa9HQbGUF8YOctBTIyzCLsZZbAQcTHZQ4F7FW04y3CVdwiiGw80GHrAELw9Z-T-AeT57M7ylJL6F6MpaCJT84BWbcC8SgbWysb1-my8BTvVB345ylFbVOaISGyVIXiFn0LAckrKc2gH0oGLtk3Z0h7PXC1mEcsEsAF1y0l_xCx4wEUauV6Tz8m_4A';
            }
            function dropConnection() {
                if (service.hub) {
                    hub.disconnect();

                }
            }
            function establishConnection() {
                if (service.hub) {
                    hub.connect();
                }
            }
          

        }]);




})();