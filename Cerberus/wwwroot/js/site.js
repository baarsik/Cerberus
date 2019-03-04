$(document).ready(function() {
    updateQuotationPanelStyle();
});

function updateQuotationPanelStyle() {
    $('.panel').each(function(i) {
        if ($(this).children('.panel-top-label').length === 0)
            return true;

        var additiveMargin = $(this).children('.panel-top-label').first().height() / 2;
        $(this).css('position', 'relative');
        $(this).css('top', function(index, currentValue) {
            var currentMargin = parseInt(currentValue, 10);
            return currentMargin + additiveMargin + 'px';
        });
        $(this).css('margin-bottom', function(index, currentValue) {
            var currentMargin = parseInt(currentValue, 10);
            return currentMargin + additiveMargin + 'px';
        });

        var firstActualChild = $(this).children(':not(.panel-top-label)').first();
        firstActualChild.css('position', 'relative');
        firstActualChild.css('top', function(index, currentValue) {
            var currentMargin = parseInt(currentValue, 10);
            return currentMargin - additiveMargin + 'px';
        });
        firstActualChild.css('margin-bottom', function(index, currentValue) {
            var currentMargin = parseInt(currentValue, 10);
            return currentMargin - additiveMargin + 'px';
        });
    });
}