// Angular
import { Component, Inject, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Utilities } from '../../utilities/utilities';
import { HttpService } from '../../_services/http/httpService';

@Component({
	selector: 'kt-delete-entity-dialog',
	templateUrl: './delete-entity-dialog.component.html'
})
export class DeleteEntityDialogComponent implements OnInit {
	// Public properties
	viewLoading: boolean = false;
	private subscriptions: Subscription[] = [];
	/**
	 * Component constructor
	 *
	 * @param dialogRef: MatDialogRef<DeleteEntityDialogComponent>
	 * @param data: any
	 */
	constructor(
		private snackBar: MatSnackBar,
		public dialogRef: MatDialogRef<DeleteEntityDialogComponent>,
		private httpService: HttpService,
		@Inject(MAT_DIALOG_DATA) public data: any
	) { }

	/**
	 * @ Lifecycle sequences => https://angular.io/guide/lifecycle-hooks
	 */

	/**
	 * On init
	 */
	ngOnInit() {
	}

	/**
	 * Close dialog with false result
	 */
	onNoClick(): void {
		this.dialogRef.close();
	}

	/**
	 * Close dialog with true result
	 */
	onYesClick(): void {

		if (this.data && this.data.url) {
			this.viewLoading = true;
			const deleteSubscription = this.httpService.delete(this.data.url)
				.pipe(finalize(() =>
					this.viewLoading = false
				))
				.subscribe(
					(response) => {
						const message = response.Status === 'Success' ? this.data.deleteMessage : response.Message;
						this.showActionNotification(message);
						this.dialogRef.close(response);
					},
					error => {
						this.showActionNotification(Utilities.getHttpErrorMessage(error));
					});
			this.subscriptions.push(deleteSubscription);
		}
		else {
			this.dialogRef.close(true); // Keep only this row
			///* Server loading imitation. Remove this */
			//this.viewLoading = true;
			//setTimeout(() => {
			//	this.dialogRef.close(true); // Keep only this row
			//}, 2500);
		}
	}

	showActionNotification(_message: string) {
		this.snackBar.open(_message, '', {
			duration: 2000,
		});

	}
}
