
eBayWatcher.factory('DataService', ['$http', '$rootScope', function ($http, $rootScope) {

    var service = {
        baseUrl: '', /* Use the current web app */
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
        if (wishlistItem.IgnoredItemIds.indexOf(ebayListing.Id) === -1) {
            console.log("Adding rule to ignore ebay listing " + ebayListing.Id + " to wishlist item " + wishlistItem.Id);
            wishlistItem.IgnoredItemIds.push(ebayListing.Id);
            this.saveWishlistItem(wishlistItem);
        }
        else {
            console.log("Rule to ignore ebay listing " + ebayListing.Id + " already exists on wishlist item " + wishlistItem.Id + ". Ignoring.");
        }
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

    // Adds or updates wishlist item
    service.saveWishlistItem = function (wishlistItem) {
        console.log('Saving wishlist item ' + wishlistItem.Id);

        return $http.post(this.baseUrl + '/WatchListItems', wishlistItem)
            .then(function (response) {
                var savedItem = response.data;
                service.saving = false;
                service.list.push(savedItem);
            }, function (response) {
                service.saving = false;
            });
    }

    return service;

}]);