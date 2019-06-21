import { Component, OnInit } from '@angular/core';
import { SearchService } from './../../shared/search.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styles: []
})
export class SearchComponent implements OnInit {

  constructor(private router: Router, private service: SearchService, private toastr: ToastrService) { }

  selectedLevel;
  data = ["female", "male"];

  selected(){
    console.log(this.selectedLevel)
  }

  ngOnInit() {
  }

  onLogout() {
    localStorage.removeItem('token');
    this.router.navigate(['/user/login']);
  }

  onProfile() {
    this.router.navigate(['/profile']);
  }

  onSubmit() {
    const {AgeMin, AgeMax} = this.service.formModel.controls;
    const isFormValid = AgeMin.value< AgeMax.value;
    if(isFormValid) {
      this.service.search().subscribe(
        (res: any) => {
          (function() {
            var getWebSocketId = function(onIdReceived)
            {
                var url = `ws://localhost:44355/api/usersearch`
                console.log('url is: ' + url);
                var webSocket = new WebSocket(url);
                webSocket.onmessage = onIdReceived;
            };
            getWebSocketId(function (id) {
              this.service.getUserProfile(id);
          });
          }());
          
        },
        err => {
          console.log(err);
        }
      );
    }
  }
}