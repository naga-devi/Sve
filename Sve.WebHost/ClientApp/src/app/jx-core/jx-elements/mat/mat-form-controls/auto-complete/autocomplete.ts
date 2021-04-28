import {
	Component, EventEmitter, Input, Output,
	OnInit, OnDestroy, ChangeDetectionStrategy, forwardRef
} from '@angular/core';
import { Subscription, Observable, of } from 'rxjs';
import { finalize, debounceTime, switchMap, startWith, map, catchError } from 'rxjs/operators';
import { NG_VALUE_ACCESSOR, FormControl } from '@angular/forms';
import { HttpService } from '../../../../_services/http/httpService';

export class AutoCompleteItems {
	name: string;
	id: number;
}

/**
 * Usage:
 * <kt-jx-autocomplete [url]="yourUrl" [placeholder]="placeholder" [value]="[value]" [selectedItem]="selectedItem" (selectChange)="yourMethod($event)"></kt-jx-autocomplete>
 * */
@Component({
	selector: 'kt-jx-autocomplete',
	//templateUrl: './autocomplete.html',
	styles:[`.w-100{width:100% !important;}`],
	template: `<mat-form-field appearance="outline" class="mat-form-field-fluid w-100">
				<mat-label>{{label}}</mat-label>
		<input [formControl]="control" type="text" [placeholder]="placeholder" style="width: 100%;" aria-label="Number" matInput [matAutocomplete]="auto">
		<mat-autocomplete autoActiveFirstOption #auto="matAutocomplete" style="margin-top: 30px; max-height: 600px">
			<mat-option *ngIf="isLoading" class="is-loading">Loading...</mat-option>
			<ng-container *ngIf="!isLoading">
				<mat-option *ngFor="let item of resultItems$ | async; let index=index" [value]="item[valueField]" (onSelectionChange)="optionSelected(item)">
					{{ item[textField]}}
				</mat-option>
			</ng-container>
		</mat-autocomplete>
	</mat-form-field>`,
	providers: [
		{
			provide: NG_VALUE_ACCESSOR,
			useExisting: forwardRef(() => JxAutocompleteComponent),
			multi: true
		}
	],
	changeDetection: ChangeDetectionStrategy.OnPush
})
export class JxAutocompleteComponent implements OnInit, OnDestroy {

	// Private properties
	private subscriptions: Subscription[] = [];

	//control = new FormControl();
	@Input() public control: FormControl;
	resultItems$: Observable<any[]> = null;
	isLoading = false;

	@Input() url:string ="";
	@Input() placeholder = 'Search user';
	@Output() selectChange = new EventEmitter<any>();
	@Input() valueField = "";
	@Input() textField = "";
	@Input() label = "";

	//messageValue: string;
	//@Output()
	//messageChange = new EventEmitter<string>();

	//@Input()
	//get message() {
	//	return this.messageValue;
	//}

	//set message(val) {
	//	this.messageValue = val;
	//	this.messageChange.emit(this.messageValue);
	//}

	constructor(private _httpService: HttpService) { }

	ngOnInit(): void {
		this.resultItems$ = this.control.valueChanges.pipe(
			startWith(''),
			// delay emits
			debounceTime(300),
			//tap(() => this.isLoading = true),
			// use switch map so as to cancel previous subscribed events, before creating new once
			switchMap(value => {
				if (value !== '' && value.length > 2) {//
					this.isLoading = true;
					// lookup from github
					return this.lookup(value);
				} else {
					// if no value is present, return null
					return of(null);
				}
			})
		);

	}

	lookup(value: string): Observable<any[]> {
		return this._httpService.get(`${this.url}` + value.toLowerCase())
			.pipe(
				finalize(() => {
					this.isLoading = false;
				}),
				// map the item property of the github results as our return object
				map(results => results),
				// catch errors
				catchError(_ => {
					return of(null);
				})
			);
	}

	optionSelected(selectedItem: any) {
		//this.message = selectedItem.name;
		this.control.patchValue(selectedItem.name);
		this.selectChange.emit(selectedItem);
	}

	displayFn(user: any) {
		if (user) { return user.name; }
	}

	ngOnDestroy() {
		this.subscriptions.forEach(sb => sb.unsubscribe());
	}
}
