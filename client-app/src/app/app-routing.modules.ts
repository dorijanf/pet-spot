import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { LoginComponent } from "./components/login/login.component";
import { RegisterComponent } from "./components/register/register.component";
import { AuthGuard } from '../app/helpers/auth.guard';
import { PageNotFoundComponent } from "./components/page-not-found/page-not-found.component";
import { MainContentComponent } from "./layout/main-content/main-content.component";
import { AnimalCreateComponent } from "./components/animal-create/animal-create.component";
import { AnimalDetailsComponent } from "./components/animal-details/animal-details.component";

const routes: Routes = [
  { path: '', component: MainContentComponent, pathMatch: 'full', canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'create', component: AnimalCreateComponent, canActivate: [AuthGuard] },
  { path: 'edit/:id', component: AnimalCreateComponent, canActivate: [AuthGuard] },
  { path: 'details/:id', component: AnimalDetailsComponent, canActivate: [AuthGuard] },
  { path: '**', component: PageNotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class AppRoutingModule { }
