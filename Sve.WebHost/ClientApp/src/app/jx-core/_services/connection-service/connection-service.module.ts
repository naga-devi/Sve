import { NgModule } from '@angular/core';
import { ConnectionService } from './lib/connection-service.service';
import { HttpClientModule } from '@angular/common/http';
import { MatSnackBarModule } from '@angular/material/snack-bar';

@NgModule({
    imports: [
        HttpClientModule,
        MatSnackBarModule
    ],
   declarations: [],
    exports: [
        //ConnectionService
    ],
    providers: [ConnectionService]
})
export class ConnectionServiceModule {
}
