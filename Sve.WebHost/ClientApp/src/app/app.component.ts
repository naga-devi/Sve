import { PlatformLocation } from '@angular/common';
import { Component } from '@angular/core';
import { Router, NavigationEnd, NavigationStart } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { Settings, AppSettings } from './app.settings';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  loading: boolean = false;
  currentUrl: string;
  public settings: Settings;
    constructor(public appSettings: AppSettings, public router: Router, public _router: Router, location: PlatformLocation, private spinner: NgxSpinnerService){
    this.settings = this.appSettings.settings;
    this._router.events.subscribe((routerEvent) => {
      if (routerEvent instanceof NavigationStart) {
        this.spinner.show();
        location.onPopState(() => {
          window.location.reload();
        });
        this.currentUrl = routerEvent.url.substring(
          routerEvent.url.lastIndexOf('/') + 1
        );
      }
      if (routerEvent instanceof NavigationEnd) {
        this.spinner.hide();
      }
      window.scrollTo(0, 0);
    });
  }

  ngOnInit() {
   // this.router.navigate(['']);  //redirect other pages to homepage on browser refresh    
  }

  ngAfterViewInit(){
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
          window.scrollTo(0,0);
      }
    })  
  }
}
