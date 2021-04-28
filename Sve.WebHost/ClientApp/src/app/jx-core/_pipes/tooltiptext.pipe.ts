import { Pipe, PipeTransform } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';

/**
 * Truncate Pipe with optional params:
 * Usage :
 * <span [innerHTML]="row.Status | tooltiptext"></span>
 * <span [innerHTML]="row.Status | tooltiptext : 12"></span>
 * <span [innerHTML]="row.Status | tooltiptext : 12 : true"></span>
 * <span [innerHTML]="row.Status | tooltiptext : 12 : true : '***' "></span>
 */
@Pipe({
    name: 'tooltiptext'
})
export class TooltipTextPipe implements PipeTransform {
    // tslint:disable-next-line:variable-name
    constructor(private _sanitizer: DomSanitizer) { }
    transform(value: string, limit = 25, completeWords = false, ellipsis = '...') {
        value = value || '';
        let formattedHtml = '';
        if (value) {
            if (completeWords) {
                limit = value.substr(0, limit).lastIndexOf(' ');
            }
            formattedHtml = value.length > limit ? `<span matTooltip="${value}">${value.substr(0, limit) + ellipsis}</span>` : value;
        }

        return this._sanitizer.bypassSecurityTrustHtml(formattedHtml);
    }
}
