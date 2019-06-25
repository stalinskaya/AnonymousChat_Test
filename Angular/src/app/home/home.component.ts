import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styles: []
})
export class HomeComponent implements OnInit {

  constructor(private router: Router) { 
  }

  ngOnInit() {
  }
  
  onHome() {
    this.router.navigate(['/home']);
  }
  onSearch() {
    this.router.navigate(['/home/search']);
  }
  onProfile() {
    this.router.navigate(['home/profile']);
  }
  onChats() {
    this.router.navigate(['home/chats']);
  }
  onLogout() {
    localStorage.removeItem('token');
    this.router.navigate(['/user/login']);
  }
}

