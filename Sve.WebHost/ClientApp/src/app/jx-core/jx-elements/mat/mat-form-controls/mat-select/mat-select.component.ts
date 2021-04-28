import {
	Component,
	EventEmitter,
	Input,
	OnChanges,
	Output,
	SimpleChanges,
	OnInit,
	OnDestroy,
	ChangeDetectionStrategy,
	forwardRef,
	ViewChild,
} from "@angular/core";
import { Subscription, Subject, BehaviorSubject } from "rxjs";
import { finalize } from "rxjs/operators";
import { FormControl, NG_VALUE_ACCESSOR } from "@angular/forms";
import { HttpService } from "../../../../_services/http/httpService";
import { MatSelect } from "@angular/material/select";
import { MatOption } from "@angular/material/core";
import { JxBaseComponent } from "../../../_base/base.component";
import { isNullOrEmpty } from "../../../../util";
import { Utilities } from "../../../../utilities/utilities";
import { LayoutUtilsService, NotificationService } from "../../../../_services";

/**
 * Ref: https://stackblitz.com/edit/reusable-fom-control?file=app%2Fmy-select%2Fmy-select.component.ts
 * Usage:
 * <kt-jx-mat-select [label]="[urLabel]" (selectChange)="urMethod($event)" [(selectedValue)]="[urObject]" [(selectedItem)]="[urObject]"
   [url]="[apiUrl]" [valueField]="[urvalueField]" [textField]="[urtextField]" [dataSource]="[urdataSource]"
   [multiple]="true" [placeholder]="'[yourplaceholder]'" [control]="'[yourcontrolid]'" [loadOnInit]="false"></kt-jx-mat-select>
 * */

@Component({
	selector: "kt-jx-mat-select",
	templateUrl: "./mat-select.component.html",
	styleUrls: ['./mat-select.component.scss'],
	providers: [
		{
			provide: NG_VALUE_ACCESSOR,
			useExisting: forwardRef(() => JxMatSelectComponent),
			multi: true,
		},
	],
	changeDetection: ChangeDetectionStrategy.OnPush,
})
export class JxMatSelectComponent extends JxBaseComponent implements OnChanges, OnInit, OnDestroy {
	@ViewChild('select') select: MatSelect;
	// Private properties
	private subscriptions: Subscription[] = [];

	@Input() public label: string;
	@Input() public placeholder: string;
	@Input() public readonly: boolean = false;
	@Input() public control: FormControl;
	@Input() public appearance: "legacy" | "fill" | "standard" | "outline" =
		"outline";

	@Input() selectedValue: any;
	@Input() selectedItem: any;
	@Input() url = "";
	@Input() valueField = "";
	@Input() textField = "";
	@Input() dataSource: any[] = [];
	//@Input() dataSource: BehaviorSubject<any[]> = new BehaviorSubject<any[]>(null);
	@Input() firstItem = "Please Select";
	@Output() selectChange = new EventEmitter<any>();
	@Input() multiple = false;
	@Input() showHint = true;
	@Input() showDefault = true;	
	@Input() prependItem : any = {};	
	@Input() appendItem : any = {};	
	@Input() loadOnInit = false;
	@Input() loadOnChange = false;
	@Input() serverProcess = true;

	//private properties
	allSelected=false;

	// list of subjects filtered by search keyword
	public filteredItems: BehaviorSubject<any[]> = new BehaviorSubject<any[]>(null);

	// tslint:disable-next-line:max-line-length
	baseResultItems: any[] = [];

	// Subject that emits when the component has been destroyed.
	private onDestroy = new Subject<void>();

	constructor(
		private _httpService: HttpService,
		private alert: NotificationService,
		private layoutUtilsService: LayoutUtilsService
	) {
		super();
	}

	ngOnInit(): void {
		if(this.selectedValue !== undefined){

			if (!this.control) {
				this.control = new FormControl();
			}
			this.control.setValue(this.selectedValue);
		}

		if (this.baseResultItems.length === 0) {
			if (!this.serverProcess) {
				this.baseResultItems = this.dataSource;
				this.filteredItems.next(this.baseResultItems);
			} 
			else if(this.loadOnInit){
				this.loadData();
			}
		}
	}
	ngOnDestroy() {
		this.onDestroy.next();
		this.onDestroy.complete();
		this.subscriptions.forEach((sb) => sb.unsubscribe());
	}

