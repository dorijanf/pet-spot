import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatSidenavModule} from '@angular/material/sidenav';
import { AppComponent } from './layout/app.component';
import { HeaderComponent } from './layout/header/header.component';
import { LeftPanelComponent } from './layout/left-panel/left-panel.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { ListComponent } from './components/list/list.component';
import { Authentication } from './helpers/authentication';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    LeftPanelComponent,
    LoginComponent,
    RegisterComponent,
    ListComponent,
  ],
  imports: [
    BrowserModule,
    MatInputModule,
    MatFormFieldModule,
    MatSidenavModule,
  ],
  providers: [Authentication],
  bootstrap: [AppComponent]
})
export class AppModule { }
