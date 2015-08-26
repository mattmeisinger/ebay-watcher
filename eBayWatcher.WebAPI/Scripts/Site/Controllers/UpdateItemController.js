var UpdateItemController = angular.module('UpdateItemController', []);

UpdateItemController.controller('UpdateItemController', ['$scope', '$http', '$rootScope', function ($scope, $http, $rootScope) {

    $scope.saving = false;
    $scope.item = null;

    $rootScope.$on('Search', function (details, item) {
        console.log('Received item selected message.');
        console.log(item);
        $scope.item = item;
    });

    $scope.delete = function (item) {
        $scope.saving = true;
        $http.delete('/WatchListItems/' + item.Id)
            .then(function (response) {
                $rootScope.$broadcast('RefreshList');
                $scope.saving = false;
                $scope.item = null;
            }, function (response) {
                $scope.saving = false;
            });
    }

}]);