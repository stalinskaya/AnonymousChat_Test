import { Component, OnInit } from '@angular/core';
import { SignalRService } from 'src/app/shared/signal-r.service';
import { Router } from '@angular/router';
import { ChatService } from 'src/app/shared/chat.service';


@Component({
  selector: 'app-chats',
  templateUrl: './chats.component.html',
  styles: []
})
export class ChatsComponent implements OnInit {
  dialogList;
  UserId;
  DialogId;
  visibleChatDetails = true;

  constructor(public signalR: SignalRService,
    public service: ChatService,
    private router: Router) { }

  ngOnInit() {
    this.service.getUserDialogs().subscribe(
      res => {
        this.dialogList = res;
      },
      err => {
        console.log(err);
      }
    );
  }

  onOpenDialog(userId: string, dialogId: string) {
    this.UserId = userId;
    this.DialogId = dialogId;
    this.visibleChatDetails = !this.visibleChatDetails;
  }
}
