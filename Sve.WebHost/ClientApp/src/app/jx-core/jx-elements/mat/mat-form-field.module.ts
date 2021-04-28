import { NgModule } from "@angular/core";
import { JxAutocompleteComponent } from "./mat-form-controls/auto-complete/autocomplete";
import { JxMatTableComponent } from "./mat-table/data-table.component";
import { JxMatInputComponent } from "./mat-form-controls/mat-input/input.component";
import { JxMatDatePickerComponent } from "./mat-form-controls/mat-datepicker/date-picker.component";
import { JxMatSelectComponent } from "./mat-form-controls/mat-select/mat-select.component";
import { MatSelectSearchModule } from "./mat-form-controls/mat-select-search/mat-select-search.module";
import { JxSharedModule, MaterialModule } from "../../shared";
import { ControlMessagesComponent } from "./form-validation/control-messages.component";
import { JxMatFormComponent } from "./mat-form-controls/mat-form/form.component";
import { TranslateModule } from "@ngx-translate/core";
import { JxMatRisedButtonComponent } from "./button/mat-raised-button";
import { JxMatDialogCloseButtonComponent } from "./button/mat-dialog-close-button";
import {JxRow} from './row-col/row';
import {JxColLG3,
	JxColLG3FG,
	JxColLG6,
	JxColLG6FG,
	JxColLG12,
	JxColLG12FG} from './row-col/col';

@NgModule({
	declarations: [
		JxMatInputComponent,
		JxMatTableComponent,
		JxAutocompleteComponent,
		JxMatSelectComponent,
		ControlMessagesComponent,
		JxMatFormComponent,
		JxMatDatePickerComponent,
		JxMatRisedButtonComponent,
		JxMatDialogCloseButtonComponent,
		JxRow,
		JxColLG3,
		JxColLG3FG,
		JxColLG6,
		JxColLG6FG,
		JxColLG12,
		JxColLG12FG,
	],
	exports: [
		JxMatInputComponent,
		MatSelectSearchModule,
		JxMatTableComponent,
		JxAutocompleteComponent,
		JxMatSelectComponent,
		ControlMessagesComponent,
		JxMatFormComponent,
		JxMatDatePickerComponent,
		JxMatRisedButtonComponent,
		JxMatDialogCloseButtonComponent,
		JxRow,
		JxColLG3,
		JxColLG3FG,
		JxColLG6,
		JxColLG6FG,
		JxColLG12,
		JxColLG12FG,
	],
	imports: [
		MatSelectSearchModule,
		JxSharedModule,
		MaterialModule,
		TranslateModule.forChild(),
	],
})
export class JxMatFormControlsModule {}
