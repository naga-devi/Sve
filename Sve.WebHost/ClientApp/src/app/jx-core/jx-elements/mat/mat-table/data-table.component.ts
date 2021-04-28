import {
	Component,
	ViewChild,
	Input,
	Output,
	EventEmitter,
	OnInit,
	OnChanges,
	ElementRef,
	OnDestroy,
	AfterViewInit,
} from "@angular/core";
import { Subscription, of } from "rxjs";
import { HttpService } from "../../../_services/http/httpService";
import { SelectionModel } from "@angular/cdk/collections";
import { finalize } from "rxjs/operators";

import {
	JxMatTableConfig,
	JxPagerModel,
	JxSortModel,
	JxMatTableActionItem,
} from "./mat-table-config";
import { MatSort } from "@angular/material/sort";
import { MatPaginator } from "@angular/material/paginator";
import { isNullOrEmpty } from "../../../util";
import {
	FormBuilder,
	FormControl,
	FormGroup,
	ValidatorFn,
	Validators,
} from "@angular/forms";
import { BaseDataSource } from "../../../models/_base.datasource";
import { LayoutUtilsService, MessageType } from "src/app/jx-core/_services/layout-utils.service";
import { QueryParamsModel, QueryResultsModel, ResponseModel } from "src/app/jx-core/models";
import { Utilities } from "src/app/jx-core/utilities/utilities";
import { ResponseStatus } from "src/app/jx-core/_enums";

export class BaseGrideAddModel {
	// Edit
	_isEditMode: boolean = false;
	// Log
	_userId: number = 0; // Admin
	_createdDate: string;
	_updatedDate: string;
}

export class AddGridItemModel extends BaseGrideAddModel {
	Id: number;
	clear(): void {
		this.Id = 0;
	}
}