	loadData() {
		if(!this.serverProcess){
			this.baseResultItems = this.dataSource;
			this.filteredItems.next(this.baseResultItems);
		}
		else if(
			this.url &&
			this.url.length > 0 &&
			this.textField &&
			this.textField.length > 0 &&
			this.valueField &&
			this.valueField.length > 0
		) {			
			this.layoutUtilsService.startLoadingMessage();
			const prerequisiteSubscription = this._httpService
				.get(this.url)
				.pipe(
					finalize(() => {
						this.layoutUtilsService.stopLoadingMessage();
						this.takeUntilDestroy();
					})
				)
				.subscribe(
					(responseList) => {

						if(!isNullOrEmpty(this.prependItem)){
							this.baseResultItems.push(this.prependItem);
							this.baseResultItems.push(responseList);
						}
						else if(!isNullOrEmpty(this.appendItem)){
							this.baseResultItems = responseList;
							this.baseResultItems.push(this.appendItem);
						}
						else{
							this.baseResultItems = responseList;
						}
						if(this.baseResultItems)
							this.filteredItems.next(this.baseResultItems);
						
					},
					(error) => {
						this.alert.error(
							Utilities.getHttpErrorMessage(error)
						);
					}
				);
			this.subscriptions.push(prerequisiteSubscription);
		}
	}

	//Alternative to optionClick() method
	optionSelected(_event: any, selectedItem: any) {
		if (_event.isUserInput) {
			//this.loadOnChange = false;
			//do something
			this.selectedValue = selectedItem[this.valueField];
			this.selectedItem = selectedItem;
			//this.message = selectedItem.name;
			this.selectChange.emit(selectedItem);
		}
	}

	toggleAllSelection() {
		let selectedItemIds=[];
		if (this.allSelected) {
		  this.select.options.forEach((item: MatOption) => {
			  item.select();
			  selectedItemIds.push(item.value);
			});
		} else {
		  this.select.options.forEach((item: MatOption) => item.deselect());
		}

		this.selectedValue = selectedItemIds;
		this.selectedItem = selectedItemIds;// NEED TO BE ENHANCE
		this.selectChange.emit(selectedItemIds);
	}

	optionClick(selectedItem: any) {
		if(this.multiple){
			let selectedItemIds=[];
			let newStatus = true;
			this.select.options.forEach((item: MatOption) => {
			if(item.selected){
				selectedItemIds.push(item.value);
			}
			if (!item.selected) {
				newStatus = false;
			}
			});
			this.allSelected = newStatus;

			this.selectedValue = selectedItemIds;
			this.selectedItem = selectedItemIds;// NEED TO BE ENHANCE
			this.selectChange.emit(selectedItemIds);
			//console.log(selectedItemIds);
		}
		else{
			this.selectedValue = selectedItem[this.valueField];
			this.selectedItem = selectedItem;
			this.selectChange.emit(selectedItem);
		}
	}

	ngOnChanges(changes: SimpleChanges) {
		let firstChange = true;
		for (const propName in changes) {
			if (changes.hasOwnProperty(propName)) {
				let change = changes[propName];
				//console.log(`${propName}:`, change.currentValue);
				firstChange = change.firstChange;
				//   switch (propName) {
				// 	case 'pageSize': {
				// 	  console.log(`pageSize changed to:`, change.currentValue);
				// 	}
				//   }
			}
		}

		if (!firstChange && this.loadOnChange) {
			this.loadData();
		}
		else if(this.loadOnChange){
			this.loadData();
		}
	}

	public onFilterChange = (search: string) => {
		if (!this.baseResultItems) {
			return;
		}

		// get the search keyword
		if (!search) {
			this.filteredItems.next(this.baseResultItems.slice());
			return;
		} else {
			search = search.toLowerCase();
		}

		// filter the items
		this.filteredItems.next(
			this.baseResultItems.filter(
				(item) =>
					item[this.textField].toLowerCase().indexOf(search) > -1
			)
		);
	};

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

	public refreshLocalDataSource(source: any){
		this.dataSource = source;
	}
}
