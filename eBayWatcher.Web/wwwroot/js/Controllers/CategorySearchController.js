
eBayWatcher.controller('CategorySearchController', ['$scope', '$http', '$rootScope', 'DataService', function ($scope, $http, $rootScope, DataService) {

    $scope.searchTerm = '';
    $scope.selectedCategory = { id: null, name: '[Please select category]' };
    $scope.visible = false;
    $scope.results = [];
    $scope.loading = false;

    $rootScope.$on('category:open', function () {
        console.log('Opening select category window...');
        $scope.visible = true;
        document.getElementById("categorySearchBox").focus();
    });

    $scope.backdropClicked = function () {
        $scope.visible = false;
    }

    $scope.contentClicked = function ($event) {
        $event.stopPropagation();
    }

    $scope.search = function () {
        $scope.loading = true;
        $http.post(DataService.baseUrl + '/Categories/Search', { searchTerm: $scope.searchTerm, eBayToken: getCookie('eBayWatcherToken') })
            .then(function (response) {
                $scope.results = response.data;
                $scope.loading = false;
                $scope.searched = true;
            }, function (response) {
                alert('error sending request');
                $scope.loading = false;
            });
    }

    $scope.selectItem = function (item) {
        $rootScope.$broadcast("category:selected", item);
        $scope.visible = false;
    }

}]);