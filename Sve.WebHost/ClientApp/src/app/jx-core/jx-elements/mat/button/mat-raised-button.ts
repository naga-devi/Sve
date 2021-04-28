import {
    Component, Input, ChangeDetectionStrategy, OnInit, EventEmitter, Output} from '@angular/core';

/**
 * Usage:
 * <jx-mat-raised-button (onClick)="urMethod()" [title]="'SUBMIT'" [loading]="false" [disabled]="false" [isFilterButton]="true"></jx-mat-raised-button>
 * */
@Component({
    selector: 'jx-mat-raised-button',
    templateUrl: './mat-raised-button.html',
    //styleUrls: ["./info-box.scss"],
})
export class JxMatRisedButtonComponent {
    @Input() public title='SUBMIT';
    @Input() public tooltip='';
    @Input() public color : "primary" | "accent" |"warn" = "primary";
    @Input() public loading : false;
    @Input() public disabled : false;
    @Input() public isFilterButton : false;
    @Output() onClick = new EventEmitter<void>();
}
