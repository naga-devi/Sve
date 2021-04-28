import { Component, OnInit } from "@angular/core";
import { FormBuilder, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { BehaviorSubject } from "rxjs";
import { finalize } from "rxjs/operators";
import {
  LayoutUtilsService,
  HttpService,
  JxBaseComponent,
  MessageType,
  ResponseModel,
  ResponseStatus,
  Utilities,
} from "src/app/jx-core";
import { AppService } from "../../../../app.service";
import { TransactionsModel } from "../../_models";

@Component({
  selector: "app-accounts-transaction-add",
  templateUrl: "./transaction.component.html",
  styleUrls: ["./transaction.component.scss"],
})
export class NewTransactionComponent extends JxBaseComponent implements OnInit {
  fConfig = {
    fGroup: null,
    formLoading: false,
    saving: false,
  };
  customerApiUrl = `accounts/customers/all/1?name=`;

  transactions: TransactionsModel;
  showBankAccountSection = false;
  showChequeSection = false;
  fromAccounts: BehaviorSubject<any[]>;
  toAccounts: BehaviorSubject<any[]>;

  constructor(
    public appService: AppService,
    private router: Router,
    private fb: FormBuilder,
    private layoutUtilsService: LayoutUtilsService,
    private _httpService: HttpService
  ) {
    super();
    this.fromAccounts = new BehaviorSubject<any[]>(null);
    this.toAccounts = new BehaviorSubject<any[]>(null);
  }

  ngOnInit() {
    this.createForm();
  }

  voucherTypeChanged(e: any) {
    this.customerApiUrl = `accounts/customers/all/${e.voucherTypeId}?name=`;
  }
  accountTypeChanged(e: any) {
    this.showBankAccountSection = Number(e.accountTypeId) === 2;
  }
  paymodeChanged(e: any) {
    this.showChequeSection = Number(e.payModeId) === 2;

    if (this.showBankAccountSection) {
      this.getCustomerAccountDetails();
    }
  }

  createForm() {
    this.transactions = new TransactionsModel();
    this.transactions.clear();
    this.fConfig.fGroup = this.fb.group({
      VoucherTypeId: [this.transactions.VoucherTypeId, Validators.required],
      AccountTypeId: [this.transactions.AccountTypeId, Validators.required],
      CustomerId: [this.transactions.CustomerId, Validators.required],
      PayModeId: [this.transactions.PayModeId],
      PaidAmount: [this.transactions.PaidAmount, Validators.required],
      PaidDate: [this.transactions.PaidDate, Validators.required],
      Remarks: [this.transactions.Remarks],
      FromAccountId: [null],
      ToAccountId: [null],
      ChequeNo: [null],
      ChequeDate: [null],
      UTRNo: [null],
    });
  }

  goBackToList() {
    this.router.navigateByUrl(`/Accounts/transactions`);
  }

  getCustomerAccountDetails() {
    const voucherTypeId = Number(
      this.fConfig.fGroup.controls["VoucherTypeId"].value
    );
    const customerId = Number(this.fConfig.fGroup.controls["CustomerId"].value);
    if (voucherTypeId > 0 && customerId > 0) {
      this.layoutUtilsService.startLoadingMessage();
      this._httpService
        .get(`accounts/accountdetails/all/${voucherTypeId}/${customerId}`)
        .pipe(
          finalize(() => {
            this.layoutUtilsService.stopLoadingMessage();
            this.takeUntilDestroy();
          })
        )
        .subscribe(
          (response:any) => {
            this.fromAccounts.next(response.fromAccounts);
            this.toAccounts.next(response.toAccounts);
          },
          (error) => {
            this.layoutUtilsService.showActionNotification(
              Utilities.getHttpErrorMessage(error)
            );
          }
        );
    } else {
      this.layoutUtilsService.showActionNotification(
        "Vourcher type and customer fields must be selected."
      );
    }
  }

  onSubmit(withBack: boolean = false) {
    this.fConfig.saving = false;
    const dataToBeSaved = this.fConfig.fGroup.value;
    dataToBeSaved.TransactionId = this.transactions.TransactionId;
    this.save(dataToBeSaved, withBack, dataToBeSaved.TransactionId > 0);
  }

  save(
    transactions: TransactionsModel,
    _withBack: boolean = false,
    isUpdate: boolean = false
  ) {
    this.fConfig.saving = true;
    this.layoutUtilsService.startLoadingMessage();
    const actionName = isUpdate ? "update" : "create";
    this._httpService
      .post(`accounts/transactions/${actionName}`, transactions, isUpdate)
      .pipe(
        finalize(() => {
          this.layoutUtilsService.stopLoadingMessage();
          this.fConfig.saving = false;
          this.takeUntilDestroy();
        })
      )
      .subscribe(
        (response: ResponseModel) => {
          this.layoutUtilsService.showActionNotification(
            response.message,
            isUpdate ? MessageType.Update : MessageType.Create
          );
          if (response.status === ResponseStatus.Success) {
            console.log('transaction completed');
            this.fConfig.fGroup.patchValue(null);
          }
        },
        (error) => {
          this.layoutUtilsService.showActionNotification(
            Utilities.getHttpErrorMessage(error)
          );
        }
      );
  }
}
