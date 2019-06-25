import { Injectable } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { MessageInfo } from '../models/MessageInfo';

@Injectable({
    providedIn: 'root'
})
export class ChatService {

    constructor(private fb: FormBuilder,
        private http: HttpClient) { }

    readonly BaseURI = 'https://localhost:44355/api';
    
    getUserDialogs() {
        return this.http.get(this.BaseURI + '/chat/GetAllDialogs');
    }

    getDetailsUserDialogs(id: string){
        return this.http.get(this.BaseURI + '/chat/DialogDetails/' + id)
    }

    sendMessage(outgoingMessage: MessageInfo){
        console.log(outgoingMessage);
        return this.http.post(this.BaseURI + '/chat/SendMessage', outgoingMessage);
    }
}