import { Component, Input } from "@angular/core";
import { FormControl } from "@angular/forms";
import { ValidationService } from "./validation.service";

@Component({
	selector: "control-messages",
	template: `<mat-error *ngIf="errorMessage !== null">{{
		errorMessage
	}}</mat-error>`,
})
export class ControlMessagesComponent {
	@Input() control: FormControl;
	constructor() {}

	get errorMessage() {
		if (this.control) {
			for (let propertyName in this.control.errors) {
				if (
					this.control.errors.hasOwnProperty(propertyName) &&
					this.control.touched
				) {
					return ValidationService.getValidatorErrorMessage(
						propertyName,
						this.control.errors[propertyName]
					);
				}
			}
		}

		return null;
	}
}
