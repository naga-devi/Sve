import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { AppService } from "src/app/app.service";
import { getDate, toServerDate, Utilities } from "../../../jx-core";

@Component({
  selector: "app-sales-reports-dayledger",
  templateUrl: "./day-ledger.component.html",
  styleUrls: ["./day-ledger.component.scss"],
})
export class SalesReportsDayLedgerComponent implements OnInit {
  data = [];
  chartData = [];
  public form: FormGroup;
  today = getDate();
  constructor(public appService: AppService,
    public formBuilder: FormBuilder) {}

  ngOnInit(): void {
    this.initForm();
    this.getDayLedger(this.today);
  }

  initForm() {
    this.form = this.formBuilder.group({
      reportDate: [getDate(), Validators.required],
    });
  }

  public getDayLedger(date: any) {
    this.appService
      .getBy(`sales/reports/day-ledger/${date}`)
      .subscribe((data) => {
        this.data = data ? data : [];
        if (this.data.length > 0) {
          this.chartData = this.prepareChartData(this.data);
        }
      });
  }

  onDateChange(){
    let selectedDate = this.form.controls['reportDate'].value;
    if(selectedDate)
      this.getDayLedger(toServerDate(selectedDate));
  }

  prepareChartData(data: any[]) {
    const filterData = data.filter((x) => x.paymodeText !== "Total");
    let cData = [];
    cData.push(this.createChartItem(filterData, "Quantity", "totalQuantity"));
    cData.push(this.createChartItem(filterData, "Total Amount", "totalAmount"));
    cData.push(this.createChartItem(filterData, "Discountt", "discountAmount"));
    cData.push(this.createChartItem(filterData, "Net Amount", "netAmount"));
    cData.push(this.createChartItem(filterData, "RoundOff", "roundOffAmount"));
    cData.push(this.createChartItem(filterData, "Grand Total", "grandTotal"));
    cData.push(this.createChartItem(filterData, "Paid Amount", "paidAmount"));
    cData.push(
      this.createChartItem(filterData, "Balance Total", "balanceAmount")
    );

    return cData;
  }

  createChartItem(filterData, title, property) {
    let item = {
      name: title,
      series: [
        {
          name: "Cash",
          value: filterData.filter((x) => x.paymodeText === "Cash")[0][
            property
          ],
        },
        {
          name: "Online",
          value: filterData.filter((x) => x.paymodeText === "Online")[0][
            property
          ],
        },
      ],
    };

    return item;
  }
}
