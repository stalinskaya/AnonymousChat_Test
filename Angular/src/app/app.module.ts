import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule, FormsModule } from "@angular/forms";
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import { Ng4LoadingSpinnerModule} from 'ng4-loading-spinner';
import { CommonModule } from '@angular/common';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { UserComponent } from './user/user.component';
import { RegistrationComponent } from './user/registration/registration.component';
import { UserService } from './shared/user.service';
import { LoginComponent } from './user/login/login.component';
import { HomeComponent } from './home/home.component';
import { AuthInterceptor } from './auth/auth.interceptor';
import { ProfileComponent } from './home/profile/profile.component';
import { SearchComponent } from './home/search/search.component';
import { ChatsComponent } from './home/chats/chats.component';
import { DialogComponent } from './home/chats/dialog/dialog.component';
import { UserProfileComponent } from './home/user-profile/user-profile.component';
import { DialogFromSearchComponent } from './home/dialog-from-search/dialog-from-search.component';

@NgModule({
  declarations: [
    AppComponent,
    UserComponent,
    RegistrationComponent,
    LoginComponent,
    HomeComponent,
    ProfileComponent,
    SearchComponent,
    ChatsComponent,
    DialogComponent,
    UserProfileComponent,
    DialogFromSearchComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot({
      progressBar: true
    }),
    FormsModule,
    BrowserAnimationsModule,
    NgbModule,
    Ng4LoadingSpinnerModule.forRoot(),
    CommonModule
    
  ],
  exports: [
    BrowserAnimationsModule
  ],
  providers: [UserService, {
    provide: HTTP_INTERCEPTORS,
    useClass: AuthInterceptor,
    multi: true
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }
