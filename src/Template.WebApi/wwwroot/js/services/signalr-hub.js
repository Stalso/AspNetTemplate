angular.module('SignalR', [])
    .constant('$', window.jQuery)
        .factory('Hub', ['$', 'Auth', '$q', '$rootScope', function ($, Auth, $q, $rootScope) {
            //This will allow same connection to be used for all Hubs
            //It also keeps connection as singleton.
            var globalConnections = [];

            function initNewConnection(options) {
                var connection = null;
                if (options && options.rootPath) {
                    connection = $.hubConnection(options.rootPath, { useDefaultPath: false });
                } else {
                    connection = $.hubConnection();
                }

                connection.logging = (options && options.logging ? true : false);
                // getToken - gets access token from server or storage
                // getConnectionToken - gets token which is used by connection
                // setConnectionToken() - set token which is used by connection
                // unAuthHandler - 401 handler
                // forbiddenHandler - 403 handler
                
                if (options.securityTokenOptions) {
                    connection.getToken = options.securityTokenOptions.getToken;
                    connection.getConnectionToken = options.securityTokenOptions.getConnectionToken;
                    connection.setConnectionToken = options.securityTokenOptions.setConnectionToken;
                    connection.unAuthHandler = options.securityTokenOptions.unAuthHandler;
                    connection.forbiddenHandler = options.securityTokenOptions.forbiddenHandler;
                    connection.tokenChangedEvent = options.securityTokenOptions.tokenChangedEvent;
                    if (connection.tokenChangedEvent)
                    {
                        connection.tokenChangedEventHandler = function () {
                            if(!connection.tokenChangedEventBlocked)
                                return connection.tokenChangedEvent();
                        }
                    }
                }
                return connection;
            }
          

            function getConnection(options) {
                var useSharedConnection = !(options && options.useSharedConnection === false);
                if (useSharedConnection) {
                    return typeof globalConnections[options.rootPath] === 'undefined' ?
                    globalConnections[options.rootPath] = initNewConnection(options) :
                    globalConnections[options.rootPath];
                }
                else {
                    return initNewConnection(options);
                }
            }

            return function (hubName, options) {
                var Hub = this;

                Hub.connection = getConnection(options);
                Hub.proxy = Hub.connection.createHubProxy(hubName);

                Hub.on = function (event, fn) {
                    Hub.proxy.on(event, fn);
                };
                Hub.invoke = function (method, args) {
                    return Hub.proxy.invoke.apply(Hub.proxy, arguments)
                };
                
                Hub.disconnect = function () {
                    //delete Hub.connection.qs;
                    Hub.connection.stop();
                };

                // getToken - gets access token from server or storage
                // getConnectionToken - gets token which is used by connection
                // setConnectionToken() - set token which is used by connection
                // unAuthHandler - 401 handler
                // forbiddenHandler - 403 handler
                //Hub.securityTokenOptions = options.securityTokenOptions;
                var currentConnection = Hub.connection;
                if (currentConnection.getToken)
                    Hub.getToken = function () { return currentConnection.getToken(); }
                if (currentConnection.getConnectionToken)
                    Hub.getConnectionToken = function () { return currentConnection.getConnectionToken() };
                if (currentConnection.setConnectionToken)
                    Hub.setConnectionToken = function (token) { return currentConnection.setConnectionToken(token) }
                if (options.securityTokenOptions)
                {
                    //Hub.getToken = options.securityTokenOptions.getToken;
                    //Hub.getConnectionToken = options.securityTokenOptions.getConnectionToken;
                    //Hub.setConnectionToken = options.securityTokenOptions.setConnectionToken;
                    //Hub.unAuthHandler = options.securityTokenOptions.unAuthHandler;
                    //Hub.forbiddenHandler = options.securityTokenOptions.forbiddenHandler;
                   
                    Hub.unAuthHandler = options.securityTokenOptions.unAuthHandler;
                    Hub.forbiddenHandler = options.securityTokenOptions.forbiddenHandler;
                }
                Hub.validateSecurityOptions = function () {
                   
                        return Hub.getToken
                            && Hub.getConnectionToken
                                && Hub.setConnectionToken;
                  
                }
                Hub.connect = function () {

                    var startOptions = {};
                    if (options.transport) startOptions.transport = options.transport;
                    if (options.jsonp) startOptions.jsonp = options.jsonp;
                   
                    //if (Hub.securityTokenOptions) {
                        if (Hub.getToken) {
                            return Hub.getToken().then(function (token) {
                                if (Hub.setConnectionToken)
                                    Hub.setConnectionToken(token);
                                return Hub.connection.start(startOptions);
                            });
                        }
                    //}
                    else {
                        return Hub.connection.start(startOptions);
                    }                  
                };

                if (options && options.listeners) {
                    Object.getOwnPropertyNames(options.listeners)
                    .filter(function (propName) {
                        return typeof options.listeners[propName] === 'function';
                    })
                        .forEach(function (propName) {
                            Hub.on(propName, options.listeners[propName]);
                        });
                }
               
                Hub.secureInvoke = function (Hub, args) {

                    var res = Hub.invoke.apply(Hub, args);
                    // 401
                    return res.then(function (data) {
                        console.log(data);
                    }, function (err,data) {
                        console.log('Error while sending message');
                        if (Hub.connection.unAuthHandler) {
                            Hub.connection.unAuthHandler();
                        }
                       
                        
                    });
                }
                //////////////////////////

                if (options && options.methods) {
                    angular.forEach(options.methods, function (method) {
                        Hub[method] = function () {

                            var args = $.makeArray(arguments);
                            args.unshift(method);

                            //var callPromise;
                            if (Hub.validateSecurityOptions()) {
                                return Hub.getToken().then(function (newToken) {

                                        var oldToken = null;
                                       
                                            oldToken = Hub.getConnectionToken();

                                            var tokensAreEqual = false;
                                                if ((!newToken && !oldToken) || newToken === oldToken) {
                                                    tokensAreEqual = true;
                                                }
                                                if (!tokensAreEqual) {
                                                    Hub.disconnect();
                                                    //Hub.securityTokenOptions.setConnectionToken(newToken);
                                                    setTimeout(function () {

                                                        Hub.connect().then(function (data) {
                                                            return Hub.secureInvoke(Hub, args);
                                                            //return Hub.invoke.apply(Hub, args);
                                                        });
                                                    }, 1);
                                                }
                                                else {
                                                    return Hub.secureInvoke(Hub, args);
                                                }
                                       
                                    });
                            }
                            else
                                return Hub.secureInvoke(Hub, args);
                        };
                    });
                }
                if (options && options.queryParams) {
                    Hub.connection.qs = options.queryParams;
                }
                if (options && options.errorHandler) {
                    Hub.connection.error(options.errorHandler);
                    //if (Hub.securityTokenOptions)
                    //{
                        Hub.connection.error(function (error, data) {
                            if (error.context.status === 401
                                && Hub.unAuthHandler) {
                                Hub.unAuthHandler();
                            }
                            if (error.context.status === 403 
                                && Hub.forbiddenHandler)
                            {
                                Hub.forbiddenHandler();
                            }
                              
                        });
                    //}
                    //if (Hub.securityTokenOptions)
                    //{
                    //    if( Hub.securityTokenOptions.unAuthHandler)
                    //        Hub.connection.error(Hub.securityTokenOptions.unAuthHandler);
                    //    if (Hub.securityTokenOptions.forbiddenHandler)
                    //        Hub.connection.error(Hub.securityTokenOptions.forbiddenHandler);
                    //}
                }
                if (options && options.stateChanged) {
                    Hub.connection.stateChanged(options.stateChanged);
                }

                //Adding additional property of promise allows to access it in rest of the application.
                if (options.autoConnect === undefined || options.autoConnect) {
                    Hub.promise = Hub.connect();
                }

                return Hub;
            };
        }]);

// Common.js package manager support (e.g. ComponentJS, WebPack)
if (typeof module !== 'undefined' && typeof exports !== 'undefined' && module.exports === exports) {
    module.exports = 'SignalR';
}

