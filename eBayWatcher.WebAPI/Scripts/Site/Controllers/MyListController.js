var MyListController = angular.module('MyListController', []);

MyListController.controller('MyListController', ['$scope', '$http', '$rootScope', function ($scope, $http, $rootScope) {

    console.log('MyListController');
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
        $scope.list.push({
            Name: item.Name,
            SearchText: item.SearchText || item.Name,
            CategoryId: item.CategoryId,
            CategoryName: item.CategoryName
        });
    }

}]);