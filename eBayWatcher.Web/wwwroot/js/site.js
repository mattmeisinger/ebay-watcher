// Write your Javascript code.
$.ajax({
    type: 'POST',
    url: 'https://ebaywatcherwebapi.azurewebsites.net/Account',
    data: 'json',
    success: function (d) {
        alert('Got data');
        console.log(d);
    }
});