import { Component, OnInit } from '@angular/core';
import { Data, AppService } from '../../../app.service';
import { Settings, AppSettings } from '../../../app.settings';
import { AuthenticationService, MessageService } from '../../../jx-core';

@Component({
    selector: 'app-top-menu',
    templateUrl: './top-menu.component.html'
})
export class TopMenuComponent implements OnInit {
    public currency: any;    
    public flag: any;

    public settings: Settings;
    constructor(public appSettings: AppSettings,
        public messageService: MessageService,
        public appService: AppService,
        public authService: AuthenticationService) {
        this.settings = this.appSettings.settings;
    }

    ngOnInit() {
    }

    public changeCurrency(currency) {
        this.currency = currency;
    }

    public changeLang(flag) {
        this.flag = flag;
    }

    public signOut() {
        this.authService.logout();
    }
}
