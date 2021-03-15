import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { ListComponent } from "./components/list/list.component";
import { LoginComponent } from "./components/login/login.component";
import { LogoutComponent } from "./components/login/logout.component";
import { RegisterComponent } from "./components/register/register.component";
import { AuthGuard } from '../app/helpers/auth.guard';

const routes: Routes = [
  { path: '', pathMatch: 'full'},
  { path: 'login', component: LoginComponent },
  { path: 'logout', component: LogoutComponent, canActivate: [AuthGuard]},
  { path: 'register', component: RegisterComponent},
  { path: 'list', component: ListComponent, canActivate: [AuthGuard]}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class AppRoutingModule {}
