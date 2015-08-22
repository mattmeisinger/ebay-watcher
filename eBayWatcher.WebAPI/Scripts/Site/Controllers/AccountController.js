var AccountController = angular.module('AccountController', []);

AccountController.controller('AccountController', ['$scope', '$http', function ($scope, $http) {

    console.log('AccountController');
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
            window.open(response.data.LoginUrl); // open popup window so the user can log in
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
}]);