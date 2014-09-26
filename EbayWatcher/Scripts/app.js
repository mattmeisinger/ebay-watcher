var ebayWatcherApp = angular.module('ebayWatcherControllers', []);

ebayWatcherApp.config(['$routeProvider',
  function ($routeProvider) {
      $routeProvider.
        when('/Wishlist', {
            templateUrl: '/Wishlist',
            controller: 'WishlistController'
        }).
        otherwise({
            redirectTo: '/Account/Login'
        });
  }]);