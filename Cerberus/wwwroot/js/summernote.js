const PanelAccentFilled = (context) => {
    let ui = $.summernote.ui;
    let button = ui.button({
        contents: '<i class="fas c-accent fa-info-square"/>',
        tooltip: 'Insert info panel (accent)',
        click: () => {
            let selectedText = context.invoke('editor.createRange').toString();
            if (selectedText.trim().length === 0) {
                selectedText = "Insert your text here";
            }
            context.invoke('editor.pasteHTML', '<div class="panel panel-filled panel-c-accent"><div class="panel-heading">Information</div><div class="panel-body"><div>' + selectedText + '</div></div></div>');
        }
    });
    return button.render();
};

const PanelInfoFilled = function (context) {
    let ui = $.summernote.ui;
    let button = ui.button({
        contents: '<i class="fas c-info fa-info-square"/>',
        tooltip: 'Insert info panel (info)',
        click: () => {
            let selectedText = context.invoke('editor.createRange').toString();
            if (selectedText.trim().length === 0) {
                selectedText = "Insert your text here";
            }
            context.invoke('editor.pasteHTML', '<div class="panel panel-filled panel-c-info"><div class="panel-heading">Information</div><div class="panel-body"><div>' + selectedText + '</div></div></div>');
        }
    });
    return button.render();
};

const PanelSpeech = (context) => {
    let ui = $.summernote.ui;
    let button = ui.button({
        contents: '<i class="fas fa-quote-left"/>',
        tooltip: 'Insert speech panel',
        click: () => {
            let selectedText = context.invoke('editor.createRange').toString();
            if (selectedText.trim().length === 0) {
                selectedText = "Insert your text here";
            }
            context.invoke('editor.pasteHTML', '<div class="panel panel-speech"><div class="panel-top-label">Speaker</div><div class="panel-body"><div>' + selectedText + '</div></div></div>');
        }
    });
    return button.render();
};

function activateBasicSummernote(selector) {
    $(selector).summernote({
        callbacks: {
            onPaste: (e) => {
                // Paste plain text with no duplicated line breaks
                let bufferText = ((e.originalEvent || e).clipboardData || window.clipboardData)
                    .getData('Text')
                    .replace(/\n\s*\n/g, '\n');
                e.preventDefault();
                document.execCommand('insertText', false, bufferText);
            }
        },
        toolbar: [
            ['style', ['bold', 'italic', 'underline', 'clear']]
        ]
    });
}

function attachBlazorToSummernote(selector, dotNetComponent) {
    $(selector).on('summernote.change', (we, contents, $editable) => {
        dotNetComponent.invokeMethodAsync('OnTextChange', contents);
    });
}

function activateRichSummernote(selector) {
    $(selector).summernote({
        callbacks: {
            onPaste: (e) => {
                // Paste plain text with no duplicated line breaks
                let bufferText = ((e.originalEvent || e).clipboardData || window.clipboardData)
                    .getData('Text')
                    .replace(/\n\s*\n/g, '\n');
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
}

function resetSummernote(selector) {
    $(selector).summernote('reset');
}

$(document).ready(function() {
    activateRichSummernote('.summernote:not(.blazor)');
    activateBasicSummernote('.summernote.summernote-comments.blazor');
});