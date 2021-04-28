import { Pipe, PipeTransform } from '@angular/core';

/**
 * Truncate Pipe with optional params:
 * Usage : {{longStr | truncate }}
 * {{longStr | truncate : 12 }}
 * {{longStr | truncate : 12 : true }}
 * {{longStr | truncate : 12 : false : '***' }}
 */
@Pipe({
    name: 'truncate'
})
export class TruncatePipe implements PipeTransform {
    transform(value: string, limit = 25, completeWords = false, ellipsis = '...') {
        value = value || '';

        if (value) {
			if (completeWords) {
				limit = value.substr(0, limit).lastIndexOf(' ');
			}
			return value.length > limit ? value.substr(0, limit) + ellipsis : value;
        }

        return '';
    }
}
