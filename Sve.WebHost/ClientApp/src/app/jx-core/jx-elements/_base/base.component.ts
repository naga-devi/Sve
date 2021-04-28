import { Directive, OnDestroy } from "@angular/core";
import { Subject } from "rxjs";
import { takeUntil } from "rxjs/operators";

@Directive() // Needed only for Angular v9+ strict mode that enforce decorator to enable Component inheritance
export abstract class JxBaseComponent implements OnDestroy {
    
	// _destroy$ is LAZY: it'll be created (and allocate memory) only if you use takeUntilDestroy
	private _destroy$?: Subject<void>;
	protected takeUntilDestroy = <T>() => {
		if (!this._destroy$) this._destroy$ = new Subject<void>(); // LAZY Subject
		return takeUntil<T>(this._destroy$);
	};

	// messageValue : string;
	// @Output()
	// messageChange = new EventEmitter<string>();
	// @Input()
	// get message(){
	// 	return this.messageValue;
	// }
	// set message(val) {
	// 	this.messageValue = val;
	// 	this.messageChange.emit(this.messageValue);
	// }
	// <example-component [(message)]="title"></example-component>

	ngOnDestroy() {
		if (this._destroy$) {
			this._destroy$.next();
			this._destroy$.complete();
		}
	}
}

// sampleSubcribe(log:boolean = false){
//     this.route.paramMap
//       .pipe(this.takeUntilDestroy()) //NOTICE takeUntilDestroy PATTERN TO AUTOMATIC unsubscribe!
//       .subscribe(pm => {
//         const obj = {};
//         pm.keys.forEach(k => {
//           obj[k] = pm.get(k);
//         });
//         if (log) console.log({ pm, obj })
//         this.pars = obj;
//       });
//   }