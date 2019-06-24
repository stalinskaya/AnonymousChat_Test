import { Component, OnInit } from '@angular/core';
import { SearchService } from './../../shared/search.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styles: []
})

export class SearchComponent implements OnInit {

  constructor(private router: Router, private service: SearchService, private toastr: ToastrService, private spinnerService: Ng4LoadingSpinnerService) { }

  selectedLevel;
  data = ["female", "male"];
  userId;

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
          if (res == null) {
            this.router.navigate(['home/search']);
          }
          this.router.navigateByUrl('/home/dialog-from-search/' + res);
        },
        err => {
          console.log(err);
        });
    }
  }
}