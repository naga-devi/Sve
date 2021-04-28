import { Component, OnInit, ChangeDetectionStrategy, EventEmitter, Output } from '@angular/core';
import { MatDatepickerInputEvent } from '@angular/material/datepicker';
import { JxFormControlBaseComponent } from '../_base-component/form-control-base.component';

/**
 * Usage: <jx-mat-date-picker [label]="'label'" [placeholder]="'DD-MM-YYYY'" [readonly]="true" [control]="formcontrol" [appearance]="'outline'"></jx-mat-date-picker>
 */
@Component({
  selector: 'jx-mat-date-picker',
  templateUrl: './date-picker.component.html',
  //styleUrls: ['./form-field.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class JxMatDatePickerComponent extends JxFormControlBaseComponent implements OnInit {
  @Output() onDateChange: EventEmitter<MatDatepickerInputEvent<any>> = new EventEmitter();

  // constructor(private dateAdapter: DateAdapter<Date>) {
  //   super();
  //   this.dateAdapter.setLocale('en-GB'); //dd/MM/yyyy
  // }

  public ngOnInit() {
    if(this.placeholder && this.placeholder.length === 0){
      this.placeholder= this.label;
    }
  }

  dateChange(date: any): void {
    this.onDateChange.emit(date);
  }
}