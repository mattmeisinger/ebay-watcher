var MyListController = angular.module('MyListController', []);

MyListController.controller('MyListController', ['$scope', '$http', '$rootScope', function ($scope, $http, $rootScope) {

    $scope.loading = false;
    $scope.newItem = {};
    $scope.list = [];

    // For testing
    $scope.newItem.CategoryId = 260;
    $scope.newItem.CategoryName = "Stamps";

    $scope.selectCategory = function () {
        $rootScope.$broadcast('OpenSelectCategoryWindow');
    };

    $rootScope.$on('CategorySelected', function (details, item) {
        console.log('received category selected');
        console.log(item);
        $scope.newItem.CategoryId = item.Id;
        $scope.newItem.CategoryName = item.Name;
    });

    $scope.itemSelected = function (item) {
        console.log('item selected');
        console.log(item);
        $rootScope.$broadcast('Search', item);

        $.each($scope.list, function (i, listItem) {
            listItem.Selected = false;
        });
        item.Selected = true;
    }

    $scope.addItem = function (item) {
        if (!item.Name || item.Name.length < 3) {
            alert('Search text must be at least 3 characters long.');
            return;
        }
        console.log('item added to list');
        console.log(item);
        var newItem = {
            Name: item.Name,
            SearchText: item.SearchText || item.Name,
            CategoryId: item.CategoryId,
            CategoryName: item.CategoryName
        };
        $scope.saving = true;
        $http.post('/WatchListItems', { Username: getCookie('eBayWatcherUsername'), Token: getCookie('eBayWatcherToken'), item: newItem })
            .then(function (response) {
                var savedItem = response.data;
                $scope.saving = false;
                $scope.list.push(savedItem);
            }, function (response) {
                $scope.saving = false;
            });
    }

    $scope.refresh = function () {
        $http.get('/WatchListItems')
            .then(function (response) {
                $scope.list = response.data;
            }, function (response) {

            });
    }
    $scope.refresh();

    $rootScope.$on('RefreshList', function (details) {
        $scope.refresh();
    });

}]);