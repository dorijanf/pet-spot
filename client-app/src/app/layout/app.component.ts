import { Component } from '@angular/core';
import { AccountService } from 'src/services/account.service';
import { Authentication } from '../helpers/authentication';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  isAuthenticated: boolean;
  title = 'PetSpot';

  constructor(private authentication: Authentication) {

  }

  ngOnInit() {
    this.isAuthenticated = this.authentication.isAuthenticated();
  }

}
