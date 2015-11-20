
eBayWatcher.controller('SearchController', ['$scope', '$http', '$rootScope', 'DataService', function ($scope, $http, $rootScope, DataService) {

    $scope.loading = true;
    $scope.searched = false;
    $scope.wishlistItem = null;

    $rootScope.$on('item:search', function (details, item) {
        $scope.wishlistItem = item;
        $scope.search(item.SearchText, item.CategoryId);
    });

    $scope.search = function (searchTerm, categoryId) {
        $scope.loading = true;
        $http.post(DataService.baseUrl + '/EbaySearch', { searchTerm: searchTerm, categoryId: categoryId })
            .then(function (response) {
                $scope.results = response.data;
                $scope.loading = false;
                $scope.searched = true;
            }, function (response) {
                $scope.loading = false;
            });
    };

    $scope.ignore = function (ebayListing) {
        ebayListing.Ignore = true;
        DataService.ignore($scope.wishlistItem, ebayListing);
    }
    $scope.pin = function (ebayListing) {
        ebayListing.Pin = true;
        DataService.pin($scope.wishlistItem, ebayListing);
    }
    $scope.unpin = function (ebayListing) {
        ebayListing.Pin = false;
        DataService.unpin($scope.wishlistItem, ebayListing);
    }

}]);