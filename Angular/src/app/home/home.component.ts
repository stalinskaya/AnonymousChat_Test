import { UserService } from './../shared/user.service';
import { SearchService } from './../shared/search.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styles: []
})
export class HomeComponent implements OnInit {

  constructor(private router: Router, private service: SearchService, private toastr: ToastrService) { 
  }

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
    const isFormValid =AgeMin.value< AgeMax.value;
    debugger
    if(isFormValid) {
      this.service.search().subscribe(
        (res: any) => {
            this.service.formModel.reset();
        },
        err => {
          console.log(err);
        }
      );
    }
  }
}

