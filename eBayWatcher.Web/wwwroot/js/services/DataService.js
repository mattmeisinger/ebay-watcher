
eBayWatcher.factory('DataService', ['$http', '$rootScope', function ($http, $rootScope) {

    var service = {
        baseUrl: 'https://ebaywatcherwebapi.azurewebsites.net',
        notifyAllOfItemUpdated: function (item) {
            $rootScope.$broadcast('item:updated', item);
        }
    };

    service.setItem = function (myListItem) {
        this.currentListItem = myListItem;
    };

    service.ignore = function (wishlistItem, ebayListing) {
        console.log('ignoring');
        console.log(wishlistItem);
        console.log(ebayListing);
    };

    service.pin = function (wishlistItem, ebayListing) {
        console.log('pinning');
        console.log(wishlistItem);
        console.log(ebayListing);
    };

    service.unpin = function (wishlistItem, ebayListing) {
        console.log('unpinning');
        console.log(wishlistItem);
        console.log(ebayListing);
    };

    return service;

}]);