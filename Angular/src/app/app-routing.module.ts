import { AuthGuard } from './auth/auth.guard';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UserComponent } from './user/user.component';

import { RegistrationComponent } from './user/registration/registration.component';
import { LoginComponent } from './user/login/login.component';
import { HomeComponent } from './home/home.component';
import { ProfileComponent } from './home/profile/profile.component';
import { SearchComponent } from './home/search/search.component';
import { ChatsComponent } from './home/chats/chats.component';
import { DialogComponent } from './home/chats/dialog/dialog.component';
import { DialogFromSearchComponent } from './home/dialog-from-search/dialog-from-search.component'
import { UserProfileComponent } from './home/user-profile/user-profile.component';

const routes: Routes = [
  {path:'',redirectTo:'/user/login',pathMatch:'full'},
  {
    path: 'user', component: UserComponent,
    children: [
      { path: 'registration', component: RegistrationComponent },
      { path: 'login', component: LoginComponent }
    ]
  },
  {
    path:'home',component: HomeComponent , canActivate:[AuthGuard],
    children: [
      {path:'profile',component:ProfileComponent, canActivate:[AuthGuard]},
      {path: 'search', component: SearchComponent, canActivate:[AuthGuard]},
      {path: 'dialog-from-search/:userId', component: DialogFromSearchComponent},
      {path: 'chats', component: ChatsComponent, canActivate:[AuthGuard],
        children: [
          {path: 'dialog', component: DialogComponent, canActivate:[AuthGuard]}
        ]
      },
      { path: 'user-profile', component: UserProfileComponent, canActivate:[AuthGuard] }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
