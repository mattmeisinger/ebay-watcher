
eBayWatcher.controller('UpdateItemController', ['$scope', '$http', '$rootScope', 'DataService', function ($scope, $http, $rootScope, DataService) {

    $scope.saving = false;
    $scope.item = null;

    $rootScope.$on('item:selected', function (details, item) {
        console.log('Received item selected message.');
        console.log(item);
        $scope.item = item;
        DataService.setItem(item);
    });

    $scope.delete = function (item) {
        $scope.saving = true;
        $http.delete(DataService.baseUrl + '/WatchListItems/' + item.Id)
            .then(function (response) {
                $rootScope.$broadcast('list:refresh');
                $scope.saving = false;
                $scope.item = null;
            }, function (response) {
                $scope.saving = false;
            });
    }

    $scope.search = function (item) {
        $rootScope.$broadcast('item:search', item);
    }

}]);