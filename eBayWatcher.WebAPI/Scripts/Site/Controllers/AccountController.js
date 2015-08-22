var AccountController = angular.module('AccountController', []);

AccountController.controller('AccountController', function ($scope, $http) {

    $scope.authStatus = {};

    $scope.getState = function () {
        if (getCookie('eBayWatcherSession')) {
            getAccount(getCookie('eBayWatcherSession'));
        }
        else {
            $http.post('/Account')
                .then(function (response) {
                    console.log('Got session id ' + response.data);
                    setCookie('eBayWatcherSession', response.data);
                    getAccount(response.data);
                }, function (response) {
                    // Error
                });
        }

        function getAccount(sessionId) {
            $http.get('/Account/' + sessionId)
                .then(function (response) {
                    console.log(response.data);
                    $scope.authStatus = response.data;
                }, function (response) {
                    // Error
                });
        }
    }
    $scope.getState();

    $scope.startLogin = function () {
        $http.post('/Account/' + getCookie('eBayWatcherSession') + '/StartLogin')
        .then(function (response) {
            $scope.authStatus = response.data;
            console.log(response.data);
        }, function (response) {
            // Error
        });
    }
    $scope.continueLogin = function () {
        $http.post('/Account/' + getCookie('eBayWatcherSession') + '/ContinueLogin')
        .then(function (response) {
            $scope.authStatus = response.data;
            console.log(response.data);
        }, function (response) {
            // Error
        });
    }

    function setCookie(cname, cvalue, exdays) {
        var d = new Date();
        d.setTime(d.getTime() + ((exdays || 30) * 24 * 60 * 60 * 1000));
        var expires = "expires=" + d.toUTCString();
        document.cookie = cname + "=" + cvalue + "; " + expires;
    }
    function getCookie(cname) {
        var name = cname + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') c = c.substring(1);
            if (c.indexOf(name) == 0) return c.substring(name.length, c.length);
        }
        return "";
    }
});