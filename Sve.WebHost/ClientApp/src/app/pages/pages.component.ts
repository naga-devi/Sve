import { Component, OnInit, HostListener, ViewChild } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { Settings, AppSettings } from '../app.settings';
import { AppService } from '../app.service';
import { Category, Product } from '../app.models';
import { SidenavMenuService } from '../theme/components/sidenav-menu/sidenav-menu.service';
import { MessageService } from '../jx-core';

@Component({
    selector: 'app-pages',
    templateUrl: './pages.component.html',
    styleUrls: ['./pages.component.scss'],
    providers: [SidenavMenuService]
})
export class PagesComponent implements OnInit {
    public showBackToTop: boolean = false;
    public categories: Category[];
    public category: Category;
    public sidenavMenuItems: Array<any>;
    @ViewChild('sidenav', { static: true }) sidenav: any;
    public searchValue: string = '';
    public settings: Settings;
    constructor(public appSettings: AppSettings,
        public appService: AppService,
        public sidenavMenuService: SidenavMenuService,
        public router: Router,
        public messageService: MessageService) {
        this.settings = this.appSettings.settings;
    }

    ngOnInit() {
        this.getCategories();
        this.sidenavMenuItems = this.sidenavMenuService.getSidenavMenuItems();
    }

    public getCategories() {
        this.appService.getCategories().subscribe(data => {
            this.categories = data;
            this.category = data[0];
            this.appService.Data.categories = data;
        })
    }

    public changeCategory(event: Category) {
        if (event) {
            this.category = event;//this.categories.filter(category => category.name == event.target.innerText)[0];
        }
        if (window.innerWidth < 960) {
            this.stopClickPropagate(event);
        }
    }

    public remove(product) {
        const index: number = this.appService.Data.cartList.indexOf(product);
        if (index !== -1) {
            this.appService.Data.cartList.splice(index, 1);
            localStorage.setItem('cart', JSON.stringify(this.appService.Data.cartList));
            this.appService.Data.totalPrice = this.appService.Data.totalPrice - product.newPrice * product.cartCount;
            this.appService.Data.totalCartCount = this.appService.Data.totalCartCount - product.cartCount;
            this.appService.resetProductCartCount(product);
        }
    }

    public clear() {
        this.appService.Data.cartList.forEach(product => {
            this.appService.resetProductCartCount(product);
        });
        this.appService.Data.cartList.length = 0;
        this.appService.Data.totalPrice = 0;
        this.appService.Data.totalCartCount = 0;
        localStorage.removeItem('cart');
    }


    public changeTheme(theme) {
        this.settings.theme = theme;
    }

    public stopClickPropagate(event: any) {
        event.stopPropagation();
        event.preventDefault();
    }

    public search(searchValue: string) {
        this.messageService.sendMessage({ cat: this.category.id || 0, q: searchValue });
        if (this.category.id > 0)
            this.router.navigate(['/products', 'search'], { queryParams: { cat: this.category.id, 'q': searchValue } });
        else
            this.router.navigate(['/products', 'search'], { queryParams: { 'q': searchValue } });
    }

    searchByKeyPress(event) {        
        //console.log("You entered: ", event.target.value);
        if (parseInt(event.target.value, 0) == 0)
            return;
        this.search(event.target.value);
    }


    public scrollToTop() {
        var scrollDuration = 200;
        var scrollStep = -window.pageYOffset / (scrollDuration / 20);
        var scrollInterval = setInterval(() => {
            if (window.pageYOffset != 0) {
                window.scrollBy(0, scrollStep);
            }
            else {
                clearInterval(scrollInterval);
            }
        }, 10);
        if (window.innerWidth <= 768) {
            setTimeout(() => { window.scrollTo(0, 0) });
        }
    }
    @HostListener('window:scroll', ['$event'])
    onWindowScroll($event) {
        ($event.target.documentElement.scrollTop > 300) ? this.showBackToTop = true : this.showBackToTop = false;
    }

    ngAfterViewInit() {
        this.router.events.subscribe(event => {
            if (event instanceof NavigationEnd) {
                this.sidenav.close();
            }
        });
        this.sidenavMenuService.expandActiveSubMenu(this.sidenavMenuService.getSidenavMenuItems());
    }

    public closeSubMenus() {
        if (window.innerWidth < 960) {
            this.sidenavMenuService.closeAllSubMenus();
        }
    }

}