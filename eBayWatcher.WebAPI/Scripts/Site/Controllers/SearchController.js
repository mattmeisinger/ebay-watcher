var SearchController = angular.module('SearchController', []);

SearchController.controller('SearchController', ['$scope', '$http', '$rootScope', function ($scope, $http, $rootScope) {

    console.log('SearchController');
    $scope.searchTerm = '';
    $scope.selectedCategory = { Id: 261, Name: "United States" };
    $scope.loading = false;

    $scope.selectCategory = function () {
        console.log('emitting...');
        $rootScope.$broadcast('OpenSelectCategoryWindow');
    };

    $rootScope.$on('CategorySelected', function (details, item) {
        console.log('received category selected');
        console.log(item);
        $scope.selectedCategory = item;
    });

    $scope.search = function () {
        $scope.loading = true;
        $http.post('/EbaySearch', { searchTerm: $scope.searchTerm, categoryId: $scope.selectedCategory.Id })
            .then(function (response) {
                $scope.results = response.data;
                $scope.loading = false;
                $scope.searched = true;
            }, function (response) {
                alert('error sending request');
                $scope.loading = false;
            });
    };

}]);