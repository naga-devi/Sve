// Angular
import { Injectable } from '@angular/core';
// RxJS
import { BehaviorSubject } from 'rxjs';

@Injectable()
export class KtDialogService {
	private ktDialog: any;
	private currentState: BehaviorSubject<boolean> = new BehaviorSubject(false);

	// Public properties
	constructor() {
		this.ktDialog = new KTDialog({'type': 'loader', 'placement': 'top center', 'message': 'Loading ...'});
	}

	show(message = "Loading...") {
		this.currentState.next(true);
		this.ktDialog.options.message = message;
		this.ktDialog.show();
	}

	hide() {
		this.currentState.next(false);
		this.ktDialog.hide();
		if (document.querySelectorAll('.kt-dialog.kt-dialog--shown').length > 0) {
			document.querySelectorAll('.kt-dialog.kt-dialog--shown').forEach(x=>x.remove());
		}
	}

	checkIsShown() {
		return this.currentState.value;
	}
}
