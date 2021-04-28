import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'filterById'
})
export class FilterByIdPipe implements PipeTransform {
    transform(items: Array<any>, id?, key?) {
        if (key)
            return items.filter(item => item[key] == id)[0];
        return items.filter(item => item.id == id)[0];
    }
}

@Pipe({
    name: 'filterByKey'
})
export class FilterByKeyPipe implements PipeTransform {
    transform(items: Array<any>, key, value) {
        if (items && key && value)
            return items.filter(item => item[key] == value);

        return [];
    }
}
