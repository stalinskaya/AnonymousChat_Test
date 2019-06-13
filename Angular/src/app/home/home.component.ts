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
  data = ["female", "male", "no matter" ];

  selected(){
    console.log(this.selectedLevel)
  }

  ngOnInit() {  
  }


  onLogout() {
    localStorage.removeItem('token');
    this.router.navigate(['/user/login']);
  }

  onSubmit() {
    this.service.search().subscribe(
      (res: any) => {
        if (res.succeeded) {
          this.service.formModel.reset();
          this.toastr.success('Search successful', 'Search successful.');
        } else {
          res.errors.forEach(element => {
            switch (element.code) {
              case 'DuplicateUserName':
                this.toastr.error('Username is already taken','Registration failed.');
                break;

              default:
              this.toastr.error(element.description,'Search failed.');
                break;
            }
          });
        }
      },
      err => {
        console.log(err);
      }
    );
  }
}

