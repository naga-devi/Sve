export class TransactionsModel {
	VoucherTypeId: number;
		AccountTypeId: number;
		CustomerId: number;
		PayModeId: number;
		TransactionId: number;
		PaidAmount: number;
		PaidDate: any;
		Remarks: string;
		CreatedOn: any;
		CreatedBy: string;
		ModifiedOn: any;
		ModifiedBy: string;
		Status: number;
	
	clear(): void {
			this.VoucherTypeId = null;
			this.AccountTypeId = null;
			this.CustomerId = null;
			this.PayModeId = null;
			this.TransactionId = null;
			this.PaidAmount = null;
			this.PaidDate = new Date();
			this.Remarks = '';
			this.CreatedOn = null;
			this.CreatedBy = '';
			this.ModifiedOn = null;
			this.ModifiedBy = '';
			this.Status = null;
		}
}

