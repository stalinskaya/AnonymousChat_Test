import { UserService } from './../../shared/user.service';
import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styles: ["node_modules/bootstrap/dist/css/bootstrap.min.css"]
})
export class RegistrationComponent implements OnInit {
  constructor(public service: UserService, private toastr: ToastrService) { }

  selectedLevel;
  data = ["female", "male", "no matter" ];

  ngOnInit() {
    this.service.formModel.reset();
  }

  selected(){
    console.log(this.selectedLevel)
  }
  
  onSubmit() {
    this.service.register().subscribe(
      (res: any) => {
        if (res.succeeded) {
          this.service.formModel.reset();
          this.toastr.success('New user created!', 'Registration successful.');
        } else {
          res.errors.forEach(element => {
            switch (element.code) {
              case 'DuplicateUserName':
                this.toastr.error('Username is already taken','Registration failed.');
                break;

              default:
              this.toastr.error(element.description,'Registration failed.');
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