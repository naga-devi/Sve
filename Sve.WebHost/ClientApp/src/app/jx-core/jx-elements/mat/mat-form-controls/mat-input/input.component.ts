import {
	Component,
	OnInit,
	ChangeDetectionStrategy,
	Input,
	EventEmitter,
	Output,
} from "@angular/core";
import { JxFormControlBaseComponent } from "../_base-component/form-control-base.component";

/**
 * <jx-mat-input [label]="'label'" [type]="'text'" [placeholder]="'placeholder'" [readonly]="true" [control]="fConfig.fGroup('cntrlname')" [appearance]="'outline'"></jx-mat-input>
 */
@Component({
	selector: "jx-mat-input",
	templateUrl: "./input.component.html",
	styleUrls: ['./input.component.scss'],
	changeDetection: ChangeDetectionStrategy.OnPush,
})
export class JxMatInputComponent extends JxFormControlBaseComponent implements OnInit {
	@Input() public type: "text" | "nuber" | "email" = "text";
	@Input() public showClearButton: boolean = false;
	@Output() onChange: EventEmitter<any> = new EventEmitter();
	@Output() onKeyUp: EventEmitter<any> = new EventEmitter();
	public ngOnInit() {
		if (this.placeholder && this.placeholder.length === 0) {
			this.placeholder = this.label;
		}
		if (this.type && this.type.length === 0) {
			this.type = "text";
		}
	}

	/**
	 * Checking control validation
	 *
	 * @param validationType: string => Equals to valitors name
	 */
	isControlHasError(validationType: string): boolean {
		if (!this.control) {
			return false;
		}

		const result = this.control.hasError(validationType) && (this.control.dirty || this.control.touched);
		return result;
	}
}
