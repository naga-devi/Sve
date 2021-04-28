import { Component, Input, EventEmitter, Output, ChangeDetectionStrategy, ViewEncapsulation, ElementRef, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { JxBaseComponent } from '../../../_base/base.component';

@Component({
  selector: 'jx-mat-form',
  templateUrl: './form.component.html',
  styleUrls: ['./form.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  encapsulation: ViewEncapsulation.None
})
export class JxMatFormComponent extends JxBaseComponent{
  formErrors: any;
  validationMessage: any;
	@Input() public parentGroup: FormGroup;
  @Input() public title: string = null;
  @Input() public saveText = 'SAVE';
  @Input() public saveDisabled = false;
  @Input() public saving = false;
  @Input() public cancelText = 'CANCEL';
  @Input() public cancelDisabled = false;
  @Input() public formLoading = false;
  @Input() public isMatDialog = false;
  @Output() public onCancel = new EventEmitter<void>();
  @Output() public onSave = new EventEmitter<void>();
  @Input() public showToolBar = false;
  @Input() public showToolBarAddBtn = true;
  @Input() public showToolBarBackBtn = true;
  @Input() public toolBarAddBtnText = "Add";
  @Output() public onToolBarAdd = new EventEmitter<void>();
  @ViewChild('jxform', {static: true}) portlet: ElementRef;
  
  //saveDisabled: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(true);
  
  // buildForms() {
  //   this.parentGroup.valueChanges.subscribe(data => {
  //       this.onValueChange();
  //   });
  // }

  // onValueChange() {
  //   this.onChange.emit();
  //     const form = this.parentGroup;
  //     //console.log(this.parentGroup.invalid);
  //     //this.saveDisabled.next(this.parentGroup.invalid);
  //     for(const field in this.formErrors){
  //       /* Initialize form's Errors */
  //       this.formErrors[field] = '';
  //       const control = form.get(field);
  //       if (control && control.dirty && !control.valid) {
  //           const messages = this.validationMessage[field];
  //           for (const key in control.errors){
  //               this.formErrors[field] += messages[key];
  //           }
  //           this.emitErrorAction();
  //       }
  //       if (control && control.dirty && control.valid) {
  //           this.emitNotErrorAction();
  //       }
  //   }
    
  // }
  // emitErrorAction() {
  //       console.log('error-occured')
  // }

  // emitNotErrorAction(){
  //   console.log('no-error-occured')
  // }
  // canDeActivateInputPage(): boolean {
  //     return confirm("canDeActivateInputPage");
  // }
  hasFormErrors = false
  isFormValid() {
      // for (const field in this.formErrors) {
      //     const control = this.parentGroup.controls[field];
      //     control.markAsDirty();
      //     this.onValueChange();
      // }

      /** check form */
      const controls = this.parentGroup.controls;
		if (this.parentGroup.invalid) {
			Object.keys(controls).forEach(controlName =>
				controls[controlName].markAsTouched()
			);

			this.hasFormErrors = true;
			return;
		}
  }

  emitSaveAction(){
    this.isFormValid();
    this.onSave.emit()
  }
}