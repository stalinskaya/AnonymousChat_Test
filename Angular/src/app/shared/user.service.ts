import { Injectable } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { HttpClient} from "@angular/common/http";

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

  editModel = this.fb.group({
    FirstName: [''],
    LastName: [''],
    Birthday: [''],
    Gender: ['']
  })

  comparePasswords(fb: FormGroup) {
    let confirmPswrdCtrl = fb.get('ConfirmPassword');
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
      Gender: this.formModel.value.Gender,
      Birthday: this.formModel.value.Birthday,
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
  

  updateUserProfile() {
    var body = {
      Firstname: this.editModel.value.FirstName,
      Lastname: this.editModel.value.LastName,
      Birthday: this.editModel.value.Birthday,
      Gender: this.editModel.value.Gender
    };
    return this.http.put(this.BaseURI + '/user', body)
  }

  postFile(fileToUpload: File) {
    const formData: FormData = new FormData();
    formData.append('Image', fileToUpload);
    return this.http.put(this.BaseURI + '/user/EditPhoto', formData);
  }
}
