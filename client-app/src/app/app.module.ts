import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { AppComponent } from './layout/app.component';
import { HeaderComponent } from './layout/header/header.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.modules';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppConfig } from './configuration/config';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { JwtInterceptor } from './helpers/jwt.interceptor';
import { AccountService } from 'src/services/account.service';
import { AnimalsService } from 'src/services/animals.service';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MainContentComponent } from './layout/main-content/main-content.component';
import { AnimalListComponent } from './components/animal-list/animal-list.component';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { AnimalCreateComponent } from './components/animal-create/animal-create.component';
import { MatSelectModule } from '@angular/material/select';
import { MatDialogModule } from '@angular/material/dialog';
import { MatCardModule } from '@angular/material/card';
import { AnimalDetailsComponent } from './components/animal-details/animal-details.component';
import { AnimalDeleteComponent } from './components/animal-delete/animal-delete.component';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    LoginComponent,
    RegisterComponent,
    PageNotFoundComponent,
    MainContentComponent,
    AnimalListComponent,
    AnimalCreateComponent,
    AnimalDetailsComponent,
    AnimalDeleteComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    MatToolbarModule,
    MatButtonModule,
    MatInputModule,
    MatIconModule,
    MatFormFieldModule,
    MatDialogModule,
    MatSidenavModule,
    MatSelectModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatCardModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptor,
      multi: true
    },
    AppConfig,
    AccountService,
    AnimalsService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
