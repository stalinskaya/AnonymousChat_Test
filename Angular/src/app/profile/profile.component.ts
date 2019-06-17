import { UserService } from './../shared/user.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styles: []
})
export class ProfileComponent implements OnInit {
  userDetails;
  selectedFile = null;
  visibleDetailsUser = true;

  constructor(private router: Router, private service: UserService, private toastr: ToastrService) { }

  selectedLevel;
  data = ["female", "male", "no matter" ];

  selected(){
    console.log(this.selectedLevel)
  }

  ngOnInit() {
    this.service.getUserProfile().subscribe(
      res => {
        this.userDetails = res;
      },
      err => {
        console.log(err);
      },
    );
  }

  imageUrl: string = "/assets/img/add.jpg";
  url = '';
  onSelectFile(event) {
    if (event.target.files && event.target.files[0]) {
      var reader = new FileReader();

      reader.readAsDataURL(event.target.files[0]); // read file as data url

    }
  }

  onEditUser() {
    this.visibleDetailsUser = !this.visibleDetailsUser;
  }

  onSubmit() {
    this.service.updateUserProfile().subscribe(
      (res: any) => {
        this.toastr.success('Update!', 'Edit user infrormation successful.');
        this.ngOnInit();
        this.onEditUser();
        console.log('update');
      },
      err => {
        if (err.status == 400)
          this.toastr.error('-________-', 'Edit user infrormation failed');
        else
          console.log(err);
      }
    )
  }
  
  onLogout() {
    localStorage.removeItem('token');
    this.router.navigate(['/user/login']);
  }

}

