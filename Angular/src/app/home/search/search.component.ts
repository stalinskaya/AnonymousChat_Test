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
    this.spinnerService.show();
    setTimeout(() => {
      this.service.search().subscribe(
        (res: any) => {
          this.spinnerService.hide();
          //this.router.navigate(['dialog'], {queryParams: {userId: res}});
        },
        err => {
          this.spinnerService.hide();
          console.log(err);
        }
        )}, 32000);
    }
  }
}