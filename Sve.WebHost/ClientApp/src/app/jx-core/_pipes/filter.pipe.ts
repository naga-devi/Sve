import { Pipe, PipeTransform } from '@angular/core';

/** filter the values from object of array
 *  Usage: <li *ngFor="let item of _items | filter:{ label: filterText, description: filterText } : false">
 * {{ item.value }} - {{ item.label }} - {{ item.description }}</li>
 */

@Pipe({
    name: 'filter'
})
export class FilterPipe implements PipeTransform {
  transform(items: any, filter: any, isAnd: boolean): any {
    if (filter && Array.isArray(items)) {
      const filterKeys = Object.keys(filter);
      if (isAnd) {
        return items.filter(item =>
            filterKeys.reduce((memo, keyName) =>
                (memo && new RegExp(filter[keyName], 'gi').test(item[keyName])) || filter[keyName] === '', true));
      } else {
        return items.filter(item => {
          return filterKeys.some((keyName) => {
            console.log(keyName);
            return new RegExp(filter[keyName], 'gi').test(item[keyName]) || filter[keyName] === '';
          });
        });
      }
    } else {
      return items;
    }
  }
}
