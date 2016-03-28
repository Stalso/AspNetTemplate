angular.module('c-SignalR', [])
    .constant('$', window.jQuery)
        .factory('HubConnection', ['$', '$q', '$rootScope', function ($, $q, $rootScope) {
            //This will allow same connection to be used for all Hubs
            //It also keeps connection as singleton.

            // For connection
            //rootPath
            //queryParams
            //errorHandler
            //logging
            //transport
            //jsonp
            //stateChanged
            //autoConnect

            ///////////////// security
            // getToken - gets access token from server or storage
            // getConnectionToken - gets token which is used by connection
            // setConnectionToken - set token which is used by connection
            // unAuthHandler - 401 handler 
            // forbiddenHandler - 403 handler
            // tokenChangedLogic - in options
            // tokenChangedEventHandler - in connection

            ///////////
            /////////////////////

            //listeners
            //methods
            //stateChanged
            //errorHandler
            // on

            // list of connections
            var globalConnections = [];



            // creates new connection
            // used in getConnection

            // For connection
            //rootPath ++ 
            //queryParams++
            //errorHandler++
            //logging ++ 
            //transport++
            //jsonp++
            //stateChanged++
            //autoConnect           
            function initNewConnection(options) {
                var connection = null;
                if (options && options.rootPath) {
                    connection = $.hubConnection(options.rootPath, { useDefaultPath: false });
                } else {
                    connection = $.hubConnection();
                }

                connection.logging = (options && options.logging ? true : false);

                if (options && options.errorHandler) {
                    connection.error(options.errorHandler);
                }
                if (options && options.queryParams) {
                    connection.qs = options.queryParams;
                }
                if (options && options.stateChanged) {
                    connection.stateChanged(options.stateChanged);
                }
                ///////////////// security
                // getToken - gets access token from server or storage ++
                // getConnectionToken - gets token which is used by connection ++
                // setConnectionToken - set token which is used by connection ++
                // unAuthHandler - 401 handler ++
                // forbiddenHandler - 403 handler ++
                // tokenChangedLogic - in options ++
                // tokenChangedEventHandler - in connection ++
                ///////////

                if (options && options.securityTokenOptions) {
                    // promise
                    if (options.securityTokenOptions.getToken) {
                        connection.getToken = function () {
                            return $q.when(options.securityTokenOptions.getToken());
                        }
                    }
                    connection.getConnectionToken = options.securityTokenOptions.getConnectionToken;
                    connection.setConnectionToken = options.securityTokenOptions.setConnectionToken;
                    connection.unAuthHandler = options.securityTokenOptions.unAuthHandler;
                    connection.forbiddenHandler = options.securityTokenOptions.forbiddenHandler;

                    // must disconnect, ask token, set new token , and reconnect
                    if (options.securityTokenOptions.tokenChangedLogic) {
                        connection.tokenChangedLogic = function () {
                            return $q.when(options.securityTokenOptions.tokenChangedLogic());
                        }
                        connection.tokenChangedEventHandler = function () {
                            if (!connection.tokenChangedEventBlocked)
                                return connection.tokenChangedLogic();
                        }
                    }
                    if (options.securityTokenOptions.unAuthHandler || options.securityTokenOptions.forbiddenHandler) {
                        connection.error(function (error, data) {
                            if (error.context.status === 401
                                && connection.unAuthHandler) {
                                connection.unAuthHandler();
                            }
                            if (error.context.status === 403
                                && connection.forbiddenHandler) {
                                connection.forbiddenHandler();
                            }

                        });
                    }
                }
                return connection;
            }



            return function (connectionOptions) {
                var Connection = this;

                var connection = initNewConnection(connectionOptions);

                Connection.connection = connection;

                Connection.disconnect = function () {
                    connection.stop();
                };

                Connection.checkSecurityOptions = function () {
                    return connection.getToken
                        && connection.getConnectionToken
                            && connection.setConnectionToken;
                }
                Connection.getToken = function () {
                    return connection.getToken();
                }

                Connection.getConnectionToken = function () {
                    return connection.getConnectionToken();
                }
                if (connection.tokenChangedEventHandler)
                {
                    Connection.tokenChangedEventHandler = function () {
                        return connection.tokenChangedEventHandler();
                    }
                    Connection.tokenChangedLogic = function () {
                        return connection.tokenChangedLogic();
                    }

                }
               
                Connection.setTokenAndStart = function (token, startOptions)
                {
                    if (connection.setConnectionToken)
                        connection.setConnectionToken(token);
                    connection.tokenChangedEventBlocked = false;
                    return connection.start(startOptions);
                }

                Connection.connect = function () {

                    var startOptions = {};
                    if (connectionOptions && connectionOptions.transport)
                        startOptions.transport = connectionOptions.transport;
                    if (connectionOptions && connectionOptions.jsonp)
                        startOptions.jsonp = connectionOptions.jsonp;

                    if (connection.getToken) {
                        connection.tokenChangedEventBlocked = true;
                        return connection.getToken().then(function (token) {
                            return Connection.setTokenAndStart(token, startOptions);
                        }, function (err) {
                            console.log('Bad token during connection');
                            connection.tokenChangedEventBlocked = false;
                            return $q.reject(err);
                        });
                    }
                    else {
                        return connection.start(startOptions);
                    }
                };

                Connection.createHubProxy = function (hubName) {
                    return connection.createHubProxy(hubName);
                }

                if (connectionOptions && connectionOptions.hubs && connectionOptions.hubs.length) {
                    Connection.hubs = {};
                    for (var i = 0; i < connectionOptions.hubs.length; i++) {
                        Connection.hubs[connectionOptions.hubs[i].hubName] = new Hub(this, connectionOptions.hubs[i].hubName, connectionOptions.hubs[i].options);
                    }
                }

                if (!connectionOptions ||
                       (connectionOptions && (connectionOptions.autoConnect === undefined || connectionOptions.autoConnect))) {
                    Connection.promise = Connection.connect();
                }


                return Connection;

            }
           
            function Hub(Connection, hubName, options) {
                var Hub = this;
                Hub.connection = Connection;

                Hub.proxy = Connection.createHubProxy(hubName);

                Hub.on = function (event, fn) {
                    Hub.proxy.on(event, fn);
                };

                Hub.invoke = function (method, args) {
                    return Hub.proxy.invoke.apply(Hub.proxy, arguments)
                };

                Hub.disconnect = function () {
                    Connection.disconnect();
                };

                Hub.connect = function () {
                    return Connection.connect();
                }

                if (options && options.stateChanged) {
                    Connection.connection.stateChanged(options.stateChanged);
                }
                if (options && options.errorHandler) {
                    Connection.connection.error(options.errorHandler);
                   
                }
                if (options && (options.unAuthHandler || options.forbiddenHandler)) {
                    Hub.unAuthHandler = options.unAuthHandler;
                    Hub.forbiddenHandler = options.forbiddenHandler;

                    Hub.connection.error(function (error, data) {
                        if (error.context.status === 401
                            && Hub.unAuthHandler) {
                            Hub.unAuthHandler();
                        }
                        if (error.context.status === 403
                            && Hub.forbiddenHandler) {
                            Hub.forbiddenHandler();
                        }
                    });
                }
                // register all listeners
                if (options && options.listeners) {
                    Object.getOwnPropertyNames(options.listeners).filter(function (propName) {
                        return typeof options.listeners[propName] === 'function';
                    }).forEach(function (propName) {
                        Hub.on(propName, options.listeners[propName]);
                    });
                }

                Hub.secureInvoke = function (Hub, args) {

                    var res = Hub.invoke.apply(Hub, args);
                    return res.then(function (data) {
                        console.log(data);
                    }, function (err, data) {
                        console.log('Error while sending message');
                        if (Connection.connection.unAuthHandler) {
                            Connection.connection.unAuthHandler();
                        }
                    });
                }

                Hub.checkSecurityOptions = function () {
                    return Connection.checkSecurityOptions();
                }

                if (options && options.methods) {
                    angular.forEach(options.methods, function (method) {
                        Hub[method] = function () {

                            var args = $.makeArray(arguments);
                            args.unshift(method);

                            if (Hub.checkSecurityOptions()) {
                                Connection.connection.tokenChangedEventBlocked = true;
                                return Connection.getToken().then(function (newToken) {
                                    // check by my self and cast token logic if needed
                                    var oldToken = null;
                                    oldToken = Connection.getConnectionToken();
                                    var tokensAreEqual = false;
                                    if ((!newToken && !oldToken) || newToken === oldToken) {
                                        tokensAreEqual = true;
                                    }
                                    if (!tokensAreEqual)
                                        return Connection.tokenChangedLogic(newToken);
                                    else
                                        return newToken;
                                    
                                    
                                }, function (err) {
                                    Connection.connection.tokenChangedEventBlocked = false;
                                    Connection.disconnect();
                                    return $q.reject(err);
                                }).then(function (data) {
                                    Connection.connection.tokenChangedEventBlocked = false;
                                    return Hub.secureInvoke(Hub, args);
                                }).catch(function (err) {
                                    Connection.connection.tokenChangedEventBlocked = false;
                                });
                            }
                            else
                                return Hub.secureInvoke(Hub, args);
                        };
                    });
                }
            }


            
        }]);

// Common.js package manager support (e.g. ComponentJS, WebPack)
if (typeof module !== 'undefined' && typeof exports !== 'undefined' && module.exports === exports) {
    module.exports = 'SignalR';
}

