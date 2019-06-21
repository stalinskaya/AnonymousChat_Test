import { Injectable } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class SearchService {

  constructor(private fb: FormBuilder, private http: HttpClient) { }
  readonly BaseURI = 'https://localhost:44355/api';

  formModel = this.fb.group({
    Gender: ['', Validators.required],
    AgeMin: ['', Validators.required],
    AgeMax: ['', Validators.required],
  });

  search() {
    var body = {
      Gender: this.formModel.value.Gender,
      AgeMin: this.formModel.value.AgeMin,
      AgeMax: this.formModel.value.AgeMax
    };
    return this.http.post(this.BaseURI + '/chat/UserSearch', body);
  }
  getUserProfile(id) {
    return this.http.get(this.BaseURI + '/User/', id);
  }
}
