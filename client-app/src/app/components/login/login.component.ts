import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AccountService } from 'src/services/account.service';
import { FormArray } from '@angular/forms'
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})

export class LoginComponent implements OnInit {
  hide = true;
  authenticationForm = new FormGroup({
    username: new FormControl('', Validators.required),
    password: new FormControl('', Validators.required)
  })
  constructor(private router: Router, private accountService: AccountService) {

  }

  ngOnInit(): void {
  }

  get username() { return this.authenticationForm.get('username'); }
  get password() { return this.authenticationForm.get('password'); }

  onSubmit() {
    this.accountService.login(this.authenticationForm.value)
      .subscribe(result => {
        this.router.navigate([''])
      },
        error => console.error(error));
  }

}
