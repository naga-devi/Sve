import {
    ChangeDetectionStrategy,
    Component, ViewEncapsulation} from '@angular/core';

@Component({
    selector: 'col-lg-3',
    template:`<div class="col-lg-3"><ng-content></ng-content></div>`
})
export class JxColLG3 {}

@Component({
    selector: 'col-lg-3-fg',
    template:`<div class="col-lg-3 kt-margin-bottom-20-mobile">
    <div class="form-group kt-form__group"><ng-content></ng-content></div></div>`
})
export class JxColLG3FG {}

@Component({
    selector: 'col-lg-6',
    template:`<div class="col-lg-6"><ng-content></ng-content></div>`
})
export class JxColLG6 {}
@Component({
    selector: 'col-lg-6-fg',
    template:`<div class="col-lg-6 kt-margin-bottom-20-mobile">
    <div class="form-group kt-form__group"><ng-content></ng-content></div></div>`,
    changeDetection: ChangeDetectionStrategy.OnPush,
    encapsulation: ViewEncapsulation.None
})
export class JxColLG6FG {}

@Component({
    selector: 'col-lg-12',
    template:`<div class="col-lg-12"><ng-content></ng-content></div>`,
    changeDetection: ChangeDetectionStrategy.OnPush,
    encapsulation: ViewEncapsulation.None
})
export class JxColLG12 {}

@Component({
    selector: 'col-lg-12-fg',
    template:`<div class="col-lg-12 kt-margin-bottom-20-mobile">
    <div class="form-group kt-form__group"><ng-content></ng-content></div></div>`
})
export class JxColLG12FG {}
