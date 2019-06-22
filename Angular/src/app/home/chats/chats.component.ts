import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import * as signalR from "@aspnet/signalr";
import { HttpTransportType } from '@aspnet/signalr';


@Component({
  selector: 'app-chats',
  templateUrl: './chats.component.html',
  styles: []
})
export class ChatsComponent implements OnInit {

  constructor(private router: ActivatedRoute) { }

  ngOnInit() {
    
  }

}
