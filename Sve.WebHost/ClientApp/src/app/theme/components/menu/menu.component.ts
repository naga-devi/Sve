import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AppService } from '../../../app.service';
import { AuthenticationService, MessageService } from '../../../jx-core';

@Component({
    selector: 'app-menu',
    templateUrl: './menu.component.html',
    styleUrls: ['./menu.component.scss']
})
export class MenuComponent implements OnInit {
    categories = [];
    currentUser: any;
    constructor(public appService: AppService,
        public authService: AuthenticationService,
        public messageService: MessageService,
        public router: Router
    ) { 
    }

    ngOnInit() {
        this.getCategories();
    }

    openMegaMenu() {
        let pane = document.getElementsByClassName('cdk-overlay-pane');
        [].forEach.call(pane, function (el) {
            if (el.children.length > 0) {
                if (el.children[0].classList.contains('mega-menu')) {
                    el.classList.add('mega-menu-pane');
                }
            }
        });
    }

    public getCategories() {
        this.appService.getCategories().subscribe(data => {
            this.categories = data;
            this.categories = this.categories.filter(x => x.id != 0);
        })
    }

    navigateToLink(categoryId: number) {
        this.messageService.sendMessage({ cat: categoryId || 0, q: '' });
        this.router.navigate(['/products', 'search'], { queryParams: { cat: categoryId, 'q': '' } });
    }
}
