import {getDate} from '../../../util';
export interface JxMatTableActionItem {
	action: string;
	selectedItem: any;
}

export interface JxSortModel {
	sortOrder: string;
	sortField: string;
}

export interface JxPagerModel {
	pageSize: number;
	pageIndex: number;
}

export interface IJxMatTableConfig {
	entityName:string;
	primaryColumn: string;
	list: string;
	create:any;
	update: any;
	delete:any;
}

export class JxMatTableConfig {
	entityName:string='';
	primaryColumn: '';

	//displayColumns: any[];
	//showPager: boolean=true;
	//sortActive: string;
	//sortDirection: string;
	//listUrl: string;
	//listHttpType: string ='POST';
	//filter: any;

	//enableSearch: false;
	//searchColumn: '';

	//title: '';
	//enableActionMatIcons: true;

	//postData: any;
	//export
	exportFileName:string;

	list:any;
	create:any;
	update: any;
	delete:any;

	//constructor();
	//constructor(obj: IJxMatTableConfig);
	constructor(obj?: any) {
			this.entityName = obj && obj.entityName || '',
			//this.listUrl = obj && obj.listUrl || '',
			//this.listHttpType = obj && obj.listHttpType || 'POST',
			//this.showPager = obj && obj.showPager || true,
			//this.sortActive = obj && obj.sortActive || 'ModifiedOn',
			//this.sortDirection = obj && obj.sortDirection || 'asc',
			//this.enableSearch = obj && obj.enableSearch || false,
			//this.searchColumn = obj && obj.searchColumn || 'Name',
			//this.title = obj && obj.title || '',
			this.primaryColumn = obj && obj.primaryColumn || 'Id',
			//this.enableActionMatIcons = obj && obj.enableActionMatIcons || true,
			//this.filter = obj && obj.filter || {},
			//this.postData = obj && obj.postData || {},

			this.list = obj && obj.list || {},
			this.create = obj && obj.create || {},
			this.update = obj && obj.update || {},
			this.delete = obj && obj.delete || {},

			this.exportFileName = obj && obj.exportFileName || `list-${getDate()}`;
	}

	clear(): void {
		//this.displayColumns = [];
		// this.sortActive = '';
		// this.sortDirection = '';
		// this.filter = {};
		// this.enableSearch = false;
		// this.searchColumn = '';
		// this.title = '';
		// this.enableActionMatIcons = true;
	}
}
