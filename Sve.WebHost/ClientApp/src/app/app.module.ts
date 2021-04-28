import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { NgxSpinnerModule } from 'ngx-spinner';
import { AgmCoreModule } from '@agm/core';

import { OverlayContainer, Overlay } from '@angular/cdk/overlay';
import { MAT_MENU_SCROLL_STRATEGY } from '@angular/material/menu';
import { CustomOverlayContainer } from './theme/utils/custom-overlay-container';
import { menuScrollStrategy } from './theme/utils/scroll-strategy';

import { SharedModule } from './shared/shared.module';
import { AppRoutingModule } from './app.routing';
import { AppComponent } from './app.component';
import { PagesComponent } from './pages/pages.component';
import { NotFoundComponent } from './pages/not-found/not-found.component';
import { TopMenuComponent } from './theme/components/top-menu/top-menu.component';
import { MenuComponent } from './theme/components/menu/menu.component';
import { SidenavMenuComponent } from './theme/components/sidenav-menu/sidenav-menu.component';
import { BreadcrumbComponent } from './theme/components/breadcrumb/breadcrumb.component';

import { AppSettings } from './app.settings';
import { AppService } from './app.service';
import { OptionsComponent } from './theme/components/options/options.component';
import { FooterComponent } from './theme/components/footer/footer.component';
import { ConnectionServiceModule, ConnectionServiceOptions, ConnectionServiceOptionsToken, JxNetCoreModule } from './jx-core';
import { environment } from 'src/environments/environment';

@NgModule({
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        HttpClientModule,
        NgxSpinnerModule,
        AgmCoreModule.forRoot({
            apiKey: 'AIzaSyA1rF9bttCxRmsNdZYjW7FzIoyrul5jb-s'
        }),
        SharedModule,
        AppRoutingModule,
        ConnectionServiceModule,
        // Specify your library as an import
        JxNetCoreModule.forRoot({
            baseApiUrl: environment.apiUrl,
            loginUrl: environment.loginUrl,
            appVersion:'v.3.0.1',
            authTokenKey:environment.authTokenKey
        }),
    ],
    declarations: [
        AppComponent,
        PagesComponent,
        NotFoundComponent,
        TopMenuComponent,
        MenuComponent,
        SidenavMenuComponent,
        BreadcrumbComponent,
        OptionsComponent,
        FooterComponent
    ],
    providers: [
        AppSettings,
        AppService,

        { provide: OverlayContainer, useClass: CustomOverlayContainer },
        { provide: MAT_MENU_SCROLL_STRATEGY, useFactory: menuScrollStrategy, deps: [Overlay] },
        {
            provide: ConnectionServiceOptionsToken,
            useValue: <ConnectionServiceOptions>{
                enableHeartbeat: false,
                // heartbeatUrl: '/assets/ping.json',
                // requestMethod: 'get',
                // heartbeatInterval: 3000
            }
        }        
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }