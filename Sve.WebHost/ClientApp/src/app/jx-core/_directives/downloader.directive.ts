import { Directive, ElementRef, Input, HostListener } from '@angular/core';
/**
 * Usage: <a jxDownloader fileName="sampath.png" filePath="~/1/1.png">download</a>
 */
@Directive({
	// tslint:disable-next-line:directive-selector
	selector: '[jxDownloader]'
})
export class JxDownloaderDirective {
	@Input() public fileName: string;
	@Input() public filePath: string;

	constructor(private elRef: ElementRef) {
	}

	@HostListener('click') onClick() {
		//console.log(this.fileName + '___' + this.filePath);
	}
}
