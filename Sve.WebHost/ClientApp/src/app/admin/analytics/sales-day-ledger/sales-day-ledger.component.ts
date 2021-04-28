import { Component, OnInit, ElementRef, ViewChild, Input } from "@angular/core";

@Component({
  selector: "app-sales-day-ledger",
  templateUrl: "./sales-day-ledger.component.html",
  styleUrls: ["./sales-day-ledger.component.scss"],
})
export class SalesDayLedgerComponent implements OnInit {
  @Input() title = "";
  @Input() data: any[];
  view: any[] = [700, 400];

  // options
  showXAxis: boolean = true;
  showYAxis: boolean = true;
  gradient: boolean = true;
  showLegend: boolean = true;
  showXAxisLabel: boolean = true;
  @Input()  xAxisLabel: string = "";
  showYAxisLabel: boolean = true;
  @Input() yAxisLabel: string = "";
  @Input() legendTitle: string = "";

  colorScheme = {
    domain: ["#5AA454", "#C7B42C", "#AAAAAA"],
  };
  @ViewChild("resizedDiv") resizedDiv: ElementRef;
  public previousWidthOfResizedDiv: number = 0;
  constructor() {}

  ngOnInit() {}

  onSelect(data): void {
    //console.log('Item clicked', JSON.parse(JSON.stringify(data)));
  }

  onActivate(data): void {
    //console.log('Activate', JSON.parse(JSON.stringify(data)));
  }

  onDeactivate(data): void {
    //console.log('Deactivate', JSON.parse(JSON.stringify(data)));
  }

  ngAfterViewChecked() {
    if (
      this.previousWidthOfResizedDiv !=
      this.resizedDiv.nativeElement.clientWidth
    ) {
      //setTimeout(() => this.data = [...multi] );
    }
    this.previousWidthOfResizedDiv = this.resizedDiv.nativeElement.clientWidth;
  }
}
