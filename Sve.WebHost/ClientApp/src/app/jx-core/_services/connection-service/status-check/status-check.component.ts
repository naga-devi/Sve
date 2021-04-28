import { Component } from '@angular/core';
import { NotificationService } from '../../notification-service/notificationservice';
import { ConnectionService, ConnectionState } from '../lib/connection-service.service';

@Component({
    selector: 'jxnet-network-status-check',
    template: '',
    styleUrls: ['./status-check.component.scss']
})
export class StatusCheckComponent {

    currentState: ConnectionState;

    constructor(private connectionService: ConnectionService,
        public ns: NotificationService) {
        this.connectionService.monitor().subscribe((currentState: ConnectionState) => {
            //console.log(currentState);
            this.currentState = currentState;
            if (!currentState.hasNetworkConnection) {
                this.ns.error('You are Offline...');
            }
        });
    }
}
