window.ebayWatcherApp = angular.module('ebayWatcherApp', ['UserController']);

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

$('.find-category-button').click(function () {
    $.ajax({
        url: '/Wishlist/_FindCategory',
        type: 'POST',
        data: {
            searchTerm: $('#Name').val()
        },
        success: function (d) {
            $('#findCategoryWindow').remove();
            $('<div id="findCategoryWindow">').appendTo(document.body);
            var findCategoryWindow = $('#findCategoryWindow').kendoWindow({
                width: 600,
                title: "Find Category",
                actions: [
                    "Close"
                ],
                visible: false
            }).data('kendoWindow');
            findCategoryWindow.content(d);
            findCategoryWindow.center();
            findCategoryWindow.open();

            $('#findCategoryWindow').on('click', '.ebay-category', function (e) {
                e.preventDefault();
                $('#CategoryId').val($(this).data('id'));
                $('#CategoryName').html($(this).data('fullname'));
                findCategoryWindow.destroy();
            });
        }
    });
});

$('.find-ebay-items').click(function (e) {
    e.preventDefault();
    $.ajax({
        url: $(this).attr('href'),
        type: 'POST',
        success: function (d) {
            $('#findEndedWindow').remove();
            $('<div id="findEndedWindow">').appendTo(document.body);
            var findCategoryWindow = $('#findEndedWindow').kendoWindow({
                title: "Find Ebay Items",
                actions: [
                    "Close"
                ],
                visible: false
            }).data('kendoWindow');
            findCategoryWindow.content(d);
            findCategoryWindow.center();
            findCategoryWindow.maximize();
            findCategoryWindow.open();

            $('#findEndedWindow').on('click', '.ebay-category', function (e) {
                e.preventDefault();
                $('#CategoryId').val($(this).data('id'));
                $('#CategoryName').html($(this).data('fullname'));
                findCategoryWindow.destroy();
            });
        }
    });
});