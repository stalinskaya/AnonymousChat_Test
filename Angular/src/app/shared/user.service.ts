import { Injectable } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { HttpClient, HttpHeaders } from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private fb: FormBuilder, private http: HttpClient) { }
  readonly BaseURI = 'https://localhost:44355/api';

  formModel = this.fb.group({
    FirstName: ['', Validators.required],
    LastName: ['', Validators.required],
    Gender: ['', Validators.required],
    Birthday: ['', Validators.required],
    Email: ['', Validators.email],
    Passwords: this.fb.group({
      Password: ['', [Validators.required]],
      ConfirmPassword: ['', Validators.required]
    }, { validator: this.comparePasswords })

  });

  comparePasswords(fb: FormGroup) {
    let confirmPswrdCtrl = fb.get('ConfirmPassword');
    //passwordMismatch
    //confirmPswrdCtrl.errors={passwordMismatch:true}
    if (confirmPswrdCtrl.errors == null || 'passwordMismatch' in confirmPswrdCtrl.errors) {
      if (fb.get('Password').value != confirmPswrdCtrl.value)
        confirmPswrdCtrl.setErrors({ passwordMismatch: true });
      else
        confirmPswrdCtrl.setErrors(null);
    }
  }

  register() {
    var body = {
      FirstName: this.formModel.value.FirstName,
      LastName: this.formModel.value.LastName,
      Gender: this.formModel.value.Sex,
      Birthday: this.formModel.value.Age,
      Email: this.formModel.value.Email,
      Password: this.formModel.value.Passwords.Password,
      PasswordConfirm: this.formModel.value.Passwords.ConfirmPassword
    };
    return this.http.post(this.BaseURI + '/account/Register', body);
  }

  login(formData) {
    return this.http.post(this.BaseURI + '/account/Login', formData);
  }

  getUserProfile() {
    return this.http.get(this.BaseURI + '/User');
  }
}
