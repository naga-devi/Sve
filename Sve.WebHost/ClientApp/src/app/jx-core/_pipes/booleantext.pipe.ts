import { Pipe, PipeTransform } from '@angular/core';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
// import { EntityStatus } from '@/_enums';
@Pipe({
  name: 'booleanText'
})

export class BooleanTextPipe implements PipeTransform {

  constructor(private sanitizer: DomSanitizer) { }

  transform(value: boolean, event?: number): SafeHtml {

	  let formattedHtml = '';
	  switch (value) {
		  case true:
			  formattedHtml = '<span class="uppercase btn btn-bold btn-sm btn-font-sm  btn-label-success">YES</span>'; break;
		  case false:
			  formattedHtml = '<span class="uppercase btn btn-bold btn-sm btn-font-sm  btn-label-danger">NO</span>'; break;

	  }

	  return this.sanitizer.bypassSecurityTrustHtml(formattedHtml);
  }
}
