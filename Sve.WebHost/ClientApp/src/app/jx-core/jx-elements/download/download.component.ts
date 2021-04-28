import { Component, Input, Output, EventEmitter } from '@angular/core';
import { HttpEventType } from '@angular/common/http';
import { ProgressStatus, ProgressStatusEnum } from '../../_models/progress-status.model';
import { KtDialogService } from '../../_services/kt-dialog/kt-dialog.service'
import { finalize } from 'rxjs/operators';
import { UploadDownloadService } from '../../_services/download.service/upload-download.service';
import { Utilities } from '../../utilities/utilities';

/**
 * Usage: <kt-jx-download [disabled]="showProgress"
 * 	[filePath]="~/uploads/file.png" [fileName]="file.png" [zip]="true" [files]="[new Array<any>()]" (downloadStatus)="downloadStatus($event)"></kt-jx-download>
 */
@Component({
	selector: 'kt-jx-download',
	templateUrl: 'download.component.html'
})

export class JxDownloadComponent {
	@Input() public disabled: boolean;
	@Input() public fileName: string;
	@Input() public filePath: string;
	@Output() public downloadStatus: EventEmitter<ProgressStatus>;

	@Input() public zip = false;
	@Input() public files = new Array<any>();


	constructor(private service: UploadDownloadService, private ktDialogService: KtDialogService) {
		this.downloadStatus = new EventEmitter<ProgressStatus>();
	}

	public download() {
		this.downloadStatus.emit({ status: ProgressStatusEnum.START });
		this.ktDialogService.show(`downloading...`);
		let downloadFiles = new Array<any>();
		if (this.zip) {
			downloadFiles = this.files;
		}
		else {
			downloadFiles.push({ DisplayName: this.fileName, SavedPath: this.filePath });
		}

		const downloadFileName = this.zip && downloadFiles.length > 1 ? 'esh-bundle.zip' : this.zip && downloadFiles.length == 1 ? downloadFiles[0].DisplayName : this.fileName;

		this.service.downloadFileFromPath(downloadFiles)
			.pipe(finalize(() => {
				this.ktDialogService.hide();
			})).subscribe(
				data => {
					switch (data.type) {
						case HttpEventType.DownloadProgress:
							const percentage = Math.round((data.loaded / data.total) * 100);
							this.downloadStatus.emit({ status: ProgressStatusEnum.IN_PROGRESS, percentage });
							break;
						case HttpEventType.Response:
							this.downloadStatus.emit({ status: ProgressStatusEnum.COMPLETE });
							//const downloadedFile = new Blob([data.body], { type: data.body.type });
							//const a = document.createElement('a');
							//a.setAttribute('style', 'display:none;');
							//document.body.appendChild(a);
							//a.download = downloadFileName;
							//a.href = URL.createObjectURL(downloadedFile);
							//a.target = '_blank';
							//a.click();
							//document.body.removeChild(a);

							Utilities.downloadFile(data, downloadFileName);

							this.ktDialogService.hide();
							break;
					}
				},
				(error) => {
					this.ktDialogService.show('server error');
					this.ktDialogService.hide();
					this.downloadStatus.emit({ status: ProgressStatusEnum.ERROR });
				}
			);
	}
}
