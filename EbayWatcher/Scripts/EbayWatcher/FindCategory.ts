
module FindCategory {

    if ($('.find-category-button').length < 1)
        return;
    
    Init();
    
    function Init() {
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
    }
}