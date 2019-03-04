var PanelAccentFilled = function (context) {
    var ui = $.summernote.ui;
    var button = ui.button({
        contents: '<i class="fas c-accent fa-info-square"/>',
        tooltip: 'Insert info panel (accent)',
        click: function () {
            var selectedText = context.invoke('editor.createRange').toString();
            if (selectedText.trim().length === 0) {
                selectedText = "Insert your text here";
            }
            context.invoke('editor.pasteHTML', '<div class="panel panel-filled panel-c-accent"><div class="panel-heading">Information</div><div class="panel-body"><div>' + selectedText + '</div></div></div>');
        }
    });
    return button.render();
};

var PanelInfoFilled = function (context) {
    var ui = $.summernote.ui;
    var button = ui.button({
        contents: '<i class="fas c-info fa-info-square"/>',
        tooltip: 'Insert info panel (info)',
        click: function () {
            var selectedText = context.invoke('editor.createRange').toString();
            if (selectedText.trim().length === 0) {
                selectedText = "Insert your text here";
            }
            context.invoke('editor.pasteHTML', '<div class="panel panel-filled panel-c-info"><div class="panel-heading">Information</div><div class="panel-body"><div>' + selectedText + '</div></div></div>');
        }
    });
    return button.render();
};

var PanelSpeech = function (context) {
    var ui = $.summernote.ui;
    var button = ui.button({
        contents: '<i class="fas fa-quote-left"/>',
        tooltip: 'Insert speech panel',
        click: function () {
            var selectedText = context.invoke('editor.createRange').toString();
            if (selectedText.trim().length === 0) {
                selectedText = "Insert your text here";
            }
            context.invoke('editor.pasteHTML', '<div class="panel panel-speech"><div class="panel-top-label">Speaker</div><div class="panel-body"><div>' + selectedText + '</div></div></div>');
        }
    });
    return button.render();
};

$(document).ready(function() {
    $('.summernote').summernote({
        callbacks: {
            onPaste: function(e) {
                // Paste plain text with no duplicated line breaks
                var bufferText = ((e.originalEvent || e).clipboardData || window.clipboardData).getData('Text').replace(/\n\s*\n/g, '\n');
                e.preventDefault();
                document.execCommand('insertText', false, bufferText);
            }
        },
        toolbar: [
            ['style', ['style', 'bold', 'italic', 'underline', 'clear', 'paragraph']],
            ['font', ['superscript', 'subscript']],
            ['color', ['color']],
            ['insert', ['picture', 'table']],
            ['panels', ['panelAccent', 'panelInfo', 'panelSpeech']]
        ],
        buttons: {
            panelAccent: PanelAccentFilled,
            panelInfo: PanelInfoFilled,
            panelSpeech: PanelSpeech
        }
    });
});