var SearchController = angular.module('SearchController', []);

SearchController.controller('SearchController', ['$scope', '$http', '$rootScope', function ($scope, $http, $rootScope) {

    console.log('SearchController');
    $scope.loading = true;
    $scope.searched = false;

    $rootScope.$on('Search', function (details, item) {
        $scope.search(item.SearchText, item.CategoryId);
    });

    $scope.search = function (searchTerm, categoryId) {
        $scope.loading = true;
        $http.post('/EbaySearch', { searchTerm: searchTerm, categoryId: categoryId })
            .then(function (response) {
                $scope.results = response.data;
                $scope.loading = false;
                $scope.searched = true;
            }, function (response) {
                $scope.loading = false;
            });
    };

    $scope.ignore = function (item) {
        item.Ignore = true;
    }
    $scope.pin = function (item) {
        item.Pin = true;
    }
    $scope.unpin = function (item) {
        item.Pin = false;
    }

}]);