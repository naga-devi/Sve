import { Pipe, PipeTransform } from '@angular/core';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
// import { EntityStatus } from '@/_enums';
@Pipe({
  name: 'entityStatus'
})

export class EntityStatusPipe implements PipeTransform {

  constructor(private _sanitizer: DomSanitizer) { }

  transform(value: string, event?: number): SafeHtml {

	  let formattedHtml = '';
	  const entityStatus = parseInt(value, 0);
	  switch (entityStatus) {
		  case 0:
			  formattedHtml = '<span class="uppercase btn btn-bold btn-sm btn-font-sm  btn-label-warning">InActive</span>'; break;
		  case 1:
			  formattedHtml = '<span class="uppercase btn btn-bold btn-sm btn-font-sm  btn-label-success">Active</span>'; break;
		 case 2:
			  formattedHtml = '<span class="uppercase btn btn-bold btn-sm btn-font-sm  btn-label-success">Approved</span>'; break;
		  case 3:
			  formattedHtml = '<span class="uppercase btn btn-bold btn-sm btn-font-sm  btn-label-danger">Rejected</span>'; break;
		  case 4:
			  formattedHtml = '<span class="uppercase btn btn-bold btn-sm btn-font-sm  btn-label-danger">Deleted</span>'; break;
		  case 5:
			  formattedHtml = '<span class="uppercase btn btn-bold btn-sm btn-font-sm  btn-label-brand">Pending</span>'; break;
		  case 6:
			  formattedHtml = '<span class="uppercase btn btn-bold btn-sm btn-font-sm  btn-label-warning">Archived</span>'; break;
		  case 7:
			  formattedHtml = '<span class="uppercase btn btn-bold btn-sm btn-font-sm  btn-label-warning">Locked</span>'; break;
		  case 8:
			  formattedHtml = '<span class="uppercase btn btn-bold btn-sm btn-font-sm  btn-label-warning">InActive</span>'; break;
	  }

	  return this._sanitizer.bypassSecurityTrustHtml(formattedHtml);
  }
}
