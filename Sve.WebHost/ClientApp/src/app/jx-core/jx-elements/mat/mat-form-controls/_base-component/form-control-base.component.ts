import { Directive, Input } from "@angular/core";
import { FormControl } from "@angular/forms";
import { JxBaseComponent } from "../../../_base/base.component";

@Directive() // Needed only for Angular v9+ strict mode that enforce decorator to enable Component inheritance
export abstract class JxFormControlBaseComponent extends JxBaseComponent {	
	@Input() public label: string;
	@Input() public placeholder: string;
	@Input() public readonly: boolean = false;
	@Input() public control: FormControl;
	@Input() public appearance: "legacy" | "fill" | "standard" | "outline" =
		"outline";

	constructor() {
		super();
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
