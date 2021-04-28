import { Pipe, PipeTransform } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';

/**
 * Truncate Pipe with optional params:
 * Usage : {{longStr | empty }}
 * {{longStr | empty : '-' }}
 */
@Pipe({
    name: 'empty'
})
export class NullOrEmptyPipe implements PipeTransform {

    constructor(private _sanitizer: DomSanitizer) { }
    
    transform(value: string, ellipsis = '-') {
        const formattedHtml =  this.isEmpty(value) ? `<span class="text-center" style="width:100%;">${ellipsis}</span>` : value;

        return this._sanitizer.bypassSecurityTrustHtml(formattedHtml);        
    }

     isEmpty (value : any) {
        return (
          // null or undefined
          (value == null) ||
      
          // has length and it's zero
          (value.hasOwnProperty('length') && value.length === 0) ||
      
          // is an Object and has no keys
          (value.constructor === Object && Object.keys(value).length === 0)
        )
      }
}
