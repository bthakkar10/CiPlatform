tinymce.init({
    selector: '#CmsDescriptionEditor',
    height: 250,
    plugins: 'lists code emoticons',
    toolbar: ' bold italic strikethrough| ' +
        'subscript superscript underline| ',
    content_style: 'body { font-family:Helvetica,Arial,sans-serif; font-size:16px }'
});