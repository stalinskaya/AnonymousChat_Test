import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';
import {ActivatedRoute} from '@angular/router';
import {HubConnection, HttpTransportType} from '@aspnet/signalr';
import { ChatService } from 'src/app/shared/chat.service';
import { MessageInfo } from 'src/app/models/MessageInfo';
import { SignalRService } from 'src/app/shared/signal-r.service';
import { Message } from 'src/app/models/Message';

@Component({
  selector: 'app-dialog',
  templateUrl: './dialog.component.html',
  styleUrls: ['./dialog.component.css']
})

export class DialogComponent implements OnInit {

  @Input() userId: string;
  @Input() dialogId: string;

  constructor(private activateRoute: ActivatedRoute,
    public signalR: SignalRService,
    private router: Router,
    public service: ChatService) { }

  message = '';
  messages: Message[] = new Array();
  messagesRealTime: Message[] = new Array();
  
  incomingMessage =  new Message();

  visibleDropZone = true;

  async ngOnInit() {
    await this.activateRoute.params.subscribe(params => this.userId = params.id);
    this.signalR.startConnection();
    this.addSendListener();
    this.addSendMyselfListener();
    this.service.getDetailsUserDialogs(this.userId).subscribe(
      res => {
        this.messages = res as Message[];
      },
      err => {
        console.log(err);
      }
    )
  }

  addSendListener() {
    this.signalR.hubConnection.on('Send', (data) => {    
      this.incomingMessage = data as Message;
      this.messagesRealTime.push(this.incomingMessage);
    });
  }

  addSendMyselfListener() {
      this.signalR.hubConnection.on('SendMyself', (data) => {
        this.incomingMessage = data as Message;
        this.messagesRealTime.push(this.incomingMessage);
    });
  }

  addNewDialogListener() {
      this.signalR.hubConnection.on('AddNewDialog', (data) => {
        this.incomingMessage = data as Message;
        this.messagesRealTime.push(this.incomingMessage);
    });
  }

  onSendMessage() {
    var outgoingMessage  = new MessageInfo();
    outgoingMessage.ReceiverId = this.userId;
    outgoingMessage.Message = this.message;

    console.log(this.dialogId, this.userId, this.message);

    this.service.sendMessage(outgoingMessage).subscribe();
  }
}
