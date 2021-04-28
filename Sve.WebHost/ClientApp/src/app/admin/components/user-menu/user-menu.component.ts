import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../../../jx-core';

@Component({
    selector: 'app-user-menu',
    templateUrl: './user-menu.component.html',
    styleUrls: ['./user-menu.component.scss']
})
export class UserMenuComponent implements OnInit {
    public userImage = 'assets/images/others/admin.jpg';
    constructor(public authService: AuthenticationService) { }
    ngOnInit(): void {
    }

    logOut = () => this.authService.logout();
}
