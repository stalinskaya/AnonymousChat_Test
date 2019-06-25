import { UserService } from '../../shared/user.service';
import { Component, OnInit, RootRenderer } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  userDetails;
  selectedFile = null;
  visibleDetailsUser = true;

  constructor(private router: Router, private service: UserService, private toastr: ToastrService) {}

  selectedLevel;
  data = ["female", "male"];

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

  imageUrl: string = "assets\img\single_user.png";
  fileToUpload: File = null;

  handleFileInput(file: FileList) {
    this.fileToUpload = file.item(0);

    var reader = new FileReader();
    reader.onload = (event: any) => {
      this.imageUrl = event.target.result;
    }
    reader.readAsDataURL(this.fileToUpload);
  }

  onChangeImage() {
    this.service.postFile(this.fileToUpload).subscribe(
      (res: any) => {
        console.log('done');
        this.ngOnInit();
        this.toastr.success('Update!', 'Edit user infrormation successful.');
      },
      err => {
        console.log(err);
      }
    );
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
  
  onProfile() {
    this.router.navigate(['/profile']);
  }
  onLogout() {
    localStorage.removeItem('token');
    this.router.navigate(['/user/login']);
  }
}

