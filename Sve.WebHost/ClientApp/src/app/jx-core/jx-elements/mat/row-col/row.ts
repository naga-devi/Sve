import {
    ChangeDetectionStrategy,
    Component, Input, ViewEncapsulation} from '@angular/core';

@Component({
    selector: 'row',
    template:`<div class="row {{cssClass}}"><ng-content></ng-content></div>`,
    changeDetection: ChangeDetectionStrategy.OnPush,
    encapsulation: ViewEncapsulation.None
})
export class JxRow {
    @Input() cssClass='';
}
