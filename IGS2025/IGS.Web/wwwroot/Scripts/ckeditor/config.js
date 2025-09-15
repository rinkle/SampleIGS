/**
 * @license Copyright (c) 2003-2021, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

CKEDITOR.editorConfig = function( config ) {
    // Define changes to default configuration here. For example:
    // config.language = 'fr';
    // config.uiColor = '#AADC6E';
    config.enterMode = CKEDITOR.ENTER_P,
    config.enterMode = CKEDITOR.ENTER_Span,
    config.shiftEnterMode = CKEDITOR.ENTER_P,

    CKEDITOR.config.allowedContent = true,
    CKEDITOR.dtd.$removeEmpty.span = 0,
    CKEDITOR.dtd.$removeEmpty.i = 0,
    config.removePlugins = 'save'
	
};
