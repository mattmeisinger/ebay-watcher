
window.ebayWatcherApp.controller('UserController', function ($scope) {

    $scope.isLoggedIn = false;
    setTimeout(function () { $scope.isLoggedIn = true }, 3000);

    $scope.phones = [
        {
            'name': 'Nexus S',
            'snippet': 'Fast just got faster with Nexus S.'
        },
        {
            'name': 'Motorola XOOM™ with Wi-Fi',
            'snippet': 'The Next, Next Generation tablet.'
        },
        {
            'name': 'MOTOROLA XOOM™',
            'snippet': 'The Next, Next Generation tablet.'
        }
    ];
}); 