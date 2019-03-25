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

function select2SelectOrderingDataAdapter() {

    // Build dependencies
    var ArrayAdapter = jQuery.fn.select2.amd.require('select2/data/array');
    var Utils = jQuery.fn.select2.amd.require('select2/utils');

    function CustomArrayAdapter($element, options) {
        CustomArrayAdapter.__super__.constructor.call(this, $element, options);
    };

    Utils.Extend(CustomArrayAdapter, ArrayAdapter);

    // Add sorting
    CustomArrayAdapter.prototype.current = function (callback) {

        var data = [];

        this.$element.find(':selected').each(jQuery.proxy(function(i, element) {

            var $option = jQuery(element);
            var option = this.item($option);

            data.push(option);
        }, this));

        // Sort by addedOn timestamp
        data = data.sort(function(a, b) {
            return a._addedOn - b._addedOn;
        });

        callback(data);
    };

    // Add timestamp
    CustomArrayAdapter.prototype.select = function(data) {

        data._addedOn = new Date;

        return CustomArrayAdapter.__super__.select.call(this, data);
    };

    // Remove timestamp
    CustomArrayAdapter.prototype.unselect = function(data) {

        data._addedOn = undefined;

        return CustomArrayAdapter.__super__.unselect.call(this, data);
    };

    return CustomArrayAdapter;
}