@Component({
	selector: "kt-jx-data-table",
	templateUrl: "data-table.component.html",
	styleUrls: ["data-table.component.scss"],
})
export class JxMatTableComponent
	implements OnInit, OnChanges, OnDestroy, AfterViewInit {
	@ViewChild(MatSort, { static: true }) sort: MatSort;
	@ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;

	@ViewChild("searchInput", { static: true }) searchInput: ElementRef;

	@Input() config: JxMatTableConfig;
	@Input() initLoad: boolean = true;
	@Input() title: string = "List";
	@Input() addBtnText: string = "New";
	@Input() editableGrid = false;

	@Output("onAction") onAction = new EventEmitter();
	@Output() onPageChange = new EventEmitter<JxPagerModel>();
	@Output() onSortChange = new EventEmitter<JxSortModel>();
	// @TwoWay() pager: JxPagerModel = {pageIndex: 0, pageSize: 10} ;
	// @TwoWay() sorter: JxSortModel = {sortOrder: 'asc', sortField: 'Name'} ;
	private subscriptions: Subscription[] = [];

	selection = new SelectionModel<Element>(true, []);
	private tempTableResult: any[] = [];
	dataSource = new BaseDataSource();

	formGroup: FormGroup = new FormGroup({});
	// Add and Edit
	isSwitchedToEditMode: boolean = false;
	loadingAfterSubmit: boolean = false;

	itemForEdit: AddGridItemModel;
	itemForAdd: AddGridItemModel;	

	constructor(
		private _fb: FormBuilder,
		private httpService: HttpService,
		private layoutUtilsService: LayoutUtilsService
	) {}

	ngOnInit() {
		this.itemForAdd = new AddGridItemModel();
		if (!this.config) {
			const _config = new JxMatTableConfig();
			_config.clear();
			this.config = _config;
		}
		this.config.list.showPager = true;
		this.config.list.enableActionMatIcons = true;		
	}

	ngAfterViewInit(): void {
		const sortSubscription = this.sort.sortChange.subscribe(() => {
			this.paginator.pageIndex = 0;
			//console.log(this.sort);
			//console.log(this.sort.active);
			const params: JxSortModel = {
				sortOrder: this.sort.direction,
				sortField: this.sort.active,
			};
			this.onSortChange.emit(params);
			this.loadData();
		});
		this.subscriptions.push(sortSubscription);

		const paginatorSubscription = this.paginator.page.subscribe(() => {
			this.paginator.pageIndex = 0;
			//console.log(this.paginator);
			//console.log(this.paginator.pageIndex);
			const params: JxPagerModel = {
				pageIndex: this.paginator.pageIndex,
				pageSize: this.paginator.pageSize,
			};
			this.onPageChange.emit(params);
			this.loadData();
		});
		this.subscriptions.push(paginatorSubscription);

		if (this.config && this.initLoad) {
			this.loadData(true);
		}

		this.buildAddForm();
	}

	ngOnChanges(changes: any): void {
		//if (this.config && !this.config.hasLocalDataSource) {
		//	this.loadData(true);
		//}
	}

	// We will need this getter to exctract keys from displayColumns
	get displayColumns() {
		return (
			this.config &&
			this.config.list.displayColumns &&
			this.config.list.displayColumns.map(({ key }) => key)
		);
	}

	// this function will return a value from column configuration
	// depending on the value that element holds
	showBooleanValue(elt, column) {
		return column.config.values[`${elt[column.key]}`];
	}

	private loadData(firstLoad: boolean = false) {
		this.selection.clear();
		this.layoutUtilsService.startLoadingMessage();
		this.dataSource.loading$ = of(true);

		let listHttpType = 'POST';
		if(this.config.list && this.config.list.httpType){
			listHttpType = this.config.list.httpType;
		}

		if (listHttpType && listHttpType === "GET") {
			const getSub = this.httpService
				.get(this.config.list.url)
				.pipe(
					finalize(() => {
						this.dataSource.loading$ = of(false);
						this.layoutUtilsService.stopLoadingMessage();
					})
				)
				.subscribe(
					(response: QueryResultsModel) => {
						this.formatResponse(response);
						// let resultData =[], total = 0;
						// if(response && Array.isArray(response)){
						// 	resultData =response;
						// 	total = resultData.length;
						// }
						// else{
						// 	resultData = response.items == null ? [] : response.items;
						// 	total = response.totalCount;
						// }
						// this.dataSource.entitySubject.next(resultData);
						// this.dataSource.paginatorTotalSubject.next(total);
						// this.tempTableResult = resultData;
					},
					(error) => {
						this.layoutUtilsService.showActionNotification(
							Utilities.getHttpErrorMessage(error),
							MessageType.Delete
						);
					}
				);
			this.subscriptions.push(getSub);
		} else {
			let queryParams: any;
			if (this.config.list.postData && !isNullOrEmpty(this.config.list.postData)) {
				queryParams = this.config.list.postData;
			} else {
				queryParams = new QueryParamsModel(
					this.filterConfiguration(),
					this.sort.direction === ""
						? this.config.list.sortDirection || "asc"
						: this.sort.direction,
					this.sort.active === undefined
						? this.config.list.sortActive || "ModifiedOn"
						: this.sort.active,
					this.paginator.pageIndex,
					firstLoad
						? 10
						: this.paginator.pageSize === undefined
						? 10
						: this.paginator.pageSize
				);
			}
			const findSubscription = this.httpService
				.post(this.config.list.url, queryParams)
				.pipe(
					finalize(() => {
						this.dataSource.loading$ = of(false);
						this.layoutUtilsService.stopLoadingMessage();
					})
				)
				.subscribe(
					(response: QueryResultsModel) => {
						this.formatResponse(response);
						// let resultData =[], total = 0;
						// if(response && Array.isArray(response)){
						// 	resultData =response;
						// 	total = resultData.length;
						// }
						// else{
						// 	resultData = response.items == null ? [] : response.items;
						// 	total = response.totalCount;
						// }
						// this.dataSource.entitySubject.next(resultData);
						// this.dataSource.paginatorTotalSubject.next(total);
						// this.tempTableResult = resultData;
					},
					(error) => {
						this.layoutUtilsService.showActionNotification(
							Utilities.getHttpErrorMessage(error),
							MessageType.Delete
						);
					}
				);
			this.subscriptions.push(findSubscription);
		}
	}

	private formatResponse(response) {
		let resultData = [],
			total = 0;
		if (response && Array.isArray(response)) {
			resultData = response;
			total = resultData.length;
		} else {
			resultData = response.items == null ? [] : response.items;
			total = response.totalCount;
		}

		resultData.forEach((x) => {
			x["_isEditMode"] = false;
		});

		this.dataSource.entitySubject.next(resultData);
		this.dataSource.paginatorTotalSubject.next(total);
		this.tempTableResult = resultData;
	}

	public refresh(_config: any = {}) {
		if (!this.isEmptyObject(_config)) this.config = _config;
		this.loadData();
	}

	private filterConfiguration(): any {
		let filter: any = {};

		if (!this.isEmptyObject(this.config.list.filter))
			filter = this.config.list.filter;

		let searchText: string =
			this.searchInput && this.searchInput.nativeElement.value;
		if (searchText) {
			searchText = searchText.trim(); // Remove whitespace
			searchText = searchText.toLowerCase(); // Datasource defaults to lowercase matches
			filter[this.config.list.searchColumn] = searchText;
		}
		return filter;
	}

	private isEmptyObject(obj) {
		return obj && Object.keys(obj).length === 0;
	}

	/**
	 * Apply filter on key press
	 */
	public applyFilter(filterValue: string) {
		if (filterValue.trim().length !== 0 && filterValue.trim().length < 3) {
			return false;
		}

		this.paginator.pageIndex = 0;
		this.loadData();
	}

	/** Whether the number of selected elements matches the total number of rows. */
	private isAllSelected(): boolean {
		const numSelected = this.selection.selected.length;
		const numRows = this.tempTableResult.length;
		return numSelected === numRows;
	}

	deleteAll() {
		const idsForDeletion: number[] = [];
		for (let i = 0; i < this.selection.selected.length; i++) {
			idsForDeletion.push(
				this.selection.selected[i][this.config.primaryColumn]
			);
		}

		this.deleteRecords(idsForDeletion);
	}

	private deleteRecords(idsForDeletion: number[]) {
		if (idsForDeletion.length === 0) {
			this.layoutUtilsService.showActionNotification(
				"Please select atleast one record."
			);
			return;
		}

		const deleteUrl = `${this.config.delete.url}/${idsForDeletion}`;
		let deleteLables:any;

		if(this.config.delete && this.config.delete.labels){
			deleteLables = this.config.delete.labels;
		}
		else{
			deleteLables = {
				title: `${this.config.entityName} Delete`,
				description: `Are you sure to permanently delete this ${this.config.entityName}?`,
				waitDesciption: `Deleting...`,
				deleteMessage: `${this.config.entityName} has been deleted`,
			};
		}

		const dialogRef = this.layoutUtilsService.deleteElement(
			deleteLables.title,
			deleteLables.description,
			deleteLables.waitDesciption,
			deleteLables.deleteMessage,
			deleteUrl
		);
		const deleteSubscription = dialogRef
			.afterClosed()
			.subscribe((response) => {
				if (!response) {
					return;
				}
				if (response.Status === ResponseStatus.Success) {
					this.selection.clear();
					this.loadData();
				}
			});
		this.subscriptions.push(deleteSubscription);
	}

	actionClicked(action: string, element: any) {
		const params: JxMatTableActionItem = {
			action: action,
			selectedItem: element,
		};

		if (
			action == "delete" &&
			this.config.delete.url &&
			this.config.delete.url.length > 0
		) {
			const idsForDeletion: number[] = [];
			idsForDeletion.push(element[this.config.primaryColumn]);
			this.deleteRecords(idsForDeletion);
		} else if (
			action == "edit" &&
			this.config.update &&
			this.config.update.url.length > 0 &&
			this.editableGrid
		) {
			this.editItemButtonOnClick(element, 0);
		}

		this.onAction.emit(params);
	}

	exportGrid = () => {
		// var cols = new Array<TableColumn>();
		// this.config.list.displayColumns.forEach((x) => {
		// 	let enablePrint = true;
		// 	if (x.print != undefined && x.print === false) enablePrint = false;
		// 	cols.push(new TableColumn(x.key, x.display, enablePrint));
		// });

		// TableUtil.exportDataToExcelWithCustomHeader(
		// 	this.config.exportFileName,
		// 	this.tempTableResult,
		// 	cols
		// );
	};

	ngOnDestroy() {
		this.subscriptions.forEach((el) => el.unsubscribe());
	}

	buildAddForm() {
		if (!this.editableGrid) return;

		let obj = {};
		//this.addDisplayColumnConfig = this.addColumns();
		if (
			this.config &&
			this.config.list.displayColumns &&
			this.config.list.displayColumns.length > 0
		) {
			const editColsEle = this.config.list.displayColumns.filter(
				(x) => x.editable == true
			);
			if (editColsEle && editColsEle.length > 0) {
				editColsEle.forEach((item) => {
					let controlname = '';
					if(item.editConfig && item.editConfig.inputtype	&& item.editConfig.inputtype === 'select'){
						controlname = item.editConfig && item.editConfig.valueField	? item.editConfig.valueField : 'Id';
					}
					else{
						controlname=`${item.key}`;
					}
					if (item && item.editConfig) {
						if (
							item.editConfig &&
							item.editConfig.validation &&
							item.editConfig.validation.length > 0
						) {
							// this.formGroup.addControl(
							// 	`add${item.key}`, // control name
							// 	new FormControl("", this.getValidators(item.editConfig.validation))
							// );

							obj[`${controlname}`] = new FormControl(
								"",
								this.getValidators(item.editConfig.validation)
							);
						} else {
							// this.formGroup.addControl(
							// 	`add${item.key}`, // control name
							// 	new FormControl("")
							// );

							obj[`${controlname}`] = new FormControl("");
						}
					}
				});

				this.formGroup = this._fb.group(obj);
			}
		}
	}

	// method that returns angular validators from strings
	private getValidators(validatorsList: Array<string>): Array<ValidatorFn> {
		const validators: Array<ValidatorFn> = [];

		// map string validators to angular validators
		validatorsList.forEach((validator: string) => {
			switch (validator) {
				case "required":
					validators.push(Validators.required);
					break;
				case "email":
					validators.push(Validators.email);
					break;
				default:
					break;
			}
		});

		return validators;
	}

	// We will need this getter to exctract keys from displayColumns
	get addColumns() {
		if (!this.editableGrid) return [];

		let addCols = [];
		if (
			this.config &&
			this.config.list.displayColumns &&
			this.config.list.displayColumns.length > 0
		) {
			const editColsEle = this.config.list.displayColumns.filter(
				(x) => x.editable == true
			);
			if (editColsEle && editColsEle.length > 0) {
				editColsEle.forEach((item) => {
					let controlname = '';
					if(item.editConfig && item.editConfig.inputtype	&& item.editConfig.inputtype === 'select'){
						controlname = item.editConfig && item.editConfig.valueField	? item.editConfig.valueField : 'Id';
					}
					else{
						controlname=item.key;
					}
					addCols.push({
						inputtype: item.editConfig && item.editConfig.inputtype	? item.editConfig.inputtype : 'text',
						url: item.editConfig && item.editConfig.url	? item.editConfig.url : '',
						textField: item.editConfig && item.editConfig.textField	? item.editConfig.textField : 'Name',
						valueField: item.editConfig && item.editConfig.valueField	? item.editConfig.valueField : 'Id',
						key: `add-${item.key}`,
						editable: item.editable && item.editable === true ? true : false,
						display: item.key,
						validations:
							item.editConfig && item.editConfig.validations
								? item.editConfig.validations
								: [],
						addcontrolname: `${controlname}`,
						editcontrolname: `${controlname}`,
					});
				});

				addCols.push({
					inputtype: "",
					key: "add-actions",
					display: "actions",
					validations: [],
					addcontrolname: "add-actions",
					editcontrolname: "add-actions",
				});
			}
		}

		return addCols;
	}

	get AddDisplayColumnName() {
		if (!this.editableGrid) return [];

		let addCols = [];
		if (
			this.config &&
			this.config.list.displayColumns &&
			this.config.list.displayColumns.length > 0
		) {
			const editColsEle = this.config.list.displayColumns.filter(
				(x) => x.editable == true
			);
			if (editColsEle && editColsEle.length > 0) {
				editColsEle.forEach((item) => {
					addCols.push(`add-${item.key}`);
				});

				addCols.push("add-actions");
			}
		}

		return addCols;
	}

	// ADD Item FUNCTIONS: clearAddForm | checkAddForm | addItemButtonOnClick | cancelAddButtonOnClick | saveNewItem
	clearAddForm() {
		Object.keys(this.formGroup.controls).forEach((key) => {
			this.formGroup.controls[key].setValue("");
			this.formGroup.controls[key].markAsPristine();
			this.formGroup.controls[key].markAsUntouched();
		});

		this.itemForAdd = new AddGridItemModel();
		this.itemForAdd.clear();
		this.itemForAdd._isEditMode = false;
	}

	checkAddForm() {
		Object.keys(this.formGroup.controls).forEach((key) => {
			if (this.formGroup.controls[key].invalid) {
				this.formGroup.controls[key].markAsTouched();
				return false;
			}
		});

		return true;
	}

	addItemButtonOnClick() {
		if(this.editableGrid){
			this.clearAddForm();
			this.itemForAdd._isEditMode = true;
			this.isSwitchedToEditMode = true;
		}

		this.actionClicked('new', null);
	}

	cancelAddButtonOnClick() {
		this.itemForAdd._isEditMode = false;
		this.isSwitchedToEditMode = false;
	}

	saveNewItem() {
		if (!this.checkAddForm()) {
			return;
		}

		this.loadingAfterSubmit = true;
		//console.log(this.formGroup.value);
		this.layoutUtilsService.startLoadingMessage("Saving...");
		const createSub = this.httpService
			.post(this.config.create.url, this.formGroup.value)
			.pipe(
				finalize(() => {
					this.dataSource.loading$ = of(false);
					this.loadingAfterSubmit = false;
					this.layoutUtilsService.stopLoadingMessage();
				})
			)
			.subscribe(
				(response: ResponseModel) => {
					if (response.status === ResponseStatus.Success) {
						this.itemForAdd._isEditMode = false;
						this.isSwitchedToEditMode = false;
						this.clearAddForm();
						this.selection.clear();
						this.loadData();
					}
				},
				(error) => {
					this.layoutUtilsService.showActionNotification(
						Utilities.getHttpErrorMessage(error),
						MessageType.Delete
					);
				}
			);
		this.subscriptions.push(createSub);

		this.isSwitchedToEditMode = false;
		this.clearAddForm();
	}

	// EDIT Item FUNCTIONS: clearEditForm | checkEditForm | editItemButtonOnClick | cancelEditButtonOnClick |
	clearEditForm() {
		Object.keys(this.formGroup.controls).forEach((key) => {
			this.formGroup.controls[key].setValue("");
			this.formGroup.controls[key].markAsPristine();
			this.formGroup.controls[key].markAsUntouched();
		});

		this.itemForAdd = new AddGridItemModel();
		this.itemForAdd.clear();
		this.itemForAdd._isEditMode = false;
		//this.cdr.detectChanges();
	}

	checkEditForm() {
		Object.keys(this.formGroup.controls).forEach((key) => {
			if (this.formGroup.controls[key].invalid) {
				this.formGroup.controls[key].markAsTouched();
				return false;
			}
		});

		return true;
	}

	/**
	 * Update Item
	 *
	 * @param _item: AddGridItemModel
	 */
	editItemButtonOnClick(_item: any, rowIndex) {
		console.log(`row index:${rowIndex}-Item:${_item}`);
		this.formGroup.patchValue(_item);
		this.tempTableResult[rowIndex]._isEditMode = true;
		this.dataSource.entitySubject.next(this.tempTableResult);
		this.isSwitchedToEditMode = true;
	}

	/**
	 * Cancel Item
	 *
	 * @param _item: AddGridItemModel
	 */
	cancelEditButtonOnClick(_item: AddGridItemModel, rowIndex) {
		console.log(`row index:${rowIndex}-Item:${_item}`);
		this.tempTableResult[rowIndex]._isEditMode = false;
		this.dataSource.entitySubject.next(this.tempTableResult);
		this.isSwitchedToEditMode = false;
	}

	/**
	 * Save Item
	 *
	 * @param _item: AddGridItemModel
	 */
	saveUpdatedItem(_item: AddGridItemModel, _rowIndex) {
		if (!this.checkEditForm()) {
			return;
		}

		const objectForUpdate = Object.assign(_item, this.formGroup.value);
		console.log(objectForUpdate);

		this.loadingAfterSubmit = true;
		this.layoutUtilsService.startLoadingMessage("Updating...");

		const createSub = this.httpService
			.post(this.config.update.url, objectForUpdate, true)
			.pipe(
				finalize(() => {
					this.dataSource.loading$ = of(false);
					this.loadingAfterSubmit = false;
					this.layoutUtilsService.stopLoadingMessage();
				})
			)
			.subscribe(
				(response: ResponseModel) => {
					if (response.status === ResponseStatus.Success) {
						this.itemForAdd._isEditMode = false;
						this.isSwitchedToEditMode = false;
						this.clearAddForm();
						this.selection.clear();
						this.loadData();
					}
				},
				(error) => {
					this.layoutUtilsService.showActionNotification(
						Utilities.getHttpErrorMessage(error),
						MessageType.Delete
					);
				}
			);
		this.subscriptions.push(createSub);
	}

	/** ACTIONS */
	/**
	 * Delete Item
	 *
	 * @param _item: AddGridItemModel
	 */
	deleteItem(_item: AddGridItemModel, _rowIndex) {
		const idsForDeletion: number[] = [];
		idsForDeletion.push(_item[this.config.primaryColumn]);
		this.deleteRecords(idsForDeletion);
	}

	private touchAllFormFields(formGroup: FormGroup): void {
		Object.keys(formGroup.controls).forEach((key) => {
			formGroup.get(key).markAsDirty();
		});
	}

	private markFormGroupTouched(formGroup: FormGroup) {
		Object.keys(formGroup.controls).forEach((key) => {
			const control = formGroup.controls[key];
			control.markAsDirty();
			if (control instanceof FormGroup) {
				this.markFormGroupTouched(control);
			}
		});
	}
}
