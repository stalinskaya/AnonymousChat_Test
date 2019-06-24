import { Injectable } from '@angular/core';
import * as signalR from "@aspnet/signalr";
import { HttpTransportType } from '@aspnet/signalr';
import { MessageInfo } from '../models/MessageInfo';


@Injectable({
    providedIn: 'root'
})

export class SignalRService {

    public hubConnection: signalR.HubConnection
    private token = localStorage.getItem('token');

    constructor() { }

    public startConnection = () => {
        this.hubConnection = new signalR.HubConnectionBuilder()
            .withUrl('https://localhost:44355/chat',
                {
                    skipNegotiation: true,
                    transport: HttpTransportType.WebSockets,
                    accessTokenFactory: () => this.token
                })
            .build();

        this.hubConnection
            .start()
            .then(() => console.log('Connection started'))
            .catch(err => console.log('Error while starting connection: ' + err))
    }
    

    Send(outgoingMessage: MessageInfo): void {
        this.hubConnection
            .invoke('Send', outgoingMessage)
            .catch(err => console.error(err));
    }
}