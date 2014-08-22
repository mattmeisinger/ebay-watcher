var FindCategory;
(function (FindCategory) {
    if ($('.find-ebay-items').length < 1)
        return;

    Init();

    function Init() {
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
    }
})(FindCategory || (FindCategory = {}));
//# sourceMappingURL=SelectEbayItems.js.map
