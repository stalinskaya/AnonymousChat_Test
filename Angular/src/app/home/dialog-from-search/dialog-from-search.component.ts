import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { ChatService } from 'src/app/shared/chat.service';
import { SignalRService } from 'src/app/shared/signal-r.service';
import { Message } from 'src/app/models/Message';
import { SearchService } from 'src/app/shared/search.service';
import { MessageInfo } from 'src/app/models/MessageInfo';

@Component({
  selector: 'app-dialog',
  templateUrl: './dialog-from-search.component.html',
  styleUrls: ['./dialog-from-search.component.css']
})


export class DialogFromSearchComponent implements OnInit {

  @Input() userId: string;

  constructor(private activateRoute: ActivatedRoute, public signalR: SignalRService, public search: SearchService, private router: Router, public service: ChatService) { }

  message = '';
  messages: Message[] = new Array();
  messagesRealTime: Message[] = new Array();
  
  incomingMessage =  new Message();

  visibleDropZone = true;

async ngOnInit() {
  
  await this.activateRoute.params.subscribe(params => this.userId = params.userId);
  
  this.signalR.startConnection();
  this.search.getUserChat(this.userId).subscribe(
    res => {
      console.log(res);
    },
    err => {
      console.log(err);
    }
  )
  this.addSendListener();
  this.addSendMyselfListener();
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

    console.log(this.userId, this.message);

    this.service.sendMessage(outgoingMessage).subscribe();
  }
}
