/**
 * @license Copyright (c) 2003-2023, CKSource Holding sp. z o.o. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
	// config.uiColor = '#AADC6E';
    config.syntaxhighlight_lang = 'csharp';
    config.syntaxhighlight_hideControls = true;
    config.language = 'vi';
    config.filebrowserBrowseUrl = '/Content/Admin/ckfinder/ckfinder.html';
    config.filebrowser = ImageBrowseUrl = '/Content/Admin/ckfinder/ckfinder.html?Type=Images';
    config.filebrowserFlashBrowseUrl = 'Content/Admin/ckfinder/ckfinder.html?Type=Flash';
    config.filebrowserUploadUrl = '/Content/Admin/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files';
    config.filebrowserImageUploadUrl = '/Data/Upload/';
    config.filebrowserFlashUploadUrl = '/Content/Admin/ckfinder/core/connector/aspx/connector.aspx?command-QuickUpload&type=Flash';

    CKFinder.setupCKEditor(null, '/Content/Admin/ckfinder');
};
