import {
    Component, Input, ChangeDetectionStrategy, OnInit, EventEmitter, Output} from '@angular/core';

/**
 * Usage:
 * <jx-mat-dialog-close-button (onClick)="urMethod()"></jx-mat-dialog-close-button>
 * */
@Component({
    selector: 'jx-mat-dialog-close-button',
    templateUrl: './mat-dialog-close-button.html',
})
export class JxMatDialogCloseButtonComponent {
    @Input() title='CANCEL';
    @Input() tooltip='Cancel dialog';
    @Input() disabled : false;
    @Output() onClick = new EventEmitter<void>();
}
