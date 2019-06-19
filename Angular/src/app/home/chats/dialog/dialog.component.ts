import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import {HubConnection, HubConnectionBuilder, HttpTransportType} from '@aspnet/signalr';

@Component({
  selector: 'app-dialog',
  templateUrl: './dialog.component.html',
  styleUrls: ['./dialog.component.css']
})
export class DialogComponent implements OnInit {
  public _hubConnection: HubConnection;
  public messages: string[] = [];
  public message: string;

constructor(private router: Router) { }

  ngOnInit() {
    const hubConnection = new HubConnectionBuilder()
      .withUrl("home/chats/dialog", { skipNegotiation: true,
      transport: HttpTransportType.WebSockets, accessTokenFactory: () => localStorage.getItem('token')})
      .build();

      this._hubConnection.on("Send", (message) => {
        this.messages.push(message);
      });
  
      // starting the connection
      this._hubConnection.start();
    }
  
  send() {
    // message sent from the client to the server
    this._hubConnection.invoke("Echo", this.message);
    this.message = "";
  }
}