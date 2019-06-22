import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import {ActivatedRoute} from '@angular/router';
import {HubConnection, HttpTransportType} from '@aspnet/signalr';
import * as signalR from '@aspnet/signalr';

@Component({
  selector: 'app-dialog',
  templateUrl: './dialog.component.html',
  styleUrls: ['./dialog.component.css']
})

export class DialogComponent implements OnInit {
  public _hubConnection: HubConnection;
  public message: string;
  public _hubConnecton: HubConnection;
  // msgs: Message[] = [];
  private token = localStorage.getItem('token');

constructor(private activatedRoute: ActivatedRoute) { }

  ngOnInit() {
    this.activatedRoute.queryParams.subscribe((params) => {
      console.log(params['userId'])});
      
    
    // this.startConnection();
    //   this.service.getUserDialogs().subscribe(
    //     res => {
    //       this.userList = res;
    //     },
    //     err => {
    //       console.log(err);
    //     }
    //   );
  }
}

    
  // public startConnection = () => {
  //       this.hubConnection = new signalR.HubConnectionBuilder()
  //           .withUrl('https://localhost:44355/chat',
  //               { 
  //                   skipNegotiation: true, 
  //                   transport: HttpTransportType.WebSockets,
  //                   accessTokenFactory: () => this.token
  //               })
  //           .build();

  //       this.hubConnection
  //           .start()
  //           .then(() => console.log('Connection started'))
  //           .catch(err => console.log('Error while starting connection: ' + err))
  //   }
  //   message = '';
  //   //messages: string[] = [];
  
  //   messages: Message[] = new Array();
  //   outgoingMessage  = new MessageInfo();
  
  //   //messagesFromDb;
  
  //   ngOnInit() {
  //     this.signalR.startConnection();
  //     this.addSendListener();
  //     this.addSendMyselfListener();
  
  //     this.service.getDetailsUserDialogs(this.dialogId).subscribe(
  //       res => {
  //         // this.messagesFromDb = res;
  //         this.messages = res as Message[];
  //       },
  //       err => {
  //         console.log(err);
  //       }
  //     )
  //   }
  
  //   addSendListener() {
  //     this.hubConnection.on('Send', (data) => {
  //       this.incomingMessage = data as Message;
  //       this.messages.push(this.signalR.incomingMessage);
  //       // this.messages.push(message);
  //     });
  //   }
  
  //   addSendMyselfListener() {
  //     this.hubConnection.on('SendMyself', (data) => {
  //       this.signalR.incomingMessage = data as Message;
  //       this.messages.push(this.signalR.incomingMessage);
        
  //       // this.messages.push(message);
  //     });
  //   }
  
  //   addNewDialogListener() {
  //     this.hubConnection.on('AddNewDialog', (data) => {
  //       this.signalR.incomingMessage = data as Message;
  //       this.messages.push(this.signalR.incomingMessage);
  
  //       // this.messages.push(message);
  //     });
  //   }
  
  //   onSendMessage() {
  //     this.outgoingMessage.DialogId = this.dialogId;
  //     this.outgoingMessage.ReceiverId = this.userId;
  //     this.outgoingMessage.Message = this.message;
  //     this.Send(this.outgoingMessage);
  //   }