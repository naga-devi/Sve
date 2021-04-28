import { Pipe, PipeTransform } from '@angular/core';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
// import { EntityStatus } from '@/_enums';
@Pipe({
  name: 'entityApprove'
})

export class EntityApprovePipe implements PipeTransform {

  constructor(private sanitizer: DomSanitizer) { }

  transform(value: boolean, event?: number): SafeHtml {

	  let formattedHtml = '';
	  switch (value) {
		  case true:
			  formattedHtml = '<span class="uppercase btn btn-bold btn-sm btn-font-sm  btn-label-success">APPROVED</span>'; break;
		  case false:
			  formattedHtml = '<span class="uppercase btn btn-bold btn-sm btn-font-sm  btn-label-danger">PENDING</span>'; break;

	  }

	  return this.sanitizer.bypassSecurityTrustHtml(formattedHtml);
  }
}
