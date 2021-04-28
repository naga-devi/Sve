import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges, ViewEncapsulation, ViewChild } from '@angular/core';

@Component({
	selector: 'kt-jx-file',
	templateUrl: './file.component.html',
	styleUrls: ['./file.component.css'],
	encapsulation: ViewEncapsulation.Emulated
})
export class JxFileComponent implements OnChanges {

	@Input() label = 'Attachment';
	@Input() multiple = false;
	@Input() size = 2; // default 2MB
	@Input() allowFileTypes = '';
	@Output() fileChange = new EventEmitter<FileList>();
	@ViewChild('jxuploader', { static: true }) jxuploader: any;

	public imagePath;
	imgURL: any;
	public errorMessage: string;
	public hasError: boolean = false;
	public attachedFiles;
	constructor() { }

	ngOnChanges(changes: SimpleChanges) {
		this.jxuploader.nativeElement.value = '';
	}

	fileChanged(files: FileList) {
		// this.preview(files);
		this.hasError = false;
		this.errorMessage = '';
		this.attachedFiles = '';
		for (let i = 0; files.length > i; i++) {
			this.checkFile(files[0], this.size, 50, this.allowFileTypes);
		}

		if (!this.hasError) {
			this.fileChange.emit(files);
			this.attachedFiles = `Attached ${files.length} file(s)`;
		}
	}

	preview(files) {
		if (files.length === 0) {
			return;
		}

		const mimeType = files[0].type;
		if (mimeType.match(/image\/*/) == null) {
			this.errorMessage = 'Only images are supported.';
			return;
		}

		const reader = new FileReader();
		this.imagePath = files;
		reader.readAsDataURL(files[0]);
		reader.onload = (event) => {
			this.imgURL = reader.result;
		};
	}

	/**
	 * validate file
	 * usage : onchange="checkFile(this,4,50,'doc,docx,pdf')"
	 * @param what : always =this in calling function
	 * @param mb : max file size allowed in MB
	 * @param iLen : max length of filename allowed (excluding the extension); 0 means don't test for length
	 * @param types : comma-delimited string of allowed file extensions
	 */
	checkFile(what: File, mb: number, iLen: number, types: string) {
		let msg = '';
		const fName = what.name;
		const ext = fName.substring(fName.lastIndexOf('.') + 1, fName.length) || fName;
		const exts = types.split(',');
		let fileTypeAllowed = false;

		// validate file name length

		//const regex = /^[A-Za-z0-9_ -]{1,1024}$/;
		//if (!regex.test(fName)) {
		//	msg = `The file name contains illegal characters\n
		//		Please re-name the file using only alphanumeric characters, hyphens, spaces and underscores\n`;
		//}

		if (types && types.length > 0 && exts.length > 0) {
			for (let k = 0; k < exts.length; k++) {
				if (ext === exts[k]) {
					fileTypeAllowed = true;
					break;
				}
			}

			if (!fileTypeAllowed) {
				msg = 'Please upload files of the following types only:\n  ' + types + '\n';
			}
		}

		if ((iLen > 0) && (fName.length > iLen)) {
			msg += 'The file name is too long\n  Please restrict it to ' + iLen.toString() + ' characters.\n';
		}

		const fileSize = what.size;
		const iMax = mb * 1024 * 1024;

		if (!((fileSize > 0) && (fileSize <= iMax))) {
			msg += 'The file size should be greater than 0 and less than ' + mb.toString() + 'MB\n';
		}

		if (!(msg === '')) {
			what = null;
			// alert(msg + '\n' + fName + '.' + ext);
			this.errorMessage = msg;
			this.hasError = true;
		}
	}

}
