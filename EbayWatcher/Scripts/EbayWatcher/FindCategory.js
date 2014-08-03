var FindCategory;
(function (FindCategory) {
    if ($('.find-category-button').length < 1)
        return;

    Init();

    function Init() {
        $('.find-category-button').click(function () {
            alert('test');
        });
    }
})(FindCategory || (FindCategory = {}));
//# sourceMappingURL=FindCategory.js.map
